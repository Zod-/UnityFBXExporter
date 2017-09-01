namespace UnityFBXExporter
{
    public class FbxConnectionProperty : FbxValueNode
    {
        private readonly long _childId;

        public FbxConnectionProperty(long parentId, long childId, string connectionType = "OO") : base(connectionType, parentId)
        {
            _childId = childId;
        }

        public override string ToString()
        {
            return string.Format("C: \"{0}\",{1},{2}", Name, _childId, Value);
        }
    }
}
