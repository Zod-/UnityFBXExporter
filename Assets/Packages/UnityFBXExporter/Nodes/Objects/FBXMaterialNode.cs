using UnityEngine;

namespace UnityFBXExporter
{
    public class FBXMaterialNode : FBXNode
    {
        public new string Name { get { return "Material"; } }
        public new string Class { get { return "Standard"; } }
        public new int Id { get { return Mathf.Abs(_mat.GetInstanceID()); } }

        private Material _mat;

        public FBXMaterialNode(Material mat)
        {
            _mat = mat;
            Node("Version", 102);
            Node("ShadingModel", "phong");
            Node("MultiLayer", 0);
            Property("Diffuse", "Vector3D", "Vector", "", mat.color);
            Property("DiffuseColor", "Color", "", "A", mat.color);
            EmissionColor();
        }

        private void EmissionColor()
        {
            if (!_mat.HasProperty("_EmissionColor")) { return; }
            var color = _mat.GetColor("_EmissionColor");

        }
    }
}
