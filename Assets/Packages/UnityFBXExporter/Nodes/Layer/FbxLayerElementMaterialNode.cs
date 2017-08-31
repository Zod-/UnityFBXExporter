using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxLayerElementMaterialNode : FbxLayerElementBaseNode
    {
        private readonly Mesh _mesh;

        public FbxLayerElementMaterialNode(int layer, Mesh mesh) : base("LayerElementMaterial", layer, "", "ByPolygon", "IndexToDirect")
        {
            _mesh = mesh;
            Materials();
        }

        public void Materials()
        {
            var triangles = _mesh.triangles;
            var numberOfSubMeshes = _mesh.subMeshCount;
            var materials = new List<int>(triangles.Length);

            // For just one subMesh, we set them all to zero
            if (numberOfSubMeshes != 1)
            {
                var allSubMeshes = new List<int[]>(numberOfSubMeshes);

                // Load all subMeshes into a space
                for (var i = 0; i < numberOfSubMeshes; i++)
                {
                    allSubMeshes.Add(_mesh.GetIndices(i));
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
                            triangles[i + 1] != allSubMeshes[subMeshIndex][n + 1] ||
                            triangles[i + 2] != allSubMeshes[subMeshIndex][n + 2])
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
