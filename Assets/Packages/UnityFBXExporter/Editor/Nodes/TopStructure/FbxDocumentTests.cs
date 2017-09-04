using System;
using System.IO;
using System.Linq;
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

            var document = new FbxDocument(new[] { gameObject }, "TestPath", new DateTime());
            var actual = string.Join("\r\n", FbxAsciiWriter.SerializeDocument(document).ToArray());
            var testDocument = File.ReadAllText("Assets\\Packages\\UnityFBXExporter\\Editor\\Nodes\\TopStructure\\TestDocument.fbx");
            Assert.That(actual, Is.EqualTo(string.Format(testDocument, FbxNode.InstanceId(gameObject), FbxNode.InstanceId(cube))));
        }
    }
}
