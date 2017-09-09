using NUnit.Framework;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxValueSerializerTests
    {
        [Test]
        public void TestSerializeVector3()
        {
            Assert.That(FbxValueSerializer.Serialize(Vector3.one), Is.EqualTo("1,1,1"));
        }

        [Test]
        public void TestSerializeColor()
        {
            Assert.That(FbxValueSerializer.Serialize(Color.white), Is.EqualTo("1,1,1"));
        }

        [Test]
        public void TestSerializeIntCollection()
        {
            Assert.That(FbxValueSerializer.Serialize(new[] { 1, 2, 3, 4, 5 }), Is.EqualTo("1,2,3,4,5"));
        }

        [Test]
        public void TestSerializeFloatCollection()
        {
            Assert.That(FbxValueSerializer.Serialize(new[] { 1.234f, 2f, 3.5f, 4.5f, 5.1f }), Is.EqualTo("1.234,2,3.5,4.5,5.1"));
        }

        [Test]
        public void TestSerializeIntCollectionArrayCheck()
        {
            Assert.That(FbxValueSerializer.Serialize(new[] { 1, 2 }), Is.EqualTo("1,2"));
        }

        [Test]
        public void TestSerializeFloatCollectionArrayCheck()
        {
            Assert.That(FbxValueSerializer.Serialize(new[] { 1.1f, 2.2f }), Is.EqualTo("1.1,2.2"));
        }

        [TestCase("String", "\"String\"")]
        [TestCase(123, "123")]
        [TestCase(17179869176L, "17179869176")]
        [TestCase('T', "T")]
        public void TestSerializePrimitiveValuesArrayCheck(object actual, string expected)
        {
            Assert.That(FbxValueSerializer.Serialize(actual), Is.EqualTo(expected));
        }
    }
}
