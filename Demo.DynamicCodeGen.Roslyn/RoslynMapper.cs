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
            var srcType = typeof(TSrc);
            var destType = typeof(TDest);

            string text = MapperTextBuilder.CreateText(srcType, destType);

            var type = MapperTypeBuilder.GetMapperType(text, srcType, destType);

            return (Action<TSrc, TDest>)
                Delegate.CreateDelegate(typeof(Action<TSrc, TDest>), type, "Map");
        }
    }
}
