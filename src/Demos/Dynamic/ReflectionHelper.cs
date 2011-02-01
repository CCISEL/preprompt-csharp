using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Demos.Dynamic
{
    public class ReflectionHelper : DynamicObject
    {
        private readonly Type _type;

        private ReflectionHelper(Type type)
        {
            _type = type;
        }

        //
        // TODO: Process generic methods.
        //

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var method = _type
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == binder.Name);

            return (result = method) != null;
        }

        public static dynamic From<T>()
        {
            return new ReflectionHelper(typeof(T));
        }

        [Ignore]
        public static void Run()
        {
            var member = From<List<int>>().EnsureCapacity;
            Console.WriteLine(((MethodInfo)member).IsPublic);
        }
    }
}