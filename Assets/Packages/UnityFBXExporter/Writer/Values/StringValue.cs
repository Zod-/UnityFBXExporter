using System.Text;

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
            var sb = new StringBuilder(_string.Length + 2);
            sb.Append('"');
            sb.Append(_string);
            sb.Append('"');
            return sb.ToString();
        }
    }
}
