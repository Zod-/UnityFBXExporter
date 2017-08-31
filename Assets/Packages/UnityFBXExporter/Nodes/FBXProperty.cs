namespace UnityFBXExporter
{
    public class FbxProperty
    {
        public string Name;
        public string Type;
        public string Label;
        public string Flags;
        public object Value;
        
        public FbxProperty(string name, string type, string label, string flags, object value)
        {
            Name = name;
            Type = type;
            Label = label;
            Flags = flags;
            Value = value;
        }
    }
}
