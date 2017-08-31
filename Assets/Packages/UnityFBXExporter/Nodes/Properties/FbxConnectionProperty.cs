namespace UnityFBXExporter
{
    public class FbxConnectionProperty
    {
        public string Type { get; private set; }
            
        public long ParentId { get; private set; }
        public long ChildId { get; private set; }

        public FbxConnectionProperty(long parentId, long childId, string connectionType = "OO")
        {
            ParentId = parentId;
            ChildId = childId;
            Type = connectionType;
        }
    }
}
