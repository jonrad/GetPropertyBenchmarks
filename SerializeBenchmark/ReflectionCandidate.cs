using System;
using System.Reflection;

namespace SerializeBenchmark
{
    public class ReflectionCandidate : ICandidate
    {
        public int Divisor => 10;

        public Func<object, object> CreateDelegate(object obj, PropertyInfo propertyInfo)
        {
            return o => propertyInfo.GetValue(o, new object[0]);
        }
    }
}
