﻿using System.Collections.Generic;
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

        protected void Property(string name, string type, string label, string flags, Color value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, new[] { value.r, value.g, value.b }));
        }

        protected void Property(string name, string type, string label, string flags, Vector3 value)
        {
            Properties.Add(new FbxProperty(name, type, label, flags, new[] { value.x, value.y, value.z }));
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
