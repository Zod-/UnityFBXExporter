using System;

namespace UnityFBXExporter
{
    public class FbxHeaderExtensionNode : FbxNode
    {
        public override string Header
        {
            get
            {
                return
@"; FBX 7.3.0 project file
; Copyright (C) 1997-2010 Autodesk Inc. and/or its licensors.
; All rights reserved.
; ----------------------------------------------------
";
            }
        }

        public FbxHeaderExtensionNode(string path) : base("FBXHeaderExtension")
        {
            Node("FBXHeaderVersion", 1003);
            Node("FBXVersion", 7300);
            Node("Creator", "FBX Unity Export version 1.2.1");
            ChildNodes.Add(new FbxCreationTimeStampNode(DateTime.Now));
            ChildNodes.Add(new FbxSceneInfoNode(path));
        }
    }
}
