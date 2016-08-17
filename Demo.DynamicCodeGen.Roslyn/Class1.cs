using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Demo.DynamicCodeGen.Roslyn
{
    public static class RoslynMapperV1
    {
        public static Action<TInput, TOutput> CreateFunc<TInput, TOutput>()
            where TInput : class, new()
            where TOutput : class, new()
        {
            var srcType = typeof(TInput);
            var destType = typeof(TOutput);

            string text = CreateText<TInput, TOutput>();

            var compilation = CreateCompilation(text, srcType, destType);

            var assembly = CreateAssembly(compilation);

            Type type = assembly.GetType("RoslynCompileSample.Mapper");

            return (Action<TInput, TOutput>)
                Delegate.CreateDelegate(typeof(Action<TInput, TOutput>), type, "Map");
        }

        private static CSharpCompilation CreateCompilation(string text, Type srcType, Type destType)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(text);

            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = {
                MetadataReference.CreateFromFile(typeof (object).Assembly.Location),
                MetadataReference.CreateFromFile(srcType.Assembly.Location),
                MetadataReference.CreateFromFile(destType.Assembly.Location),
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(
                    outputKind: OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release));
            return compilation;
        }

        private static Assembly CreateAssembly(CSharpCompilation compilation)
        {
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    return assembly;
                }
            }
            return null;
        }

        public static string CreateText<TInput, TOutput>()
            where TInput : class, new()
            where TOutput : class, new()
        {
            var srcType = typeof(TInput);
            var destType = typeof(TOutput);

            var srcProperties = srcType.GetProperties();
            var destProperties = destType.GetProperties();

            var builder = new StringBuilder();
            builder.AppendLine("using System;                                                       ");
            builder.AppendLine("namespace RoslynCompileSample                                       ");
            builder.AppendLine("{                                                                   ");
            builder.AppendLine("    public static class Mapper                                             ");
            builder.AppendLine("    {                                                               ");

            builder.AppendLine($"public static void Map({srcType.FullName} src, {destType.FullName} dest)");
            builder.AppendLine("{");

            foreach (var srcProperty in srcProperties)
            {
                string name = srcProperty.Name;

                var destProperty = destProperties.First(p => p.Name == name);

                if (destProperty.PropertyType.IsAssignableFrom(srcProperty.PropertyType))
                {
                    builder.AppendLine($"dest.{name} = src.{name};");
                }
                else
                {
                    //custom map
                }
            }

            builder.AppendLine("}");

            builder.AppendLine("    }                                                               ");
            builder.AppendLine("}");

            return builder.ToString();
        }
    }
}
