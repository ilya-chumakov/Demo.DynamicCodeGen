using System;
using System.Linq;
using System.Text;

namespace Demo.DynamicCodeGen.Roslyn
{
    public static class MapperTextBuilder
    {
        public static string CreateText(Type srcType, Type destType)
        {
            var srcProperties = srcType.GetProperties();
            var destProperties = destType.GetProperties();

            var builder = new StringBuilder();
            builder.AppendLine("using System;                                                       ");
            builder.AppendLine("namespace RoslynCompileSample                                       ");
            builder.AppendLine("{                                                                   ");
            builder.AppendLine("    public static class Mapper                                      ");
            builder.AppendLine("    {                                                               ");

            builder.AppendLine($"       public static void Map({srcType.FullName} src, {destType.FullName} dest)");
            builder.AppendLine("        {");

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

            builder.AppendLine("        }");

            builder.AppendLine("    }                                                               ");
            builder.AppendLine("}");

            return builder.ToString();
        }
    }
}