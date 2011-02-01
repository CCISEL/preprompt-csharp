using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Demos.Dynamic
{
    public class SimpleCallSite
    {
        public class SomeClass
        {
            public int Power(int x)
            {
                return x * x;
            }
        }

        [Ignore]
        public static void Run()
        {
            var obj = new SomeClass();

            var callSite = CallSite<Func<CallSite, SomeClass, int, object>>.Create(get_binder());
            int result = (int)callSite.Target(callSite, obj, 3);
            Console.WriteLine(result);

            //
            // Using an expression trees (like the DLR compilers do):
            //

            result = (int)Expression.Lambda<Func<object>>(
                            Expression.Dynamic(get_binder(), typeof(object), 
                                Expression.Constant(obj),
                                Expression.Constant(4))
                          ).Compile().Invoke();
            Console.WriteLine(result);
        }

        private static CallSiteBinder get_binder()
        {
            return Binder.InvokeMember(CSharpBinderFlags.None, "Power", null, MethodBase.GetCurrentMethod().DeclaringType, new[]
            {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null)
            });
        }
    }
}
