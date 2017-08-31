
namespace UnityFBXExporter
{
    public class FbxDefinitionsNode : FbxNode
    {
        public FbxDefinitionsNode() : base("Definitions")
        {
            ChildNodes.Add(new FbxGlobalSettingsDefinitionNode());
            ChildNodes.Add(new FbxModelTemplateNode());
            ChildNodes.Add(new FbxGeometryTemplateNode());
            ChildNodes.Add(new FbxMaterialTemplateNode());
            //TODO Add Texture
        }
    }
}
