using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Demos.Expressions.Expandable
{
    public static class ExpandableQuery
    {
        public static IQueryable<T> AsExpandable<T>(this IQueryable<T> queryable)
        {
            return new ExpandableWrapper(queryable.Provider).CreateQuery<T>(queryable.Expression);
        }
    }

    public class ExpandableWrapper : IQueryProvider
    {
        private readonly IQueryProvider _previousProvider;

        public ExpandableWrapper(IQueryProvider previousProvider)
        {
            _previousProvider = previousProvider;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            Console.WriteLine("Before:");
            Console.WriteLine(expression);
            Console.WriteLine();

            expression = expression.Expand();

            Console.WriteLine("After:");
            Console.WriteLine(expression);

            return new ExpandableQuery<TElement>(this, _previousProvider.CreateQuery<TElement>(expression));
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return _previousProvider.CreateQuery(expression);
        }

        public object Execute(Expression expression)
        {
            return _previousProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)Execute(expression);
        }
    }

    public class ExpandableQuery<T> : IQueryable<T>
    {
        private readonly ExpandableWrapper _provider;
        private readonly IQueryable<T> _origin;

        public ExpandableQuery(ExpandableWrapper provider, IQueryable<T> origin)
        {
            _provider = provider;
            _origin = origin;
        }

        public Expression Expression
        {
            get { return _origin.Expression; }
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public IQueryProvider Provider
        {
            get { return _provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _origin.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_origin).GetEnumerator();
        }
    }
}