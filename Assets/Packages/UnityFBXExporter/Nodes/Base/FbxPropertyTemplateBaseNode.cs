namespace UnityFBXExporter
{
    public class FbxPropertyTemplateBaseNode : FbxNode
    {
        private readonly StringValue _definitionName;

        protected FbxPropertyTemplateBaseNode(string definitionName, string name = "") : base(string.IsNullOrEmpty(name) ? "PropertyTemplate" : name)
        {
            _definitionName = new StringValue(definitionName);
        }

        protected override string GetMetaName()
        {
            return FbxValueSerializer.Serialize(_definitionName);
        }
    }
}
