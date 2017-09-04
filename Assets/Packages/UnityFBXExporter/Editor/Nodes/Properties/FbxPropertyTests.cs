using NUnit.Framework;

namespace UnityFBXExporter
{
    public class FbxPropertyTests
    {
        [Test]
        public void TestPropertyToStringFormat()
        {
            Assert.That(new FbxProperty("Name", "Type", "Label", "Flags", 123).ToString(), Is.EqualTo("P: \"Name\", \"Type\", \"Label\", \"Flags\", 123"));
        }
    }
}
