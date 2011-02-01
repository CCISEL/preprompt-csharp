using System;
using Demos;

namespace csharp4.Dynamic
{
    public class DynamicExpressions
    {
        public DynamicExpressions(dynamic x)
        { }

        [Ignore]
        public static void Run()
        {
            dynamic x = 0;
            var obj = new DynamicExpressions(x); // var is of type DynamicExpressions

            int[] ints = { 1, 2 };
            var i = ints[x]; // i is of type int
            Console.WriteLine(i);
        }
    }
}
