using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityFBXExporter
{
    public abstract class FbxNode
    {
        public string Name { get; private set; }

        public List<FbxNode> Nodes = new List<FbxNode>();
        public List<FbxProperty> Properties = new List<FbxProperty>();

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

        public void CreationTimeStamp(DateTime currentDate)
        {
            Nodes.Add(new FbxCreationTimeStampNode(currentDate));
        }

        public void MetaData()
        {
            Nodes.Add(new FbxMetaDataNode());
        }


        public static long InstaceId(Object obj)
        {
            return Mathf.Abs(obj.GetInstanceID());
        }

    }
}
