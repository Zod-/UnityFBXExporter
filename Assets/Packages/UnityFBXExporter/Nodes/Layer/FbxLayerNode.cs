using System.Collections.Generic;

namespace UnityFBXExporter
{
    public class FbxLayerNode : FbxLayerBaseNode
    {
        public FbxLayerNode(int layer, ICollection<FbxLayerElementBaseNode> layerElements) : base("Layer", layer, 100)
        {
            foreach (var layerElement in layerElements)
            {
                ChildNodes.Add(new FbxLayerConnectionNode(layerElement.Name));
            }
        }
    }
}
