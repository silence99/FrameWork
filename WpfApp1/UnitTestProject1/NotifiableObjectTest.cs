using Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class NotifiableObjectTest
    {
        [TestMethod]
        public void TestCreateAop()
        {
        }
    }

    public interface ITest
    {
        string Name { get; set; }
    }

    public class TestCls : NotifiableObject, ITest
    {
        public virtual string Name { get; set; }
    }
}
