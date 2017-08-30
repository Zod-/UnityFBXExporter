using UnityEngine;

namespace UnityFBXExporter
{
    public static class GameObjectExtensions
    {
        public static Mesh GetMesh(this GameObject gameObject)
        {
            var filter = gameObject.GetComponent<MeshFilter>();
            return filter == null ? null : filter.sharedMesh;
        }

        public static bool HasMesh(this GameObject gameObject)
        {
            return gameObject.GetMesh() != null;
        }
    }
}
