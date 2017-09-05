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
            var fbxModelNode = new FbxModelNode(cube);

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(fbxModelNode, 0));
            Assert.That(actual, Is.EqualTo(string.Format(SampleData.GetTestData("Class\\CubeModelTest.fbx"), FbxNode.InstanceId(cube))));
        }

        [Test]
        public void TestSerializeEmptyModelNode()
        {
            var gameObject = new GameObject();
            var fbxModelNode = new FbxModelNode(gameObject);
            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(fbxModelNode, 0));
            Assert.That(actual, Is.EqualTo(string.Format(SampleData.GetTestData("Class\\EmptyModelTest.fbx"), FbxNode.InstanceId(gameObject))));
        }
    }
}
