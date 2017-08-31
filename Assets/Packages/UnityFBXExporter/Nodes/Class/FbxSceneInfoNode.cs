namespace UnityFBXExporter
{
    public class FbxSceneInfoNode : FbxClassNode
    {
        public FbxSceneInfoNode(string path) : base("SceneInfo", -1, "GlobalInfo", "UserData")
        {
            Node("Type", "UserData");
            Node("Version", 100);
            MetaData();
            Property("DocumentUrl", "KString", "Url", "", path);
            Property("SrcDocumentUrl", "KString", "Url", "", path);
            Property("Original", "Compound", "", "", "");
            Property("Original|ApplicationVendor", "KString", "", "", "");
            Property("Original|ApplicationName", "KString", "", "", "");
            Property("Original|ApplicationVersion", "KString", "", "", "");
            Property("Original|DateTime_GMT", "DateTime", "", "", "");
            Property("Original|FileName", "KString", "", "", "");
            Property("LastSaved", "Compound", "", "", "");
            Property("LastSaved|ApplicationVendor", "KString", "", "", "");
            Property("LastSaved|ApplicationVersion", "KString", "", "", "");
            Property("LastSaved|DateTime_GMT", "DateTime", "", "", "");
        }
    }
}
