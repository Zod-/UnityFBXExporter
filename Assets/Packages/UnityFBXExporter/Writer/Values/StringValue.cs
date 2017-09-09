namespace UnityFBXExporter
{
    public class StringValue : FbxValue
    {
        private readonly string _string;

        public StringValue(string str)
        {
            _string = str;
        }

        public override string ToString()
        {
            return string.Format("\"{0}\"", _string);
        }
    }
}
