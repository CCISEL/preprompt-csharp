using System;
using System.Diagnostics;

namespace Demos.Dynamic
{
    public class OverloadResolution
    {
        public static int Add(int x, int y)
        {
            return x + y;
        }

        public static TimeSpan Add(TimeSpan x, TimeSpan y)
        {
            return x.Add(y);
        }

        public static object AddDynamic(dynamic x, dynamic y)
        {
            return Add(x, y);
        }

        public static object Add(object x, object y)
        {
            if (x is int && y is int)
            {
                return Add((int)x, (int)y);
            }
            if (x is TimeSpan && y is TimeSpan)
            {
                return Add((TimeSpan)x, (TimeSpan)y);
            }
            throw new InvalidOperationException();
        }

        [Ignore]
        public static void Run()
        {
            object x = 3;
            object y = 4;

            Console.WriteLine(Add(x, y));
            Console.WriteLine(AddDynamic(x, y));

            Console.WriteLine();

            Console.WriteLine("Add time: {0}", measure(() => Add(x, y)));
            Console.WriteLine("AddDynamic time: {0}", measure(() => AddDynamic(x, y)));
        }

        private static long measure(Action action)
        {
            const int repeats = 10000000;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            repeats.Times(action);
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }
    }
}