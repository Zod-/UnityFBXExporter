using System.Collections;

namespace UnityFBXExporter
{
    public class FbxArrayNode : FbxNode
    {
        public int Length { get; private set; }

        public FbxArrayNode(string name, ICollection value) : base(name)
        {
            Length = value.Count;
            Node("a", value);
        }
    }
}
