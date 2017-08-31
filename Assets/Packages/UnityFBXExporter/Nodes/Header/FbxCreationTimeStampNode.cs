using System;

namespace UnityFBXExporter
{
    public class FbxCreationTimeStampNode : FbxNode
    {
        public FbxCreationTimeStampNode(DateTime currentDate) : base("CreationTimeStamp")
        {
            Node("Version", 1000);
            Node("Year", currentDate.Year);
            Node("Month", currentDate.Month);
            Node("Day", currentDate.Day);
            Node("Hour", currentDate.Hour);
            Node("Minute", currentDate.Minute);
            Node("Second", currentDate.Second);
            Node("Millisecond", currentDate.Millisecond);
        }
    }
}
