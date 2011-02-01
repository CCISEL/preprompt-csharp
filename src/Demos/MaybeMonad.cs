using System;

namespace Demos
{
    public static class MaybeMonad
    {
        public static TBind SelectMany<TSource, TBind>(this TSource source, Func<TSource, TBind> bind)
            where TSource : class
            where TBind : class
        {
            return source == null ? null : bind(source);
        }

        public static TResult SelectMany<TSource, TBind, TResult>(this TSource source, 
                                                                  Func<TSource, TBind> bind,
                                                                  Func<TSource, TBind, TResult> resultSelector)
            where TSource : class
            where TResult : class
        {
            return source == null ? null : resultSelector(source, bind(source));
        }

        [Ignore]
        public static void Run()
        {
            string s = "a string";
            // string s = null;

            var x = from _ in s
                    from u in s.ToUpper()
                    select u;
            Console.WriteLine(x);
        }
    }
}
