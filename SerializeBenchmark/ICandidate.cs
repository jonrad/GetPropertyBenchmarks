using System;
using System.Reflection;

namespace SerializeBenchmark
{
    public interface ICandidate
    {
        int Divisor { get; }

        Func<object, object> CreateDelegate(object obj, PropertyInfo propertyInfo);
    }
}
