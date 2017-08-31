namespace UnityFBXExporter
{
    public class FbxConnectionProperty
    {
        public long ParentId { get; private set; }
        public long ChildId { get; private set; }

        public FbxConnectionProperty(long parentId, long childId)
        {
            ParentId = parentId;
            ChildId = childId;
        }
    }
}
