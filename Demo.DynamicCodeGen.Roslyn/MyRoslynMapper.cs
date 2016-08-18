using System;
using System.Collections.Generic;
using Demo.DynamicCodeGen.Common;

namespace Demo.DynamicCodeGen.Roslyn
{
    public class MyRoslynMapper : ITestableMapper
    {
        public static MyRoslynMapper Instance => new MyRoslynMapper();

        public Action<TSrc, TDest> CreateMapMethod<TSrc, TDest>()
        {
            var context = MapContext.Create<TSrc, TDest>();

            string text = MapperTextBuilder.CreateText(context);

            var type = MapperTypeBuilder.CreateMapperType(text, context);

            return (Action<TSrc, TDest>)
                Delegate.CreateDelegate(typeof(Action<TSrc, TDest>), type, context.MapperMethodName);
        }
    }
}
