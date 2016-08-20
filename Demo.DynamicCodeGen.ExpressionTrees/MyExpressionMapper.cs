using System;
using System.Linq.Expressions;
using Demo.DynamicCodeGen.Common;

namespace Demo.DynamicCodeGen.ExpressionTrees
{
    /// <summary>
    /// Compiles an expression using Expression.Compile() method
    /// </summary>
    public class MyExpressionMapperV1 : ITestableMapper
    {
        public static MyExpressionMapperV1 Instance => new MyExpressionMapperV1();

        public Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>()
        {
            Expression<Action<TInput, TOutput>> expression = ExpressionBuilder.CreateExpression<TInput, TOutput>();

            return expression.Compile();
        }
    }

    /// <summary>
    /// Emittes an expression using Expression.CompileToMethod(MethodBuilder) in dynamic assembly (explicitly).
    /// </summary>
    public class MyExpressionMapperV2 : ITestableMapper
    {
        public static MyExpressionMapperV2 Instance => new MyExpressionMapperV2();

        public Action<TInput, TOutput> CreateMapMethod<TInput, TOutput>()
        {
            Expression<Action<TInput, TOutput>> expression = ExpressionBuilder.CreateExpression<TInput, TOutput>();

            //emitting map method in dynamic assembly
            var mapperType = MapperTypeBuilder.CreateMapperType(expression);

            return (Action<TInput, TOutput>) Delegate.CreateDelegate(expression.Type, mapperType.GetMethod("Map"));
        }
    }
}