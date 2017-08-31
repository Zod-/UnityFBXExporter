using System.Collections.Generic;

namespace UnityFBXExporter
{
    public abstract class FbxNodeList
    {
        public string Name { get; set; }

        public List<FbxNode> Nodes = new List<FbxNode>();
    }
}