
namespace UnityFBXExporter
{
    public class FbxDefinitionsNode : FbxNode
    {
        public FbxDefinitionsNode() : base("Definitions")
        {
            Nodes.Add(new FbxGlobalSettingsDefinitionNode());
            Nodes.Add(new FbxModelTemplateNode());
            Nodes.Add(new FbxGeometryTemplateNode());
            Nodes.Add(new FbxMaterialTemplateNode());
            //TODO Add Texture
        }
    }
}
