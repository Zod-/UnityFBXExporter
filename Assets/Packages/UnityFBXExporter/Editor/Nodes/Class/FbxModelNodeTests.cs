using NUnit.Framework;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxModelNodeTests
    {
        [Test]
        public void TestSerializeMeshModelNode()
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var sut = new FbxModelNode(cube);

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(sut, 0));
            Assert.That(actual, Is.EqualTo(string.Format(SampleData.GetTestData("Class\\CubeModelTest.fbx"), FbxNode.InstanceId(cube))));
        }

        [Test]
        public void TestSerializeEmptyModelNode()
        {
            var gameObject = new GameObject();
            var sut = new FbxModelNode(gameObject);
            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(sut, 0));
            Assert.That(actual, Is.EqualTo(string.Format(SampleData.GetTestData("Class\\EmptyModelTest.fbx"), FbxNode.InstanceId(gameObject))));
        }
    }
}
