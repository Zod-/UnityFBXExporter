using System.Text;

namespace UnityFBXExporter
{
    public class FbxStringBuilderWriter : IWriter
    {
        public readonly StringBuilder Sb = new StringBuilder(100000);

        public void AppendLine(string value)
        {
            Sb.AppendLine(value);
        }
    }
}
