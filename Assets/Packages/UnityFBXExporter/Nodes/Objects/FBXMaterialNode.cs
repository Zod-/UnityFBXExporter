using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxMaterialNode : FbxClassNode
    {
        private readonly Material _mat;

        public FbxMaterialNode(Material mat)
        {
            Name = "Material";
            Class = "Standard";
            Id = Mathf.Abs(mat.GetInstanceID());
            _mat = mat;

            Node("Version", 102);
            Node("ShadingModel", "phong");
            Node("MultiLayer", 0);
            DiffuseColor();
            EmissionColor();
            Mode();
            SpecColor();
        }

        private void DiffuseColor()
        {
            Property("Diffuse", "Vector3D", "Vector", "", _mat.color);
            Property("DiffuseColor", "Color", "", "A", _mat.color);
        }

        private void EmissionColor()
        {
            if (!_mat.HasProperty("_EmissionColor")) { return; }

            var color = _mat.GetColor("_EmissionColor");
            var averageColor = (color.r + color.g + color.b) / 3f;
            Property("Emissive", "Vector3D", "Vector", "", color);
            Property("EmissiveFactor", "Number", "", "A", averageColor);
        }

        private void Mode()
        {
            if (!_mat.HasProperty("_Mode")) { return; }

            switch ((int)_mat.GetFloat("_Mode"))
            {
                case 0: // Map is opaque
                    break;

                case 1: // Map is a cutout //  TODO: Add option if it is a cutout
                    break;

                case 2: // Map is a fade
                case 3: // Map is transparent
                    var color = _mat.GetColor("_Color");
                    Property("TransparentColor", "Color", "", "A", color);
                    Property("Opacity", "Number", "double", "", color.a);
                    break;
            }
        }

        private void SpecColor()
        {
            if (!_mat.HasProperty("_SpecColor")) { return; }
            var color = _mat.GetColor("_SpecColor");
            Property("Specular", "Vector3D", "Vector", "", color);
            Property("SpecularColor", "ColorRGB", "Color", "", color);
        }
    }
}
