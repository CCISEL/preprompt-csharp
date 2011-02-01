using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Demos.Expressions.Expandable
{
    public static class ExpressionExtensions
    {
        public static TResult Invoke<T, TResult>(this Expression<Func<T, TResult>> expr, T element)
        {
            return expr.Compile().Invoke(element);
        }

        public static Expression<TFunc> Expand<TFunc>(this Expression<TFunc> expr)
        {
            return (Expression<TFunc>)new ExpressionExpander().Visit(expr);
        }

        public static Expression Expand(this Expression expr)
        {
            return new ExpressionExpander().Visit(expr);
        }
    }

    public class ExpressionExpander : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, Expression> _replaceVars;

        public ExpressionExpander()
        { }

        private ExpressionExpander(Dictionary<ParameterExpression, Expression> replaceVars)
        {
            _replaceVars = replaceVars;
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_replaceVars != null && _replaceVars.ContainsKey(p))
                return _replaceVars[p];
            return base.VisitParameter(p);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType != typeof(ExpressionExtensions))
            {
                return base.VisitMethodCall(m);
            }

            var closureFieldAccess = (MemberExpression)m.Arguments[0];
            var closureObject = (ConstantExpression)closureFieldAccess.Expression;
            var lambda = (LambdaExpression)((FieldInfo)closureFieldAccess.Member).GetValue(closureObject.Value);

            var replaceVars = new Dictionary<ParameterExpression, Expression>();

            for (int i = 0; i < lambda.Parameters.Count; i++)
            {
                Expression rep = m.Arguments[i + 1];
                if (_replaceVars != null && rep is ParameterExpression && _replaceVars.ContainsKey((ParameterExpression)rep))
                {
                    replaceVars.Add(lambda.Parameters[i], _replaceVars[(ParameterExpression)rep]);
                }
                else
                {
                    replaceVars.Add(lambda.Parameters[i], rep);
                }
            }

            if (_replaceVars != null)
            {
                foreach (KeyValuePair<ParameterExpression, Expression> pair in _replaceVars)
                {
                    replaceVars.Add(pair.Key, pair.Value);
                }
            }
            return new ExpressionExpander(replaceVars).Visit(lambda.Body);
        }
    }
}