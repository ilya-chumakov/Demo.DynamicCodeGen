using System;
using System.Collections.Generic;

namespace Demo.DynamicCodeGen.Roslyn
{
    public static class RoslynMapper
    {
        public static Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>()
            where TInput : class, new()
            where TOutput : class, new()
        {
            var srcType = typeof(TInput);
            var destType = typeof(TOutput);

            string text = MapperTextBuilder.CreateText<TInput, TOutput>();

            var type = MapperTypeBuilder.GetMapperType<TInput, TOutput>(text, srcType, destType);

            return (Action<TInput, TOutput>)
                Delegate.CreateDelegate(typeof(Action<TInput, TOutput>), type, "Map");
        }
    }
}
