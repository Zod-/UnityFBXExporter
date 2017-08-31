namespace UnityFBXExporter
{
    public class FbxLayerElementBaseNode : FbxLayerBaseNode
    {
        public FbxLayerElementBaseNode(string name, int layer, string layerName, string mappingInformationType, string referenceInformationType) : base(name, layer)
        {
            Node("Name", layerName);
            Node("MappingInformationType", mappingInformationType);
            Node("ReferenceInformationType", referenceInformationType);
        }
    }
}
