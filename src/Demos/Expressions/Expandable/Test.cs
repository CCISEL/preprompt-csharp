using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Demos.Expressions.Expandable
{
    public class Test
    {
        public class Book
        {
            public String Title { get; set; }
            public String Author { get; set; }
        }

        public static IEnumerable<Book> GetResults(Expression<Func<Book, bool>> criteria)
        {
            IQueryable<Book> db = new[] {
                                            new Book { Title = "War and Peace", Author = "Leo Tolstoy"},
                                            new Book { Title = "Hunger", Author = "Knut Hamsun" },
                                            new Book { Title = "Embers", Author = "Sandor Marai" }
                                        }.AsQueryable();

            var query = from book in db.AsExpandable()
                        where criteria.Invoke(book)
                        select book;
                
            return query;
        }

        [Ignore]
        public static void Run()
        {
            foreach (var book in GetResults(b => b.Title == "Embers"))
            {
                Console.WriteLine(book.Author);
            }
        }
    }
}