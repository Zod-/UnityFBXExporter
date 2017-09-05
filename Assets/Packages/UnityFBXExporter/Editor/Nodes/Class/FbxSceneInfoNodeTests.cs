using NUnit.Framework;

namespace UnityFBXExporter
{
    public class FbxSceneInfoNodeTests
    {
        [Test]
        public void TestSerializeMeshModelNode()
        {
            var sut = new FbxSceneInfoNode("TestPath");

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(sut, 0));
            Assert.That(actual, Is.EqualTo(SampleData.GetTestData("Class\\SceneInfoTest.fbx")));
        }
    }
}
