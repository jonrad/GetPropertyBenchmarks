using System;
using System.Reflection;

namespace SerializeBenchmark
{
    // always need a control
    public class ControlCandidate : ICandidate
    {
        public int Divisor => 1;

        public Func<object, object> CreateDelegate(object obj, PropertyInfo propertyInfo)
        {
            // this will obviously fail the Age test
            return o => ((Program.Person)o).Name;
        }
    }
}
