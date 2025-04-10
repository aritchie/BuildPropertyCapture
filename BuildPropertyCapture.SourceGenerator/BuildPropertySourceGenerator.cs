using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace BuildPropertyCapture.SourceGenerator;

[Generator]
public class BuildPropertySourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var opts = context.AnalyzerConfigOptions.GlobalOptions;
        var properties = opts.Keys.Where(x => x.StartsWith("build_property.")).ToList();
        if (properties.Count == 0)
            return;

        // we only want to generate code where host setup takes place
        if (!opts.TryGetValue("build_property.outputtype", out var outputType))
            return;

        if (!outputType.Equals("exe", StringComparison.InvariantCultureIgnoreCase))
            return;

        var sb = new StringBuilder();
        sb
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
                var pn = property.Replace("build_property.", "");
                var ev = value.Replace("\\", "\\\\");
                sb
                    .Append("\t\t\t{")
                    .Append($"\"{pn}\", \"{ev}\"")
                    .Append("},\r\n");
            }
        }

        sb
            .AppendLine("\t\t});") // close dictionary
            .AppendLine("\t}")
            .AppendLine("}");
        
        context.AddSource("__BuildVariables.g.cs", sb.ToString());
    }
}