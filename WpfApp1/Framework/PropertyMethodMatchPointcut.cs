using System;
using System.Reflection;
using Spring.Aop.Support;

namespace Framework
{
    public class PropertyMethodMatchPointcut : StaticMethodMatcherPointcut
    {
        public override bool Matches(MethodInfo method, Type targetType)
        {
            return method.Name.StartsWith("set_") && method.IsPublic;
        }
    }
}
