using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Demo.DynamicCodeGen.ExpressionTrees
{
    internal static class MapperTypeBuilder
    {
        public static Type CreateMapperType<TInput, TOutput>(Expression<Action<TInput, TOutput>> expression)
        {
            var typeBuilder = CreateTypeBuilder<TInput, TOutput>();

            var methodBuilder = typeBuilder.DefineMethod("Map", MethodAttributes.Public | MethodAttributes.Static);

            expression.CompileToMethod(methodBuilder);

            return typeBuilder.CreateType();
        }

        private static TypeBuilder CreateTypeBuilder<TInput, TOutput>()
        {
            var assemblyName = new AssemblyName("ThisMemberFunctionsAssembly_" + Guid.NewGuid().ToString("N"));

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Module");

            string name = $"From_{typeof (TInput).Name}_to_{typeof (TOutput).Name}_{Guid.NewGuid().ToString("N")}";

            var typeBuilder = moduleBuilder.DefineType(name);

            return typeBuilder;
        }
    }
}