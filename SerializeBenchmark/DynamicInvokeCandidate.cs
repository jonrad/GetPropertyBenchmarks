using System;
using System.Reflection;

namespace SerializeBenchmark
{
    public class DynamicInvokeCandidate : ICandidate
    {
        // I don't have all day
        public int Divisor => 100;

        public Func<object, object> CreateDelegate(object obj, PropertyInfo propertyInfo)
        {
            var objType = obj.GetType();

            var func = Delegate.CreateDelegate(
                typeof(Func<,>).MakeGenericType(objType, propertyInfo.PropertyType),
                null,
                propertyInfo.GetGetMethod());

            return o => func.DynamicInvoke(o);
        }
    }
}
