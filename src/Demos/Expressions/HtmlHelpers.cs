using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Demos.Expressions
{
    public static class HtmlHelpers
    {
        public static PropertyInfo GetProperty<TModel, TResult>(Expression<Func<TModel, TResult>> expression)
        {
            var memberExpression = get_member_expression(expression);
            return (PropertyInfo)memberExpression.Member;
        }

        private static MemberExpression get_member_expression<TModel, TResult>(Expression<Func<TModel, TResult>> expression)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", "expression");
            }
            
            return memberExpression;
        }

        public static XElement LabelFor<TModel>(Expression<Func<TModel, object>> expression)
        {
            var propName = GetProperty(expression).Name;
            return new XElement("label", new XAttribute("for", propName), propName);
        }

        public class AModel
        {
            public string SomeProperty { get; set; }
        }

        [Ignore]
        public static void Run()
        {
            Console.WriteLine(LabelFor<AModel>(x => x.SomeProperty));
        }
    }
}
