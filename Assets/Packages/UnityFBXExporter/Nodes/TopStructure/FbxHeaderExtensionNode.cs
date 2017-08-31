using System;

namespace UnityFBXExporter
{
    public class FbxHeaderExtensionNode : FbxNode
    {
        public FbxHeaderExtensionNode() : base("FBXHeaderExtension")
        {
            Node("FBXHeaderVersion", 1003);
            Node("FBXVersion", 7300);
            Node("Creator", "FBX Unity Export version 1.2.1");
            CreationTimeStamp(DateTime.Now);
        }
    }
}
