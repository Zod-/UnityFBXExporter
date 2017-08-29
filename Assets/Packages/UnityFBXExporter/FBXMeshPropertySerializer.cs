// ===============================================================================================
//	The MIT License (MIT) for UnityFBXExporter
//
//  UnityFBXExporter was created for Building Crafter (http://u3d.as/ovC) a tool to rapidly 
//	create high quality buildings right in Unity with no need to use 3D modeling programs.
//
//  Copyright (c) 2016 | 8Bit Goose Games, Inc.
//		
//	Permission is hereby granted, free of charge, to any person obtaining a copy 
//	of this software and associated documentation files (the "Software"), to deal 
//	in the Software without restriction, including without limitation the rights 
//	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
//	of the Software, and to permit persons to whom the Software is furnished to do so, 
//	subject to the following conditions:
//		
//	The above copyright notice and this permission notice shall be included in all 
//	copies or substantial portions of the Software.
//		
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
//	PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
//	HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
//	OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
//	OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// ===============================================================================================

using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnityFBXExporter
{
    public static class FBXMeshPropertySerializer
    {
        public static void Serialize(Mesh mesh, Renderer renderer, long modelId, StringBuilder objectsSb, StringBuilder connectionsSb)
        {
            var geometryId = FBXExporter.GetRandomFBXId();
            MeshGeometryHeader(geometryId, objectsSb);
            {
                SerializeMesh(mesh, objectsSb);
            }
            MeshGeometryFooter(objectsSb);
            // Add the connection for the model to the geometry so it is attached the right mesh
            SerializeMeshConnections(renderer, mesh.name, geometryId, modelId, connectionsSb);
        }

        /// <summary>
        /// Adds in geometry if it exists, if it it does not exist, this is a empty gameObject file and skips over this
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="objectsSb"></param>
        private static void SerializeMesh(Mesh mesh, StringBuilder objectsSb)
        {
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;
            var containsColors = mesh.colors.Length == vertices.Length;
            SerializeVertices(mesh, vertices, objectsSb);
            SerializeTriangles(triangles, objectsSb);
            SerializeNormals(mesh, triangles, objectsSb);
            SerializeColors(mesh, containsColors, triangles, objectsSb);
            SerializeUv1(mesh, triangles, objectsSb);
            SerializeUv2();
            SerializeSmoothing();
            SerializeMaterials(mesh, triangles, objectsSb);
            SerializeLayerElements(containsColors, objectsSb);
        }

        private static void MeshGeometryFooter(StringBuilder objectsSb)
        {
            objectsSb.AppendLine("\t}");
        }

        private static void MeshGeometryHeader(long geometryId, StringBuilder objectsSb)
        {
            objectsSb.AppendLine("\tGeometry: " + geometryId + ", \"Geometry::\", \"Mesh\" {");
        }

        private static void SerializeVertices(Mesh mesh, Vector3[] vertices, StringBuilder objectsSb)
        {
            objectsSb.AppendLine("\t\tVertices: *" + mesh.vertexCount * 3 + " {");
            objectsSb.Append("\t\t\ta: ");
            for (var i = 0; i < vertices.Length; i++)
            {
                if (i > 0)
                {
                    objectsSb.Append(",");
                }

                // Points in the vertices. We also reverse the x value because Unity has a reverse X coordinate
                objectsSb.AppendFormat("{0},{1},{2}", vertices[i].x * -1, vertices[i].y, vertices[i].z);
            }

            objectsSb.AppendLine();
            objectsSb.AppendLine("\t\t} ");
        }

        private static void SerializeColors(Mesh mesh, bool containsColors, int[] triangles, StringBuilder objectsSb)
        {
            if (containsColors)
            {
                SerializeColor(mesh, triangles, objectsSb);
            }
        }

        private static void SerializeTriangles(int[] triangles, StringBuilder objectsSb)
        {
            objectsSb.AppendLine("\t\tPolygonVertexIndex: *" + triangles.Length + " {");

            // Write triangle indexes
            objectsSb.Append("\t\t\ta: ");
            for (var i = 0; i < triangles.Length; i += 3)
            {
                if (i > 0)
                {
                    objectsSb.Append(",");
                }

                // To get the correct normals, must rewind the triangles since we flipped the x direction
                objectsSb.AppendFormat("{0},{1},{2}",
                    triangles[i],
                    triangles[i + 2],
                    (triangles[i + 1] * -1) - 1); // <= Tells the poly is ended

            }

            objectsSb.AppendLine();

            objectsSb.AppendLine("\t\t} ");
            objectsSb.AppendLine("\t\tGeometryVersion: 124");
            objectsSb.AppendLine("\t\tLayerElementNormal: 0 {");
            objectsSb.AppendLine("\t\t\tVersion: 101");
            objectsSb.AppendLine("\t\t\tName: \"\"");
            objectsSb.AppendLine("\t\t\tMappingInformationType: \"ByPolygonVertex\"");
            objectsSb.AppendLine("\t\t\tReferenceInformationType: \"Direct\"");
        }

        private static void SerializeNormals(Mesh mesh, int[] triangles, StringBuilder objectsSb)
        {
            var normals = mesh.normals;

            objectsSb.AppendLine("\t\t\tNormals: *" + (triangles.Length * 3) + " {");
            objectsSb.Append("\t\t\t\ta: ");

            for (var i = 0; i < triangles.Length; i += 3)
            {
                if (i > 0)
                {
                    objectsSb.Append(",");
                }

                // To get the correct normals, must rewind the normal triangles like the triangles above since x was flipped
                var newNormal = normals[triangles[i]];

                objectsSb.AppendFormat("{0},{1},{2},",
                    newNormal.x * -1, // Switch normal as is tradition
                    newNormal.y,
                    newNormal.z);

                newNormal = normals[triangles[i + 2]];

                objectsSb.AppendFormat("{0},{1},{2},",
                    newNormal.x * -1, // Switch normal as is tradition
                    newNormal.y,
                    newNormal.z);

                newNormal = normals[triangles[i + 1]];

                objectsSb.AppendFormat("{0},{1},{2}",
                    newNormal.x * -1, // Switch normal as is tradition
                    newNormal.y,
                    newNormal.z);
            }

            objectsSb.AppendLine();
            objectsSb.AppendLine("\t\t\t}");
            objectsSb.AppendLine("\t\t}");
        }

        private static void SerializeLayerElements(bool containsColors, StringBuilder objectsSb)
        {
            // ============= INFORMS WHAT TYPE OF LATER ELEMENTS ARE IN THIS GEOMETRY =================
            objectsSb.AppendLine("\t\tLayer: 0 {");
            objectsSb.AppendLine("\t\t\tVersion: 100");
            objectsSb.AppendLine("\t\t\tLayerElement:  {");
            objectsSb.AppendLine("\t\t\t\tType: \"LayerElementNormal\"");
            objectsSb.AppendLine("\t\t\t\tTypedIndex: 0");
            objectsSb.AppendLine("\t\t\t}");
            objectsSb.AppendLine("\t\t\tLayerElement:  {");
            objectsSb.AppendLine("\t\t\t\tType: \"LayerElementMaterial\"");
            objectsSb.AppendLine("\t\t\t\tTypedIndex: 0");
            objectsSb.AppendLine("\t\t\t}");
            objectsSb.AppendLine("\t\t\tLayerElement:  {");
            objectsSb.AppendLine("\t\t\t\tType: \"LayerElementTexture\"");
            objectsSb.AppendLine("\t\t\t\tTypedIndex: 0");
            objectsSb.AppendLine("\t\t\t}");
            if (containsColors)
            {
                objectsSb.AppendLine("\t\t\tLayerElement:  {");
                objectsSb.AppendLine("\t\t\t\tType: \"LayerElementColor\"");
                objectsSb.AppendLine("\t\t\t\tTypedIndex: 0");
                objectsSb.AppendLine("\t\t\t}");
            }
            objectsSb.AppendLine("\t\t\tLayerElement:  {");
            objectsSb.AppendLine("\t\t\t\tType: \"LayerElementUV\"");
            objectsSb.AppendLine("\t\t\t\tTypedIndex: 0");
            objectsSb.AppendLine("\t\t\t}");
            // TODO: Here we would add UV layer 1 for ambient occlusion UV file
            //			objectsSb.AppendLine("\t\t\tLayerElement:  {");
            //			objectsSb.AppendLine("\t\t\t\tType: \"LayerElementUV\"");
            //			objectsSb.AppendLine("\t\t\t\tTypedIndex: 1");
            //			objectsSb.AppendLine("\t\t\t}");
            objectsSb.AppendLine("\t\t}");
        }

        private static void SerializeMaterials(Mesh mesh, int[] triangles, StringBuilder objectsSb)
        {
            objectsSb.AppendLine("\t\tLayerElementMaterial: 0 {");
            objectsSb.AppendLine("\t\t\tVersion: 101");
            objectsSb.AppendLine("\t\t\tName: \"\"");
            objectsSb.AppendLine("\t\t\tMappingInformationType: \"ByPolygon\"");
            objectsSb.AppendLine("\t\t\tReferenceInformationType: \"IndexToDirect\"");

            // So by polygon means that we need 1/3rd of how many indices we wrote.

            var subMeshesSb = new StringBuilder();
            var totalFaceCount = SerializeSubMeshes(mesh, triangles, subMeshesSb);

            objectsSb.AppendLine("\t\t\tMaterials: *" + totalFaceCount + " {");
            objectsSb.Append("\t\t\t\ta: ");
            objectsSb.AppendLine(subMeshesSb.ToString());
            objectsSb.AppendLine("\t\t\t} ");
            objectsSb.AppendLine("\t\t}");
        }

        private static void SerializeUv1(Mesh mesh, int[] triangles, StringBuilder objectsSb)
        {
            var uvLength = mesh.uv.Length;
            var uvs = mesh.uv;

            objectsSb.AppendLine("\t\tLayerElementUV: 0 {"); // the Zero here is for the first UV map
            objectsSb.AppendLine("\t\t\tVersion: 101");
            objectsSb.AppendLine("\t\t\tName: \"map1\"");
            objectsSb.AppendLine("\t\t\tMappingInformationType: \"ByPolygonVertex\"");
            objectsSb.AppendLine("\t\t\tReferenceInformationType: \"IndexToDirect\"");
            objectsSb.AppendLine("\t\t\tUV: *" + uvLength * 2 + " {");
            objectsSb.Append("\t\t\t\ta: ");

            for (var i = 0; i < uvLength; i++)
            {
                if (i > 0)
                {
                    objectsSb.Append(",");
                }

                objectsSb.AppendFormat("{0},{1}", uvs[i].x, uvs[i].y);

            }
            objectsSb.AppendLine();

            objectsSb.AppendLine("\t\t\t\t}");

            // UV tile index coords
            objectsSb.AppendLine("\t\t\tUVIndex: *" + triangles.Length + " {");
            objectsSb.Append("\t\t\t\ta: ");

            for (var i = 0; i < triangles.Length; i += 3)
            {
                if (i > 0)
                {
                    objectsSb.Append(",");
                }

                // Triangles need to be flipped for the x flip
                var index1 = triangles[i];
                var index2 = triangles[i + 2];
                var index3 = triangles[i + 1];

                objectsSb.AppendFormat("{0},{1},{2}", index1, index2, index3);
            }

            objectsSb.AppendLine();

            objectsSb.AppendLine("\t\t\t}");
            objectsSb.AppendLine("\t\t}");
        }

        private static void SerializeUv2()
        {
            // -- UV 2 Creation
            // TODO: Add UV2 Creation here
        }

        private static void SerializeSmoothing()
        {
            // -- Smoothing
            // TODO: Smoothing doesn't seem to do anything when importing. This maybe should be added. -KBH
        }

        private static void SerializeColor(Mesh mesh, int[] triangles, StringBuilder objectsSb)
        {
            var colors = mesh.colors;

            var colorTable = new Dictionary<Color, int>(); // reducing amount of data by only keeping unique colors.
            var idx = 0;

            // build index table of all the different colors present in the mesh            
            foreach (var color in colors)
            {
                if (colorTable.ContainsKey(color)) { continue; }
                colorTable[color] = idx;
                idx++;
            }

            objectsSb.AppendLine("\t\tLayerElementColor: 0 {");
            objectsSb.AppendLine("\t\t\tVersion: 101");
            objectsSb.AppendLine("\t\t\tName: \"Col\"");
            objectsSb.AppendLine("\t\t\tMappingInformationType: \"ByPolygonVertex\"");
            objectsSb.AppendLine("\t\t\tReferenceInformationType: \"IndexToDirect\"");
            objectsSb.AppendLine("\t\t\tColors: *" + colorTable.Count * 4 + " {");
            objectsSb.Append("\t\t\t\ta: ");

            var first = true;
            foreach (var color in colorTable)
            {
                if (!first)
                {
                    objectsSb.Append(",");
                }

                objectsSb.AppendFormat("{0},{1},{2},{3}", color.Key.r, color.Key.g, color.Key.b, color.Key.a);
                first = false;
            }
            objectsSb.AppendLine();

            objectsSb.AppendLine("\t\t\t\t}");

            // Color index
            objectsSb.AppendLine("\t\t\tColorIndex: *" + triangles.Length + " {");
            objectsSb.Append("\t\t\t\ta: ");

            for (var i = 0; i < triangles.Length; i += 3)
            {
                if (i > 0)
                {
                    objectsSb.Append(",");
                }

                // Triangles need to be flipped for the x flip
                var index1 = triangles[i];
                var index2 = triangles[i + 2];
                var index3 = triangles[i + 1];

                // Find the color index related to that vertices index
                index1 = colorTable[colors[index1]];
                index2 = colorTable[colors[index2]];
                index3 = colorTable[colors[index3]];

                objectsSb.AppendFormat("{0},{1},{2}", index1, index2, index3);
            }

            objectsSb.AppendLine();

            objectsSb.AppendLine("\t\t\t}");
            objectsSb.AppendLine("\t\t}");
        }

        private static int SerializeSubMeshes(Mesh mesh, int[] triangles, StringBuilder subMeshesSb)
        {
            var totalFaceCount = 0;
            var numberOfSubMeshes = mesh.subMeshCount;

            // For just one subMesh, we set them all to zero
            if (numberOfSubMeshes == 1)
            {
                var numFaces = triangles.Length / 3;

                for (var i = 0; i < numFaces; i++)
                {
                    subMeshesSb.Append("0,");
                    totalFaceCount++;
                }
            }
            else
            {
                var allSubMeshes = new List<int[]>();

                // Load all subMeshes into a space
                for (var i = 0; i < numberOfSubMeshes; i++)
                {
                    allSubMeshes.Add(mesh.GetIndices(i));
                }

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
                            subMeshesSb.Append(subMeshIndex.ToString());
                            subMeshesSb.Append(",");
                            totalFaceCount++;
                            break;
                        }
                    }
                }
            }

            return totalFaceCount;
        }

        private static void SerializeMeshConnections(Renderer renderer, string meshName, long geometryId, long modelId, StringBuilder connectionsSb)
        {
            connectionsSb.AppendLine("\t;Geometry::, Model::" + meshName);
            connectionsSb.AppendLine("\tC: \"OO\"," + geometryId + "," + modelId);
            connectionsSb.AppendLine();

            // Add the connection of all the materials in order of subMesh
            if (renderer == null) { return; }
            var allMaterialsInThisMesh = renderer.sharedMaterials;

            foreach (var mat in allMaterialsInThisMesh)
            {
                if (mat == null)
                {
                    Debug.LogError("ERROR: the game object " + renderer.name + " has an empty material on it. This will export problematic files. Please fix and reexport");
                    continue;
                }
                var referenceId = Mathf.Abs(mat.GetInstanceID());
                connectionsSb.AppendLine("\t;Material::" + mat.name + ", Model::" + meshName);
                connectionsSb.AppendLine("\tC: \"OO\"," + referenceId + "," + modelId);
                connectionsSb.AppendLine();
            }
        }
    }
}
