namespace UnityFBXExporter
{
    public class FbxArrayNode : FbxNode
    {
        private readonly int _length;

        public FbxArrayNode(string name, object value, int length) : base(name)
        {
            _length = length;
            Node("a", value);
        }

        protected override string GetMetaName()
        {
            return string.Format("*{0}", _length);
        }
    }
}
