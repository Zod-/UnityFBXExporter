using System;
using NUnit.Framework;

namespace UnityFBXExporter
{
    public class FbxHeaderExtensionNodeTests
    {
        [Test]
        public void TestSerializeMeshModelNode()
        {
            var sut = new FbxHeaderExtensionNode("TestPath", new DateTime());

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(sut, 0));
            Assert.That(actual, Is.EqualTo(SampleData.GetTestData("TopStructure\\HeaderExtensionTest.fbx")));
        }
    }
}