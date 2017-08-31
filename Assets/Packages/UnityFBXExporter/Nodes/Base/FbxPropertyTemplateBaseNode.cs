namespace UnityFBXExporter
{
    public class FbxPropertyTemplateBaseNode : FbxNode
    {
        private readonly string _definitionName;

        protected FbxPropertyTemplateBaseNode(string definitionName, string name = "") : base(string.IsNullOrEmpty(name) ? "PropertyTemplate" : name)
        {
            _definitionName = definitionName;
        }

        public override string GetMetaName()
        {
            return FbxValueSerializer.Serialize(_definitionName);
        }
    }
}
