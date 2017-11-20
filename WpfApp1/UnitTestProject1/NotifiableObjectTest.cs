using Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class NotifiableObjectTest
    {
        [TestMethod]
        public void TestCreateAop()
        {
        }
        [TestMethod]
        public void TestStackContains()
        {
            Stack<KeyValuePair<NotifiableObject, string>> stack = new Stack<KeyValuePair<NotifiableObject, string>>();
            var obj = new NotifiableObject();
            stack.Push(new KeyValuePair<NotifiableObject, string>(obj, "Name"));
            Assert.IsTrue(stack.Contains(new KeyValuePair<NotifiableObject, string>(obj, "Name")));
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
