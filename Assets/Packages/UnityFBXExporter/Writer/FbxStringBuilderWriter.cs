using System.Text;

namespace UnityFBXExporter
{
    public class FbxStringBuilderWriter : IWriter
    {
        public StringBuilder Sb = new StringBuilder(100000);

        public void AppendLine(string value)
        {
            Sb.AppendLine(value);
        }
    }
}
