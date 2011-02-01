using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Demos.Expressions.FileQuery
{
    public class FileQuery
    {
        private readonly IList<FileQuerySpecification> _specifications = new List<FileQuerySpecification>();
        private readonly IList<Expression> _expressions = new List<Expression>();
        private readonly FileQueryMetadata _metadata;

        public FileQuery()
        {
            _metadata = new FileQueryMetadata();
        }

        public FileQuery Where(Func<FileQueryMetadata, FileQuerySpecification> filter)
        {
            _specifications.Add(filter(_metadata));
            return this;
        }
      
        /*
        public FileQuery Where(Expression<Func<FileMetadata, bool>> filter)
        {
            _expressions.Add(filter);
            return this;
        }
        */

        public IEnumerable<FileQuerySpecification> Specifications
        {
            get { return _specifications; }
        }

        public IEnumerable<Expression> Expressions
        {
            get { return _expressions; }
        }

        [Ignore]
        public static void Run()
        {
            var query = from file in new FileQuery()
                        where file.Length == 2 && file.Name == "A Name"
                        select file;

            foreach (var spec in query.Specifications)
            {
                Console.WriteLine(spec);
            }
        }
    }
}