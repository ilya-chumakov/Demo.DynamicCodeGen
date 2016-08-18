using System;

namespace Demo.DynamicCodeGen.Common
{
    public interface ITestableMapper
    {
        Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>();
    }
}