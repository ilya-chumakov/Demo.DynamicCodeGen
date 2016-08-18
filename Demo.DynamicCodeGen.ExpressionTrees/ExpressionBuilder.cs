using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DynamicCodeGen.ExpressionTrees
{
    internal static class ExpressionBuilder
    {
        public static Expression<Action<TInput, TOutput>> CreateExpression<TInput, TOutput>()
        {
            var src = Expression.Parameter(typeof(TInput), "src");
            var dest = Expression.Parameter(typeof(TOutput), "dest");

            var srcProperties = src.Type.GetProperties();
            var destProperties = dest.Type.GetProperties();

            var assignments = new List<BinaryExpression>();

            foreach (var srcProperty in srcProperties)
            {
                var destProperty = destProperties.First(p => p.Name == srcProperty.Name);

                if (destProperty.PropertyType.IsAssignableFrom(srcProperty.PropertyType))
                {
                    var binaryExpression = Expression.Assign(
                        Expression.Property(dest, destProperty),
                        Expression.Property(src, srcProperty));

                    assignments.Add(binaryExpression);
                }
                // else custom map
            }

            var block = Expression.Block(assignments);

            Expression<Action<TInput, TOutput>> expression = Expression.Lambda<Action<TInput, TOutput>>(block, src, dest);
            return expression;
        }
    }
}