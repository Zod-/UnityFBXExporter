using System.Collections;

namespace UnityFBXExporter
{
    public class FbxArrayNode : FbxNode
    {
        private readonly int _length;

        public FbxArrayNode(string name, ICollection value) : base(name)
        {
            _length = value.Count;
            Node("a", value);
        }

        public override string GetMetaName()
        {
            return string.Format("*{0}", _length);
        }
    }
}
