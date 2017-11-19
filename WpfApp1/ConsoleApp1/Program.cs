using AopAlliance.Intercept;
using Spring.Aop.Framework;
using Spring.Aop.Support;
using Spring.Aop;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> a = new List<string>()
            {
                "abc",
                "def"
            };
            var s = a.Where(item => item == "a");
        }
    }

    public interface ICommand
    {
        object Execute(object context);
        //string Name { get; set; }
    }

    public class ServiceCommand : ICommand
    {
        public virtual string Name { get; set; }
        public virtual object Execute(object context)
        {
            Console.Out.WriteLine("Service implementation : [{0}]", context);
            return null;
        }
    }

    public class ConsoleLoggingAroundAdvice : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            Console.Out.WriteLine("Advice executing; calling the advised method...");
            object returnValue = invocation.Proceed();
            Console.Out.WriteLine("Advice executed; advised method returned " + returnValue);
            return returnValue;
        }
    }

    public class PropertyMethodMatchPointcut : StaticMethodMatcherPointcut
    {
        public override bool Matches(MethodInfo method, Type targetType)
        {
            return method.Name.StartsWith("set_") && method.IsPublic;
        }
    }
}
