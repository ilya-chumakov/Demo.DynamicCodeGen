using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Demo.DynamicCodeGen.Emit
{
    public static class MsilGen
    {
        public static void EmitMapMethodBody(MethodBuilder methodBuilder, Type srcType, Type destType)
        {
            var gen = methodBuilder.GetILGenerator();

            var prmSrc = methodBuilder.DefineParameter(1, ParameterAttributes.None, "source");
            var prmDest = methodBuilder.DefineParameter(2, ParameterAttributes.None, "target");

            var srcProperties = srcType.GetProperties();
            var destProperties = destType.GetProperties();

            foreach (var srcProperty in srcProperties)
            {
                string name = srcProperty.Name;
                var destProperty = destProperties.First(p => p.Name == name);

                //int -> double doesn't work  without explicit conversion OpCodes.Conv_R8
                //if (destProperty.PropertyType.IsAssignableFrom(srcProperty.PropertyType))
                {
                    var methodSrc = srcType.GetProperty(name).GetGetMethod();
                    var methodTarg = destType.GetProperty(name).GetSetMethod();

                    // Generate target.methodTarg = source.methodSrc;
                    gen.Emit(OpCodes.Ldarg, prmDest.Position - 1);
                    gen.Emit(OpCodes.Ldarg, prmSrc.Position - 1);
                    gen.Emit(OpCodes.Callvirt, methodSrc);
                    //gen.Emit(OpCodes.Conv_R8);
                    gen.Emit(OpCodes.Callvirt, methodTarg);
                }
                //else custom map
            }
            gen.Emit(OpCodes.Ret);
        }
    }
}