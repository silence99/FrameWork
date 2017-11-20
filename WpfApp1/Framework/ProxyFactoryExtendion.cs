using Spring.Aop.Framework;
using Spring.Aop.Support;

namespace Framework
{
    public static class ProxyFactoryExtendion
    {
        public static T CreateAopProxy<T>(this T obj) where T : NotifiableObject
        {
            var factory = new ProxyFactory(obj) { ProxyTargetType = true };
            DefaultPointcutAdvisor advisor = new DefaultPointcutAdvisor(new PropertyMethodMatchPointcut(), new PropertyInterceptor(obj));
            factory.AddAdvisor(advisor);
            obj.AopWapper = factory.GetProxy();
            return (T)obj.AopWapper;
        }
    }
}
