namespace UnityFBXExporter
{
    public class FbxProperty : FbxValueNode
    {
        private readonly string _propertyName;
        private readonly string _type;
        private readonly string _label;
        private readonly string _flags;

        public FbxProperty(string propertyName, string type, string label, string flags, object value) : base("Properties70", value)
        {
            _propertyName = propertyName;
            _type = type;
            _label = label;
            _flags = flags;
        }

        public override string ToString()
        {
            return string.Format(@"P: ""{0}"", ""{1}"", ""{2}"", ""{3}"", {4}", _propertyName, _type, _label, _flags, Value);
        }
    }
}
