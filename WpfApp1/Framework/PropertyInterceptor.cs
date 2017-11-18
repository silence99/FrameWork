using AopAlliance.Intercept;

namespace Framework
{
    public class PropertyInterceptor : IMethodInterceptor
    {
        private NotifiableObject Notify { get; set; }
        public PropertyInterceptor(NotifiableObject obj)
        {
            Notify = obj;
        }
        public object Invoke(IMethodInvocation invocation)
        {
            return Notify.Invoke(invocation);
        }
    }
}
