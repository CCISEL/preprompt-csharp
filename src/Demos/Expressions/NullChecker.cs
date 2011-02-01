using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Demos.Expressions
{
    public static class NullChecker
    {
        public static void CheckNonNull<T>(this T argument) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException("argument");
            }

            Checker<T>.Check(argument);
        }

        private static void MethodWithArguments<T1, T2>(T1 arg1, T2 arg2)
        {
            new { arg1, arg2 }.CheckNonNull();
            Console.WriteLine(arg1);
            Console.WriteLine(arg2);
        }

        [Ignore]
        public static void Run()
        {
            MethodWithArguments("arg", "another arg");
            MethodWithArguments("arg", default(string));
        }

        private static class Checker<T>
            where T : class
        {
            public static readonly Action<T> Check;

            static Checker()
            {
                ParameterExpression param = Expression.Parameter(typeof(T), "container");
                Check = Expression.Lambda<Action<T>>(build_checker(typeof(T).GetProperties(), param, 0), param).Compile();
            }

            private static Expression build_checker(PropertyInfo[] properties, ParameterExpression param, int index)
            {
                if (index == properties.Length)
                {
                    return Expression.Empty();
                }

                var property = properties[index];

                if (property.PropertyType.IsValueType)
                {
                    throw new ArgumentException(string.Format("Property {0} is a value type", property));
                }

                return Expression.Block(
                            Expression.IfThen(
                                Expression.Equal(
                                    Expression.Property(param, property),
                                    Expression.Constant(null, property.PropertyType)),
                                Expression.Throw(
                                    Expression.New(
                                        typeof(ArgumentNullException).GetConstructor(new[] { typeof(string) }),
                                        Expression.Constant(property.Name)))),
                            build_checker(properties, param, ++index));
            }
        }
    }
}
