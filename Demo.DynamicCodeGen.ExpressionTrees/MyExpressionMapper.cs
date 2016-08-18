using System;
using System.Linq.Expressions;
using Demo.DynamicCodeGen.Common;

namespace Demo.DynamicCodeGen.ExpressionTrees
{
    public class MyExpressionMapper : ITestableMapper
    {
        public static MyExpressionMapper Instance => new MyExpressionMapper();

        public Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>()
        {
            Expression<Action<TInput, TOutput>> expression = ExpressionBuilder.CreateExpression<TInput, TOutput>();

            var mapperType = MapperTypeBuilder.CreateMapperType(expression);

            return (Action<TInput, TOutput>) Delegate.CreateDelegate(expression.Type, mapperType.GetMethod("Map"));
        }
    }
}