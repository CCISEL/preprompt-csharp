using System;
using System.Collections.Generic;

namespace Demos
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void Times(this int times, Action action)
        {
            for (int i = 0; i < times; ++i)
            {
                action();
            }
        }
    }
}
