using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxSurfacePhongTemplateNode : FbxPropertyTemplateBaseNode
    {
        public FbxSurfacePhongTemplateNode() : base("FbxSurfacePhong")
        {
            Property("ShadingModel", "KString", "", "", "Phong");
            Property("MultiLayer", "bool", "", "", 0);
            Property("EmissiveColor", "Color", "", "A", new Color(0, 0, 0));
            Property("EmissiveFactor", "Number", "", "A", 1);
            Property("AmbientColor", "Color", "", "A", new Color(0.2f, 0.2f, 0.2f));
            Property("AmbientFactor", "Number", "", "A", 1);
            Property("DiffuseColor", "Color", "", "A", new Color(0.8f, 0.8f, 0.8f));
            Property("DiffuseFactor", "Number", "", "A", 1);
            Property("Bump", "Vector3D", "Vector", "", Vector3.zero);
            Property("NormalMap", "Vector3D", "Vector", "", Vector3.zero);
            Property("BumpFactor", "double", "Number", "", 1);
            Property("TransparentColor", "Color", "", "A", Vector3.zero);
            Property("TransparencyFactor", "Number", "", "A", 0);
            Property("DisplacementColor", "ColorRGB", "Color", "", new Color(0, 0, 0));
            Property("DisplacementFactor", "double", "Number", "", 1);
            Property("VectorDisplacementColor", "ColorRGB", "Color", "", new Color(0, 0, 0));
            Property("VectorDisplacementFactor", "double", "Number", "", 1);
            Property("SpecularColor", "Color", "", "A", new Color(0.2f, 0.2f, 0.2f));
            Property("SpecularFactor", "Number", "", "A", 1);
            Property("ShininessExponent", "Number", "", "A", 20);
            Property("ReflectionColor", "Color", "", "A", new Color(0, 0, 0));
            Property("ReflectionFactor", "Number", "", "A", 1);
        }
    }
}
