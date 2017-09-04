using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxModelNodeTests
    {
        private const string CubeModel = "Model: {0}, \"Model::Cube\", \"Mesh\" {{\tVersion: 232\tShading: T\tCulling: \"CullingOff\"\tProperties70:  {{\t\tP: \"RotationOrder\", \"enum\", \"\", \"\", 4\t\tP: \"RotationActive\", \"bool\", \"\", \"\", 1\t\tP: \"InheritType\", \"enum\", \"\", \"\", 1\t\tP: \"ScalingMax\", \"Vector3D\", \"Vector\", \"\", 0,0,0\t\tP: \"DefaultAttributeIndex\", \"int\", \"Integer\", \"\", 0\t\tP: \"currentUVSet\", \"KString\", \"\", \"U\", \"map1\"\t\tP: \"Lcl Translation\", \"Lcl Translation\", \"\", \"A+\", 0,0,0\t\tP: \"Lcl Rotation\", \"Lcl Rotation\", \"\", \"A+\", 0,0,0\t\tP: \"Lcl Scaling\", \"Lcl Scaling\", \"\", \"A\", 1,1,1\t}}}}";
        private const string EmptyModel = "Model: {0}, \"Model::New Game Object\", \"Null\" {{\tVersion: 232\tShading: T\tCulling: \"CullingOff\"\tProperties70:  {{\t\tP: \"RotationOrder\", \"enum\", \"\", \"\", 4\t\tP: \"RotationActive\", \"bool\", \"\", \"\", 1\t\tP: \"InheritType\", \"enum\", \"\", \"\", 1\t\tP: \"ScalingMax\", \"Vector3D\", \"Vector\", \"\", 0,0,0\t\tP: \"DefaultAttributeIndex\", \"int\", \"Integer\", \"\", 0\t\tP: \"currentUVSet\", \"KString\", \"\", \"U\", \"map1\"\t\tP: \"Lcl Translation\", \"Lcl Translation\", \"\", \"A+\", 0,0,0\t\tP: \"Lcl Rotation\", \"Lcl Rotation\", \"\", \"A+\", 0,0,0\t\tP: \"Lcl Scaling\", \"Lcl Scaling\", \"\", \"A\", 1,1,1\t}}}}";

        [Test]
        public void TestSerializeMeshModelNode()
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var fbxModelNode = new FbxModelNode(cube);
            var actual = string.Join("", FbxAsciiWriter.SerializeGenericNode(fbxModelNode, 0).ToArray());
            Assert.That(actual, Is.EqualTo(string.Format(CubeModel, FbxNode.InstanceId(cube))));
        }

        [Test]
        public void TestSerializeEmptyModelNode()
        {
            var gameObject = new GameObject();
            var fbxModelNode = new FbxModelNode(gameObject);
            var actual = string.Join("", FbxAsciiWriter.SerializeGenericNode(fbxModelNode, 0).ToArray());
            Assert.That(actual, Is.EqualTo(string.Format(EmptyModel, FbxNode.InstanceId(gameObject))));
        }
    }
}
