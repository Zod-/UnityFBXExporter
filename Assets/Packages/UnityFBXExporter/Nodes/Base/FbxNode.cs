using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityFBXExporter
{
    public abstract class FbxNode
    {
        public string Name { get; private set; }

        public readonly List<FbxNode> ChildNodes = new List<FbxNode>(10);
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
            ChildNodes.Add(new FbxValueNode(name, value));
        }

        protected void Property(string name, string type, string label, string flags, object value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, value));
        }

        protected void Property(string name, string type, string label, string flags, string value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, string.Format("\"{0}\"", value)));
        }

        protected void Property(string name, string type, string label, string flags, Color value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, string.Format("{0},{1},{2}", value.r, value.g, value.b)));
        }

        protected void Property(string name, string type, string label, string flags, Vector3 value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, string.Format("{0},{1},{2}", value.x, value.y, value.z)));
        }

        protected void ArrayNode(string name, ICollection value)
        {
            ChildNodes.Add(new FbxArrayNode(name, value));
        }

        protected static long InstanceId(Object obj)
        {
            return Mathf.Abs(obj.GetInstanceID());
        }
    }
}
