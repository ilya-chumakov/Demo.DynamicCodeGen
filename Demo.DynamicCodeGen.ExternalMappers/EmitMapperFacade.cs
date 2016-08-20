using System;
using Demo.DynamicCodeGen.Common;
using EmitMapper;

namespace Demo.DynamicCodeGen.ExternalMappers
{
    public class EmitMapperFacade : ITestableMapper
    {
        public static ITestableMapper Instance => new EmitMapperFacade();

        public Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>()
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<TInput, TOutput>();

            Action<TInput, TOutput> action = (src, dest) => mapper.Map(src, dest);

            return action;
        }
    }
}
