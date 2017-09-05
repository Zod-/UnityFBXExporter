using NUnit.Framework;

namespace UnityFBXExporter
{
    public class FbxGlobalSettingsNodeTests
    {
        [Test]
        public void TestSerializeMeshModelNode()
        {
            var sut = new FbxGlobalSettingsNode();

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(sut, 0));
            Assert.That(actual, Is.EqualTo(SampleData.GetTestData("TopStructure\\GlobalSettingsTest.fbx")));
        }
    }

}