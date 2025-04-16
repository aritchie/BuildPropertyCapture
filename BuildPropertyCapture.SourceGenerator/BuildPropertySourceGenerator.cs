using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace BuildPropertyCapture.SourceGenerator;


[Generator(LanguageNames.CSharp)]
public class BuildPropertySourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // we'll wrap code in ifdef as it is safer than checking all of the various pieces involved here
        // ((CSharpCompilation)context.Compilation).LanguageVersion == LanguageVersion.CSharp9
            
        var opts = context.AnalyzerConfigOptions.GlobalOptions;
        var properties = opts.Keys.Where(x => x.StartsWith("build_property.")).ToList();
        if (properties.Count == 0)
            return;

        if (!opts.TryGetValue("build_property.debugpropertycapture", out var debug) && (debug?.Equals("true", StringComparison.InvariantCultureIgnoreCase) ?? false))
            Debugger.Break();
        
        // we only want to generate code where host setup takes place
        if (!opts.TryGetValue("build_property.outputtype", out var outputType))
            return;

        if (!outputType.Equals("exe", StringComparison.InvariantCultureIgnoreCase))
            return;
        
        var sb = new StringBuilder();
        sb
            .AppendLine("#if NET7_0_OR_GREATER")
            .AppendLine("using System;")
            .AppendLine("using System.Collections.Generic;")
            .AppendLine()
            .AppendLine("namespace BuildPropertyCapture;")
            .AppendLine()
            .AppendLine("[global::System.Runtime.CompilerServices.CompilerGenerated]")
            .AppendLine("public static class BuildVariableInitializer")
            .AppendLine("{")
            .AppendLine("\t[global::System.Runtime.CompilerServices.ModuleInitializer]")
            .AppendLine("\tpublic static void Initialize()")
            .AppendLine("\t{")
            .AppendLine("\t\tglobal::BuildPropertyCapture.BuildVariables.Initialize(new global::System.Collections.Generic.Dictionary<string, string> {");
        
        foreach (var property in properties)
        {
            if (opts.TryGetValue(property, out var value))
            {
                var pn = EscapeString(property.Replace("build_property.", ""));
                var ev = EscapeString(value);
                sb
                    .Append("\t\t\t{")
                    .Append($"\"{pn}\", \"{ev}\"")
                    .Append("},\r\n");
            }
        }

        sb
            .AppendLine("\t\t});") // close dictionary
            .AppendLine("\t}")
            .AppendLine("}")
            .AppendLine("#endif");
        
        context.AddSource("__BuildVariables.g.cs", sb.ToString());
    }


    static string EscapeString(string value) => value.Replace("\\", "\\\\");
}