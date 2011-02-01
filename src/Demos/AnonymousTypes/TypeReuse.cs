using System;

namespace Demos.AnonymousTypes
{
    public class TypeReuse
    {
        private static object GetAnonymous()
        {
            return new { Session = "C#", Date = DateTime.Parse("26/01/2011") };
        }

        [Ignore]
        public static void Run()
        {
            object instance = GetAnonymous();
            var typed = Cast(instance, new { Session = "", Date = default(DateTime) });
            Console.WriteLine("Session = {0}, Date = {1}", typed.Session, typed.Date);
        }

        public static T Cast<T>(object instance, T type)
        {
            return (T)instance;
        }
    }
}
