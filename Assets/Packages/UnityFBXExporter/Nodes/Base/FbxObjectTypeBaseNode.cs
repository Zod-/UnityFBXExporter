namespace UnityFBXExporter
{
    public class FbxObjectTypeBaseNode : FbxPropertyTemplateBaseNode
    {
        protected FbxObjectTypeBaseNode(string definitionName, int count) : base(definitionName, "ObjectType")
        {
            Node("Count", count);
        }
    }
}
