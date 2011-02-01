using System;
using System.Collections;
using System.Collections.Generic;

namespace Demos.Dynamic
{
    public class DuckTyping
    {
        static void PrintCount(IEnumerable collection)
        {
            dynamic d = collection;
            int count = d.Count;
            Console.WriteLine(count);
        }

        [Ignore]
        public static void Run()
        {
            PrintCount(new BitArray(10));
            PrintCount(new HashSet<int> { 3, 5 });
            PrintCount(new List<int> { 1, 2, 3 });
        }
    }
}
