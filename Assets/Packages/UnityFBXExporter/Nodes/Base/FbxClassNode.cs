namespace UnityFBXExporter
{
    public class FbxClassNode : FbxNode
    {
        private readonly long _id;
        private readonly string _class;
        private readonly string _subClass;

        protected FbxClassNode(string name, long id, string clazz, string subClass = "") : base(name)
        {
            _id = id;
            _class = clazz;
            _subClass = subClass;
        }

        public override string GetMetaName()
        {
            if (_id < 0)
            {
                return string.Format("\"{0}::{1}\", \"{2}\"", Name, _class, _subClass);
            }
            return string.Format("{0} \"{1}::{2}\", \"{3}\"", _id, Name, _class, _subClass);
        }
    }
}
