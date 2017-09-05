using NUnit.Framework;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxObjectsNodeTests
    {
        [Test]
        public void TestSerializeMeshModelNode()
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var empty = new GameObject();
            var sut = new FbxObjectsNode(new[] { cube, empty }, new FbxConnectionsNode());

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(sut, 0));
            Assert.That(actual, Is.EqualTo(string.Format(SampleData.GetTestData("TopStructure\\CubeObjectTest.fbx"), FbxNode.InstanceId(cube), FbxNode.InstanceId(empty))));
        }
    }

}