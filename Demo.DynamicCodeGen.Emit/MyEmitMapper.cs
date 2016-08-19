using System;
using System.Reflection;
using System.Reflection.Emit;
using Demo.DynamicCodeGen.Common;

namespace Demo.DynamicCodeGen.Emit
{
    public class MyEmitMapper : ITestableMapper
    {
        public static ITestableMapper Instance => new MyEmitMapper();

        public Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>()
        {
            var srcType = typeof(TInput);
            var destType = typeof(TOutput);

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName("Test.Gen, Version=1.0.0.1"),
                AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Test.Gen.Mod", true);

            var typeBuilder = moduleBuilder.DefineType(
                "Test.MapperOne",
                TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Public);

            var methodBuilder = typeBuilder.DefineMethod(
                "callme",
                MethodAttributes.Static,
                typeof(void),
                new[] { srcType, destType });

            MsilGen.EmitMapMethodBody(methodBuilder, srcType, destType);

            var finalType = typeBuilder.CreateType(); //MANDATORY CALL

            var methodToken = methodBuilder.GetToken().Token;
            var methodInfo = moduleBuilder.ResolveMethod(methodToken);

            return (Action<TInput, TOutput>)Delegate.CreateDelegate(typeof(Action<TInput, TOutput>), (MethodInfo)methodInfo);
        }
    }
}