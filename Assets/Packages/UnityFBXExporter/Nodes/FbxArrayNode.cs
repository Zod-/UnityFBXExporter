using System.Collections;

namespace UnityFBXExporter
{
    public class FbxArrayNode : FbxNode
    {
        public int Length { get; protected set; }

        public FbxArrayNode(string name, ICollection value)
        {
            Name = name;
            Length = value.Count;
            Node("a", value);
        }
    }
}
