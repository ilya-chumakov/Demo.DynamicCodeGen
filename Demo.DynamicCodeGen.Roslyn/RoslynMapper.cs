using System;
using System.Collections.Generic;

namespace Demo.DynamicCodeGen.Roslyn
{
    public static class RoslynMapper
    {
        public static Action<TSrc, TDest> CreateMapMethod<TSrc, TDest>()
            where TSrc : class, new()
            where TDest : class, new()
        {
            var context = MapContext.Create<TSrc, TDest>();

            string text = MapperTextBuilder.CreateText(context);

            var type = MapperTypeBuilder.GetMapperType(text, context);

            return (Action<TSrc, TDest>)
                Delegate.CreateDelegate(typeof(Action<TSrc, TDest>), type, context.MapperMethodName);
        }
    }
}
