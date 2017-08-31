using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public abstract class FbxNode : FbxNodeList
    {
        public int Count { get { return -1; } }

        public object Value;
        public List<FbxProperty> Properties = new List<FbxProperty>();


        protected void Node(string name, object value)
        {
            Nodes.Add(new FbxValueNode(name, value));
        }

        protected void Property(string name, string type, string label, string flags, object value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, value));
        }

        protected void Property(string name, string type, string label, string flags, Color value)
        {
            Property(name, type, label, flags, new[] { value.r, value.g, value.b });
        }

        protected void ArrayNode(string name, ICollection value)
        {
            Nodes.Add(new FbxArrayNode(name, value));
        }

        protected void ModelNode(GameObject gameObject)
        {
            Nodes.Add(new FbxModelNode(gameObject));
        }

        protected void GeometryNode(Mesh mesh)
        {
            Nodes.Add(new FbxGeometryNode(mesh));
        }

        public void MaterialNode(Material mat)
        {
            Nodes.Add(new FbxMaterialNode(mat));
        }

        public static long InstaceId(Object obj)
        {
            return Mathf.Abs(obj.GetInstanceID());
        }

    }
}
