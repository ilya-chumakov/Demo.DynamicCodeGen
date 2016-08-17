using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Demo.DynamicCodeGen.Roslyn
{
    public static class MapperTypeBuilder
    {
        public static Type GetMapperType<TInput, TOutput>(string text, Type srcType, Type destType) where TInput : class, new()
            where TOutput : class, new()
        {
            var compilation = CreateCompilation(text, srcType, destType);

            var assembly = CreateAssembly(compilation);

            Type type = assembly.GetType("RoslynCompileSample.Mapper");
            return type;
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
    }
}