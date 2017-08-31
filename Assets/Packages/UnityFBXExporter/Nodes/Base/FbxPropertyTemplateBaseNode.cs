namespace UnityFBXExporter
{
    public class FbxPropertyTemplateBaseNode : FbxNode
    {
        public string DefinitionName { get; private set; }
        public FbxPropertyTemplateBaseNode(string definitionName, string name = "") : base(string.IsNullOrEmpty(name) ? "PropertyTemplate" : name)
        {
            DefinitionName = definitionName;
        }
    }
}
