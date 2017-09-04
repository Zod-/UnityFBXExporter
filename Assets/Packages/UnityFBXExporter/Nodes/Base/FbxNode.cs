using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityFBXExporter
{
    public abstract class FbxNode
    {
        public string Name { get; private set; }

        public virtual string Header { get { return string.Empty; } }

        public readonly List<FbxNode> ChildNodes = new List<FbxNode>(10);
        public readonly List<FbxNode> Properties = new List<FbxNode>(30);

        protected virtual string GetMetaName()
        {
            return string.Empty;
        }

        public string GetFormattedMetaName()
        {
            return string.Format("{0}: {1} {{", Name, GetMetaName());
        }

        protected FbxNode(string name)
        {
            Name = name;
        }

        //Helpers
        protected void Node(string name, object value)
        {
            ChildNodes.Add(new FbxValueNode(name, value));
        }

        protected void Property(string name, string type, string label, string flags, object value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, value));
        }

        protected void ArrayNode(string name, int[] value)
        {
            ChildNodes.Add(new FbxArrayNode(name, value));
        }

        protected void ArrayNode(string name, float[] value)
        {
            ChildNodes.Add(new FbxArrayNode(name, value));
        }

        protected static long InstanceId(Object obj)
        {
            return Mathf.Abs(obj.GetInstanceID());
        }
    }
}
