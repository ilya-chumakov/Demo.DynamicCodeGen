using System;
using Demo.DynamicCodeGen.Common;
using FastMapper;

namespace Demo.DynamicCodeGen.ExternalMappers
{
    public class FastMapperFacade : ITestableMapper
    {
        public static ITestableMapper Instance => new FastMapperFacade();

        public Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>()
        {
            Action<TInput, TOutput> action = (src, dest) => TypeAdapter.Adapt(src, dest);

            return action;
        }
    }
}
