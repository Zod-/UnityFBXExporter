using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxMeshTemplateNode : FbxPropertyTemplateBaseNode
    {
        public FbxMeshTemplateNode() : base("FbxMesh")
        {
            Property("Color", "ColorRGB", "Color", "", new Color(0.8f, 0.8f, 0.8f));
            Property("BBoxMin", "Vector3D", "Vector", "", Vector3.zero);
            Property("BBoxMax", "Vector3D", "Vector", "", Vector3.zero);
            Property("Primary Visibility", "bool", "", "", 1);
            Property("Casts Shadows", "bool", "", "", 1);
            Property("Receive Shadows", "bool", "", "", 1);
        }
    }
}
