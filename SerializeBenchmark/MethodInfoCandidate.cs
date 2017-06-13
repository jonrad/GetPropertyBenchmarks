using System;
using System.Reflection;

namespace SerializeBenchmark
{
    public class MethodInfoCandidate : ICandidate
    {
        public int Divisor => 10;

        public Func<object, object> CreateDelegate(object obj, PropertyInfo propertyInfo)
        {
            var method = propertyInfo.GetGetMethod();
            return o => method.Invoke(o, new object[0]);
        }
    }
}
