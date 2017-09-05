using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityFBXExporter
{
    public class FbxDocumentTests
    {
        [Test]
        public void TestFullDocument()
        {
            var gameObject = new GameObject();
            var cube = Object.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), gameObject.transform);
            var sut = new FbxDocument(new[] { gameObject }, "TestPath", new DateTime());

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeDocument(sut));
            var testDocument = SampleData.GetTestData("TopStructure\\TestDocument.fbx");
            Assert.That(actual, Is.EqualTo(string.Format(testDocument, FbxNode.InstanceId(gameObject), FbxNode.InstanceId(cube))));
        }
    }
}
