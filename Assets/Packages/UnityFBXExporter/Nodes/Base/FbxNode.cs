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

        protected void Node(string name, string value)
        {
            Node(name, new StringValue(value));
        }

        protected void Property(string name, string type, string label, string flags, object value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, value));
        }

        protected void Property(string name, string type, string label, string flags, string value)
        {
            Property(name, type, label, flags, new StringValue(value));
        }

        protected void Property(string name, string type, string label, string flags, Color value)
        {
            Property(name, type, label, flags, new ColorValue(value));
        }

        protected void Property(string name, string type, string label, string flags, Vector3 value)
        {
            Property(name, type, label, flags, new Vector3Value(value));
        }

        protected void ArrayNode(string name, object value, int length)
        {
            ChildNodes.Add(new FbxArrayNode(name, value, length));
        }

        protected void ArrayNode(string name, FbxValue value, int length)
        {
            ChildNodes.Add(new FbxArrayNode(name, value, length));
        }

        public static long InstanceId(Object obj)
        {
            return Mathf.Abs(obj.GetInstanceID());
        }
    }
}
