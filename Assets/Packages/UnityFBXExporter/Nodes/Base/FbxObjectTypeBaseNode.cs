namespace UnityFBXExporter
{
    public class FbxObjectTypeBaseNode : FbxPropertyTemplateBaseNode
    {
        public FbxObjectTypeBaseNode(string definitionName, int count) : base(definitionName, "ObjectType")
        {
            Node("Count", count);
        }
    }
}
