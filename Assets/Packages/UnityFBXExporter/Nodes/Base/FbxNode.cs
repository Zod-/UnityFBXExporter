using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityFBXExporter
{
    public abstract class FbxNode
    {
        public string Name { get; private set; }

        public readonly List<FbxNode> Nodes = new List<FbxNode>(10);
        public readonly List<FbxProperty> Properties = new List<FbxProperty>(30);

        public virtual string GetMetaName()
        {
            return string.Empty;
        }

        protected FbxNode(string name)
        {
            Name = name;
        }

        //Helpers
        protected void Node(string name, object value)
        {
            Nodes.Add(new FbxValueNode(name, value));
        }

        protected void Property(string name, string type, string label, string flags, object value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, value));
        }

        protected void ArrayNode(string name, ICollection value)
        {
            Nodes.Add(new FbxArrayNode(name, value));
        }

        protected static long InstanceId(Object obj)
        {
            return Mathf.Abs(obj.GetInstanceID());
        }
    }
}
