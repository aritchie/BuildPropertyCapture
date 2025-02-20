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

        opts.TryGetValue("build_property.rootnamespace", out var rootNamespace);

        var sb = new StringBuilder();
        sb
            .AppendLine("using System;")
            .AppendLine("using System.Collections.Generic;")
            .AppendLine()
            .AppendLine($"namespace {rootNamespace};")
            .AppendLine()
            .AppendLine("public class __BuildProperties : global::BuildPropertyCapture.IBuildProperties")
            .AppendLine("{")
            .AppendLine("\tpublic IReadOnlyDictionary<string, string> Properties { get; } = new Dictionary<string, string> {");
        
        foreach (var property in properties)
        {
            if (opts.TryGetValue(property, out var value))
            {
                var pn = property.Replace("build_property.", "");
                var ev = value.Replace("\\", "\\\\");
                sb
                    .Append("\t\t{")
                    .Append($"\"{pn}\", \"{ev}\"")
                    .Append("},\r\n");
            }
        }

        sb
            .AppendLine("\t};") // close dictionary
            .AppendLine("}");
        
        context.AddSource("__BuildProperties.g.cs", sb.ToString());
        
        // TODO: could use a module initializer that sets a static class OR an IMauiInitializeService on mobile (this could be too late in the process to use though)
        // [ModuleInitializer]
        // [SuppressMessage("Usage", "CA2255:The \'ModuleInitializer\' attribute should not be used in libraries")]
        var content = new StringBuilder()
            .AppendLine("using Microsoft.Extensions.DependencyInjection;")
            .AppendLine()
            .AppendLine($"namespace {rootNamespace};")
            .AppendLine()
            .AppendLine("public static class __BuildPropertiesRegistration")
            .AppendLine("{")
            .AppendLine("\tpublic static IServiceCollection AddBuildProperties(this IServiceCollection services)")
            .AppendLine("\t{")
            .AppendLine($"\t\tservices.AddSingleton<global::BuildPropertyCapture.IBuildProperties, global::{rootNamespace}.__BuildProperties>();")
            .AppendLine("\t\treturn services;")
            .AppendLine("\t} ")
            .AppendLine("}")
            .ToString();

        context.AddSource("__BuildPropertiesRegistration.g.cs", content);
    }
}