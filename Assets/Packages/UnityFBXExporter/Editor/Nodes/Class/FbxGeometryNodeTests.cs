using NUnit.Framework;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxGeometryNodeTests
    {
        [Test]
        public void TestSerializeMeshModelNode()
        {
            var mesh = GameObject.CreatePrimitive(PrimitiveType.Cube).GetMesh();
            var fbxModelNode = new FbxGeometryNode(mesh);

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(fbxModelNode, 0));
            Assert.That(actual, Is.EqualTo(string.Format(SampleData.GetTestData("Class\\CubeGeometryTest.fbx"), FbxNode.InstanceId(mesh))));
        }
    }
}
