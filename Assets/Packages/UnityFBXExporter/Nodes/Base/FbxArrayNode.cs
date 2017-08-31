using System;

namespace UnityFBXExporter
{
    public class FbxArrayNode : FbxNode
    {
        private readonly int _length;

        public FbxArrayNode(string name, Array value) : base(name)
        {
            _length = value.Length;
            Node("a", value);
        }

        public override string GetMetaName()
        {
            return string.Format("*{0}", _length);
        }
    }
}
