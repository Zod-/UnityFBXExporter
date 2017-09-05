using NUnit.Framework;

namespace UnityFBXExporter
{
    public class FbxMetaDataNodeTests
    {
        [Test]
        public void TestSerializeMeshModelNode()
        {
            var sut = new FbxMetaDataNode();

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(sut, 0));
            Assert.That(actual, Is.EqualTo(SampleData.GetTestData("Header\\MetaDataTest.fbx")));
        }
    }

}