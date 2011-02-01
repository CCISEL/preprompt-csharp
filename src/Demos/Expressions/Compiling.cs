using System;
using System.Linq;
using System.Linq.Expressions;

namespace Demos.Expressions
{
    public class Compiling
    {
        public static void GeneratedExpression()
        {
            Expression<Func<string, string, bool>> expression = (x, y) => x.StartsWith(y);
            Console.WriteLine(expression);

            var compiled = expression.Compile();
            Console.WriteLine(compiled("First", "Fir"));

            new ExpressionPrinter().Visit(expression); // Can be seen in the visualizer.
        }

        public static void CapturesVariables()
        {
            int i = 2;
            Expression<Func<int>> expression = () => i;
            Console.WriteLine(expression.Compile()());
            new ExpressionPrinter().Visit(expression);
        }

        [Ignore]
        public static void Run()
        {
            GeneratedExpression();
            //CapturesVariables();
        }
    }

    public class Identation
    {
        private int _identLevent;

        public IDisposable Ident()
        {
            _identLevent += 1;
            return new DisposableAction(() => _identLevent -= 1);
        }

        public void Print(string text, params object[] args)
        {
            Console.Write(Enumerable.Range(0, _identLevent).Aggregate("", (ident, _) => ident + "\t"));
            Console.WriteLine(text, args);
        }
    }

    public class ExpressionPrinter : ExpressionVisitor
    {
        private readonly Identation _ident = new Identation();

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            _ident.Print("Expression<{0}>", typeof(T).Name);
            print_expression_info(node);

            _ident.Print("Parameters:");
            using (_ident.Ident()) node.Parameters.ForEach(n => Visit(n));

            _ident.Print("Body:");
            using (_ident.Ident()) Visit(node.Body);

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            _ident.Print("MethodCallExpression");
            print_expression_info(node);

            _ident.Print("Method name:");
            _ident.Print("\t" + node.Method.Name);

            _ident.Print("Method arguments:");
            using (_ident.Ident()) node.Arguments.ForEach(arg => Visit(arg));

            _ident.Print("Target:");
            using (_ident.Ident()) Visit(node.Object);
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            _ident.Print("ParameterExpression");
            print_expression_info(node);
            _ident.Print("Name: {0}", node.Name);
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _ident.Print("MemberExpression:");
            print_expression_info(node);

            _ident.Print("Field:");
            _ident.Print("\t" + node.Member.Name);
            
            _ident.Print("Target:");
            using (_ident.Ident()) Visit(node.Expression);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _ident.Print("ConstantExpression:");
            print_expression_info(node);
            return node;
        }

        private void print_expression_info(Expression node)
        {
            _ident.Print("Node type: {0}", node.NodeType);
            _ident.Print("Type: {0}", node.Type);
            _ident.Print("Hash: {0}", node.GetHashCode());
        }
    }
}