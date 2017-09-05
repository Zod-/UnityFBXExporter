using NUnit.Framework;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxMaterialNodeTests : MonoBehaviour
    {
        [Test]
        public void TestSerializeMeshModelNode()
        {
            var mat = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Renderer>().sharedMaterial;
            var sut = new FbxMaterialNode(mat);

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(sut, 0));
            Assert.That(actual, Is.EqualTo(string.Format(SampleData.GetTestData("Class\\CubeMaterialTest.fbx"), FbxNode.InstanceId(mat))));
        }
    }
}
