using System.Collections.Generic;

namespace UnityFBXExporter
{
    public abstract class FBXNodeList
    {
        public string Name { get; set; }

        public List<FBXNode> Nodes = new List<FBXNode>();
    }
}