using System.Collections.Generic;

namespace UnityFBXExporter
{
    public class FbxLayerElementMaterialNode : FbxLayerElementBaseNode
    {
        private readonly MeshCache _cache;

        public FbxLayerElementMaterialNode(int layer, MeshCache cache) : base("LayerElementMaterial", layer, "", "ByPolygon", "IndexToDirect")
        {
            _cache = cache;
            Materials();
        }

        public void Materials()
        {
            var triangles = _cache.Triangles;
            var numberOfSubMeshes = _cache.Mesh.subMeshCount;
            var materials = new List<int>(triangles.Length);

            // For just one subMesh, we set them all to zero
            if (numberOfSubMeshes != 1)
            {
                var allSubMeshes = new List<int[]>(numberOfSubMeshes);

                // Load all subMeshes into a space
                for (var i = 0; i < numberOfSubMeshes; i++)
                {
                    allSubMeshes.Add(_cache.Mesh.GetIndices(i));
                }

                materials.AddRange(FindSubMeshIndex(triangles, allSubMeshes));
            }
            else
            {
                for (var i = 0; i < triangles.Length; i += 3)
                {
                    materials.Add(0);
                }
            }

            ArrayNode("Materials", materials.ToArray());
        }

        private static IEnumerable<int> FindSubMeshIndex(int[] triangles, List<int[]> allSubMeshes)
        {
            // TODO: Optimize this search pattern
            for (var i = 0; i < triangles.Length; i += 3)
            {
                for (var subMeshIndex = 0; subMeshIndex < allSubMeshes.Count; subMeshIndex++)
                {
                    for (var n = 0; n < allSubMeshes[subMeshIndex].Length; n += 3)
                    {
                        if (triangles[i] != allSubMeshes[subMeshIndex][n] ||
                            triangles[i + 2] != allSubMeshes[subMeshIndex][n + 1] ||
                            triangles[i + 1] != allSubMeshes[subMeshIndex][n + 2])
                        {
                            continue;
                        }
                        yield return subMeshIndex;
                    }
                }
            }
        }
    }
}
