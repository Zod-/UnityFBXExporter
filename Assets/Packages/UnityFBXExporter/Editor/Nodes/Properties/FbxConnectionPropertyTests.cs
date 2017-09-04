using NUnit.Framework;

namespace UnityFBXExporter
{
    public class FbxConnectionPropertyTests
    {
        [Test]
        public void TestConnectionPropertyToStringFormat()
        {
            Assert.That(new FbxConnectionProperty(1, 2, "OP").ToString(), Is.EqualTo("C: \"OP\",2,1"));
        }
        [Test]
        public void TestConnectionPropertyToStringFormatDefault()
        {
            Assert.That(new FbxConnectionProperty(2, 1).ToString(), Is.EqualTo("C: \"OO\",1,2"));
        }
    }
}
