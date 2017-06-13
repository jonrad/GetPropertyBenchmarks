using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SerializeBenchmark
{
    public class ExpressionTreeCandidate : ICandidate
    {
        public int Divisor => 1;

        public Func<object, object> CreateDelegate(object obj, PropertyInfo propertyInfo)
        {
            var p = Expression.Parameter(typeof(object));

            Expression mainBody = Expression.Property(
                Expression.Convert(p, obj.GetType()),
                propertyInfo);

            var withConversion = propertyInfo.PropertyType.IsValueType
                ? Expression.Convert(mainBody, typeof(object))
                : mainBody;

            var expression = Expression.Lambda<Func<object, object>>(withConversion, p);

            return expression.Compile();
        }
    }
}
