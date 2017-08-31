using System.Text;
using UnityFBXExporter;

namespace Assets.Packages.UnityFBXExporter.Writer
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
