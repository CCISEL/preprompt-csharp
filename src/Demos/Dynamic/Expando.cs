using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Demos.Dynamic
{
    public class Expando
    {
        [Ignore]
        public static void Run()
        {
            dynamic expando = new ExpandoObject();
            Console.WriteLine(expando is IDictionary<string, object>);

            expando.SomeProp.X = "SomeValue";
            Console.WriteLine(expando.SomeProp);

            expando.SomeProp = 2;
            int i = expando.SomeProp;
            Console.WriteLine(i);
        }
    }
}