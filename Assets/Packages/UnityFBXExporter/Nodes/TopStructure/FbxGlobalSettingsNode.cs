using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxGlobalSettingsNode : FbxNode
    {
        public FbxGlobalSettingsNode() : base("GlobalSettings")
        {
            Node("Version", 1000);
            Property("UpAxis", "int", "Integer", "", 1);
            Property("UpAxisSign", "int", "Integer", "", 1);
            Property("FrontAxis", "int", "Integer", "", 2);
            Property("FrontAxisSign", "int", "Integer", "", 1);
            Property("CoordAxis", "int", "Integer", "", 0);
            Property("CoordAxisSign", "int", "Integer", "", 1);
            Property("OriginalUpAxis", "int", "Integer", "", -1);
            Property("OriginalUpAxisSign", "int", "Integer", "", 1);
            Property("UnitScaleFactor", "double", "Number", "", 100);
            Property("OriginalUnitScaleFactor", "double", "Number", "", 100);
            Property("AmbientColor", "ColorRGB", "Color", "", new Color(0, 0, 0));
            Property("DefaultCamera", "KString", "", "", "Producer Perspective");
            Property("TimeMode", "enum", "", "", 11);
            Property("TimeSpanStart", "KTime", "Time", "", 0);
            Property("TimeSpanStop", "KTime", "Time", "", 479181389250);
            Property("CustomFrameRate", "double", "Number", "", -1);
        }
    }
}
