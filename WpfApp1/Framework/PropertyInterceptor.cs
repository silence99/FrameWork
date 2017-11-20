using AopAlliance.Intercept;

namespace Framework
{
    public class PropertyInterceptor : IMethodInterceptor
    {
        private NotifiableObject Notifier { get; set; }
        public PropertyInterceptor(NotifiableObject obj)
        {
            Notifier = obj;
        }
        public object Invoke(IMethodInvocation invocation)
        {
            return Notifier.Invoke(invocation);
        }
    }
}
