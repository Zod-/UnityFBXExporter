using NUnit.Framework;

namespace UnityFBXExporter
{
    public class FbxValueNodeTests
    {
        [Test]
        public void TestValueNodeToStringFormat()
        {
            Assert.That(new FbxValueNode("Test", 123).ToString(), Is.EqualTo("Test: 123"));
        }
    }
}
