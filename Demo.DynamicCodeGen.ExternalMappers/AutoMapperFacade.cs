using System;
using AutoMapper;
using Demo.DynamicCodeGen.Common;

namespace Demo.DynamicCodeGen.ExternalMappers
{
    public class AutoMapperFacade : ITestableMapper
    {
        public static ITestableMapper Instance => new AutoMapperFacade();

        public Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TInput, TOutput>();
            });

            Action<TInput, TOutput> action = (src, dest) => Mapper.Map(src, dest);

            return action;
        }
    }
}
