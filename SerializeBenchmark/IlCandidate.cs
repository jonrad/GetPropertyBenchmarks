using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SerializeBenchmark
{
    public class IlCandidate : ICandidate
    {
        public int Divisor => 1;

        public Func<object, object> CreateDelegate(object obj, PropertyInfo propertyInfo)
        {
            var objType = obj.GetType();
            var methodBuilder = new DynamicMethod(
                "il",
                typeof(object),
                new[] { typeof(object) });

            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, objType);
            il.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
            if (propertyInfo.PropertyType.IsValueType)
            {
                il.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }

            il.Emit(OpCodes.Ret);

            return (Func<object, object>)methodBuilder.CreateDelegate(typeof(Func<object, object>));
        }
    }

    public class ControlCandidate : ICandidate
    {
        public int Divisor => 1;

        public Func<object, object> CreateDelegate(object obj, PropertyInfo propertyInfo)
        {
            return o => ((Program.Person)o).Name;
        }
    }
}
