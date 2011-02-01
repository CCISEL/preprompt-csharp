using System;
using System.Collections.Generic;

namespace Demos.Dynamic
{
    public class RuntimeTypeInference
    {
        private static bool AddConditionallyImpl<T>(IList<T> list, T item)
        {
            if (list.Count < 10)
            {
                list.Add(item);
                return true;
            }
            return false;
        }
        
        public static bool AddConditionally(dynamic list, dynamic item)
        {
            return AddConditionallyImpl(list, item);
        }

        [Ignore]
        public static void Run()
        {
            object list = new List<string> { "x", "y" };
            object item = "z";
            AddConditionally(list, item);
            ((List<string>)list).ForEach(Console.WriteLine);
        }
    }
}
