using System;
using IronRuby;

namespace Demos.Dynamic
{
    public class IronRuby
    {
        [Ignore]
        public static void Run()
        {
            dynamic pp = Ruby.CreateEngine().CreateScriptSourceFromFile("Dynamic/preprompt.rb").Execute();
            foreach (var session in pp.sessions)
            {
                Console.WriteLine(session.title);
                Console.WriteLine("Speakers:");
                foreach (var speaker in session.speakers)
                {
                    Console.WriteLine("\t" + speaker);
                }
            }

            Console.WriteLine();

            Extensions.ForEach(pp.sessions, new Action<dynamic>(session => Console.WriteLine(session.to_s())));
        }
    }
}
