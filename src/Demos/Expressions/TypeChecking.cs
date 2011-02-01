using System.Linq.Expressions;

namespace Demos.Expressions
{
    public class TypeChecking
    {
        [Ignore]
        public void Run()
        {
            Expression.Add(
                Expression.Constant(42), 
                Expression.Constant("42"));
        }
    }
}
