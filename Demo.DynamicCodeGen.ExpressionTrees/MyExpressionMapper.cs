using System;
using System.Linq.Expressions;

namespace Demo.DynamicCodeGen.ExpressionTrees
{
    public static class MyExpressionMapper
    {
        public static Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>()
            where TInput : class, new()
            where TOutput : class, new()
        {
            Expression<Action<TInput, TOutput>> expression = ExpressionBuilder.CreateExpression<TInput, TOutput>();

            var mapperType = MapperTypeBuilder.CreateMapperType(expression);

            return (Action<TInput, TOutput>) Delegate.CreateDelegate(expression.Type, mapperType.GetMethod("Map"));
        }
    }
}