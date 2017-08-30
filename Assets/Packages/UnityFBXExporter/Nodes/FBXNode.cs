using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public abstract class FBXNode : FBXNodeList
    {
        public long Id { get { return -1; } set { } }
        public string Class { get { return string.Empty; } }
        public string SubClass { get { return string.Empty; } set { } }
        public int Count { get { return -1; } }

        public object Value;
        public List<FBXProperty> Properties = new List<FBXProperty>();


        protected void Node(string name, object value)
        {
            Nodes.Add(new FBXValueNode(name, value));
        }

        protected void Property(string name, string type, string label, string flags, object value)
        {
            Properties.Add(new FBXProperty(name, type, label, flags, value));
        }

        protected void Property(string name, string type, string label, string flags, Color value)
        {
            Property(name, type, label, flags, new[] { value.r, value.g, value.b });
        }
    }
}
