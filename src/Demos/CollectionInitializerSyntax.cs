using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Demos
{
    public class CollectionInitializerSyntax : IEnumerable
    {
        public readonly List<int> Nums = new List<int>();
        public readonly List<string> Words = new List<string>();
        public readonly List<IEnumerable<int>> Collections = new List<IEnumerable<int>>();

        public void Add(int i)
        {
            Nums.Add(i);
        }

        public void Add(string s)
        {
            Words.Add(s);
        }

        public void Add(IEnumerable<int> obj)
        {
            Collections.Add(obj);
        }

        public void Print()
        {
            Collections
                .SelectMany(x => x.Select(i => i.ToString()))
                .Union(Nums.Select(i => i.ToString()))
                .Union(Words)
                .ToList()
                .ForEach(Console.WriteLine);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
        }

        [Ignore]
        public void Run()
        {
            new CollectionInitializerSyntax { 1, "lla", 2, 3, 4, "lala", new[] { 4, 5, 6, 7 } }.Print();
        }
    }
}