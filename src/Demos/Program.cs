using System;
using System.Linq;
using System.Reflection;

namespace Demos
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class IgnoreAttribute : Attribute
    { }

    public class Program
    {
        public static void Main()
        {
            Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Select(t => t.GetMethod("Run", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance,
                                         null, Type.EmptyTypes, null))
                .Where(m => m != null && m.ReturnType == typeof(void) && m.DeclaringType.IsGenericType == false
                         && (m.IsStatic || m.DeclaringType.GetConstructor(Type.EmptyTypes) != null)
                         && m.GetCustomAttributes(false).All(a => a.GetType() != typeof(IgnoreAttribute)))
                .Aggregate(new Action(() => { }),
                           (a, m) => a + (() => m.Invoke(m.IsStatic ? null : Activator.CreateInstance(m.DeclaringType),
                                                         new object[0])))
                .Invoke();
        }
    }
}