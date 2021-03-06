﻿using System;
using NUnit.Framework;

namespace UnityFBXExporter
{
    public class FbxCreationTimeStampNodeTests
    {
        [Test]
        public void TestSerializeMeshModelNode()
        {
            var sut = new FbxCreationTimeStampNode(new DateTime());

            var actual = SampleData.ToLines(FbxAsciiWriter.SerializeGenericNode(sut, 0));
            Assert.That(actual, Is.EqualTo(SampleData.GetTestData("Header\\CreationTimeStampNodeTest.fbx")));
        }
    }

}