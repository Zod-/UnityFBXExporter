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

using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace UnityFBXExporter
{
    public static class FBXUnityMeshGetter
    {
        /// <summary>
        /// Gets all the meshes and outputs to a string (even grabbing the child of each gameObject)
        /// </summary>
        /// <returns>The mesh to string.</returns>
        /// <param name="gameObj">GameObject Parent.</param>
        /// <param name="objectsSb">The StringBuidler to create objects for the FBX file.</param>
        /// <param name="connectionsSb">The StringBuidler to create connections for the FBX file.</param>
        /// <param name="parentModelId">Parent model id, 0 if top parent.</param>
        public static void GetMeshToString(GameObject gameObj, StringBuilder objectsSb, StringBuilder connectionsSb, long parentModelId = 0)
        {
            var geometryId = FBXExporter.GetRandomFBXId();
            var modelId = FBXExporter.GetRandomFBXId();
            //@cartzhang if SkinnedMeshRender gameobject,but has no meshfilter,add one.            
            var meshfilterRender = gameObj.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (var i = 0; i < meshfilterRender.Length; i++)
            {
                if (meshfilterRender[i].GetComponent<MeshFilter>() == null)
                {
                    meshfilterRender[i].gameObject.AddComponent<MeshFilter>();
                    meshfilterRender[i].GetComponent<MeshFilter>().sharedMesh = Object.Instantiate(meshfilterRender[i].sharedMesh);
                }
            }

            // Sees if there is a mesh to export and add to the system
            var filter = gameObj.GetComponent<MeshFilter>();

            var meshName = gameObj.name;

            // A NULL parent means that the gameObject is at the top
            var isMesh = "Null";

            if (filter != null)
            {
                if (filter.sharedMesh == null)
                {
                    // The MeshFilter has no mesh assigned, so treat it like an FBX Null node.
                    filter = null;
                }
                else
                {
                    meshName = filter.sharedMesh.name;
                    isMesh = "Mesh";
                }
            }

            if (parentModelId == 0)
            {
                connectionsSb.AppendLine("\t;Model::" + meshName + ", Model::RootNode");
            }
            else
            {
                connectionsSb.AppendLine("\t;Model::" + meshName + ", Model::USING PARENT");
            }
            connectionsSb.AppendLine("\tC: \"OO\"," + modelId + "," + parentModelId);
            connectionsSb.AppendLine();
            objectsSb.AppendLine("\tModel: " + modelId + ", \"Model::" + gameObj.name + "\", \"" + isMesh + "\" {");
            objectsSb.AppendLine("\t\tVersion: 232");
            objectsSb.AppendLine("\t\tProperties70:  {");
            objectsSb.AppendLine("\t\t\tP: \"RotationOrder\", \"enum\", \"\", \"\",4");
            objectsSb.AppendLine("\t\t\tP: \"RotationActive\", \"bool\", \"\", \"\",1");
            objectsSb.AppendLine("\t\t\tP: \"InheritType\", \"enum\", \"\", \"\",1");
            objectsSb.AppendLine("\t\t\tP: \"ScalingMax\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            objectsSb.AppendLine("\t\t\tP: \"DefaultAttributeIndex\", \"int\", \"Integer\", \"\",0");
            // ===== Local Translation Offset =========
            var position = gameObj.transform.localPosition;

            objectsSb.Append("\t\t\tP: \"Lcl Translation\", \"Lcl Translation\", \"\", \"A+\",");

            // Append the X Y Z coords to the system
            objectsSb.AppendFormat("{0},{1},{2}", position.x * -1, position.y, position.z);
            objectsSb.AppendLine();

            // Rotates the object correctly from Unity space
            var localRotation = gameObj.transform.localEulerAngles;
            objectsSb.AppendFormat("\t\t\tP: \"Lcl Rotation\", \"Lcl Rotation\", \"\", \"A+\",{0},{1},{2}", localRotation.x, localRotation.y * -1, -1 * localRotation.z);
            objectsSb.AppendLine();

            // Adds the local scale of this object
            var localScale = gameObj.transform.localScale;
            objectsSb.AppendFormat("\t\t\tP: \"Lcl Scaling\", \"Lcl Scaling\", \"\", \"A\",{0},{1},{2}", localScale.x, localScale.y, localScale.z);
            objectsSb.AppendLine();

            objectsSb.AppendLine("\t\t\tP: \"currentUVSet\", \"KString\", \"\", \"U\", \"map1\"");
            objectsSb.AppendLine("\t\t}");
            objectsSb.AppendLine("\t\tShading: T");
            objectsSb.AppendLine("\t\tCulling: \"CullingOff\"");
            objectsSb.AppendLine("\t}");


            // Adds in geometry if it exists, if it it does not exist, this is a empty gameObject file and skips over this
            if (filter != null)
            {
                var mesh = filter.sharedMesh;

                // =================================
                //         General Geometry Info
                // =================================
                // Generate the geometry information for the mesh created

                objectsSb.AppendLine("\tGeometry: " + geometryId + ", \"Geometry::\", \"Mesh\" {");

                // ===== WRITE THE VERTICIES =====
                var verticies = mesh.vertices;
                var vertCount = mesh.vertexCount * 3; // <= because the list of points is just a list of comma seperated values, we need to multiply by three

                objectsSb.AppendLine("\t\tVertices: *" + vertCount + " {");
                objectsSb.Append("\t\t\ta: ");
                for (var i = 0; i < verticies.Length; i++)
                {
                    if (i > 0)
                    {
                        objectsSb.Append(",");
                    }

                    // Points in the verticies. We also reverse the x value because Unity has a reverse X coordinate
                    objectsSb.AppendFormat("{0},{1},{2}", verticies[i].x * -1, verticies[i].y, verticies[i].z);
                }

                objectsSb.AppendLine();
                objectsSb.AppendLine("\t\t} ");

                // ======= WRITE THE TRIANGLES ========
                var triangleCount = mesh.triangles.Length;
                var triangles = mesh.triangles;

                objectsSb.AppendLine("\t\tPolygonVertexIndex: *" + triangleCount + " {");

                // Write triangle indexes
                objectsSb.Append("\t\t\ta: ");
                for (var i = 0; i < triangleCount; i += 3)
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

                // ===== WRITE THE NORMALS ==========
                var normals = mesh.normals;

                objectsSb.AppendLine("\t\t\tNormals: *" + (triangleCount * 3) + " {");
                objectsSb.Append("\t\t\t\ta: ");

                for (var i = 0; i < triangleCount; i += 3)
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

                // ===== WRITE THE COLORS =====
                var containsColors = mesh.colors.Length == verticies.Length;

                if (containsColors)
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

                        // Triangles need to be fliped for the x flip
                        var index1 = triangles[i];
                        var index2 = triangles[i + 2];
                        var index3 = triangles[i + 1];

                        // Find the color index related to that vertice index
                        index1 = colorTable[colors[index1]];
                        index2 = colorTable[colors[index2]];
                        index3 = colorTable[colors[index3]];

                        objectsSb.AppendFormat("{0},{1},{2}", index1, index2, index3);
                    }

                    objectsSb.AppendLine();

                    objectsSb.AppendLine("\t\t\t}");
                    objectsSb.AppendLine("\t\t}");
                }
                else
                {
                    Debug.LogWarning("Mesh contains " + mesh.vertices.Length + " vertices for " + mesh.colors.Length + " colors. Skip color export");
                }


                // ================ UV CREATION =========================

                // -- UV 1 Creation
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
                objectsSb.AppendLine("\t\t\tUVIndex: *" + triangleCount + " {");
                objectsSb.Append("\t\t\t\ta: ");

                for (var i = 0; i < triangleCount; i += 3)
                {
                    if (i > 0)
                    {
                        objectsSb.Append(",");
                    }

                    // Triangles need to be fliped for the x flip
                    var index1 = triangles[i];
                    var index2 = triangles[i + 2];
                    var index3 = triangles[i + 1];

                    objectsSb.AppendFormat("{0},{1},{2}", index1, index2, index3);
                }

                objectsSb.AppendLine();

                objectsSb.AppendLine("\t\t\t}");
                objectsSb.AppendLine("\t\t}");

                // -- UV 2 Creation
                // TODO: Add UV2 Creation here

                // -- Smoothing
                // TODO: Smoothing doesn't seem to do anything when importing. This maybe should be added. -KBH

                // ============ MATERIALS =============

                objectsSb.AppendLine("\t\tLayerElementMaterial: 0 {");
                objectsSb.AppendLine("\t\t\tVersion: 101");
                objectsSb.AppendLine("\t\t\tName: \"\"");
                objectsSb.AppendLine("\t\t\tMappingInformationType: \"ByPolygon\"");
                objectsSb.AppendLine("\t\t\tReferenceInformationType: \"IndexToDirect\"");

                var totalFaceCount = 0;

                // So by polygon means that we need 1/3rd of how many indicies we wrote.
                var numberOfSubmeshes = mesh.subMeshCount;

                var submeshesSb = new StringBuilder();

                // For just one submesh, we set them all to zero
                if (numberOfSubmeshes == 1)
                {
                    var numFaces = triangles.Length / 3;

                    for (var i = 0; i < numFaces; i++)
                    {
                        submeshesSb.Append("0,");
                        totalFaceCount++;
                    }
                }
                else
                {
                    var allSubmeshes = new List<int[]>();

                    // Load all submeshes into a space
                    for (var i = 0; i < numberOfSubmeshes; i++)
                    {
                        allSubmeshes.Add(mesh.GetIndices(i));
                    }

                    // TODO: Optimize this search pattern
                    for (var i = 0; i < triangles.Length; i += 3)
                    {
                        for (var subMeshIndex = 0; subMeshIndex < allSubmeshes.Count; subMeshIndex++)
                        {
                            for (var n = 0; n < allSubmeshes[subMeshIndex].Length; n += 3)
                            {
                                if (triangles[i] == allSubmeshes[subMeshIndex][n]
                                   && triangles[i + 1] == allSubmeshes[subMeshIndex][n + 1]
                                   && triangles[i + 2] == allSubmeshes[subMeshIndex][n + 2])
                                {
                                    submeshesSb.Append(subMeshIndex.ToString());
                                    submeshesSb.Append(",");
                                    totalFaceCount++;
                                    break;
                                }
                            }
                        }
                    }
                }

                objectsSb.AppendLine("\t\t\tMaterials: *" + totalFaceCount + " {");
                objectsSb.Append("\t\t\t\ta: ");
                objectsSb.AppendLine(submeshesSb.ToString());
                objectsSb.AppendLine("\t\t\t} ");
                objectsSb.AppendLine("\t\t}");

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
                objectsSb.AppendLine("\t}");

                // Add the connection for the model to the geometry so it is attached the right mesh
                connectionsSb.AppendLine("\t;Geometry::, Model::" + mesh.name);
                connectionsSb.AppendLine("\tC: \"OO\"," + geometryId + "," + modelId);
                connectionsSb.AppendLine();

                // Add the connection of all the materials in order of submesh
                var meshRenderer = gameObj.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    var allMaterialsInThisMesh = meshRenderer.sharedMaterials;

                    foreach (var mat in allMaterialsInThisMesh)
                    {
                        if (mat == null)
                        {
                            Debug.LogError("ERROR: the game object " + gameObj.name + " has an empty material on it. This will export problematic files. Please fix and reexport");
                            continue;
                        }
                        var referenceId = Mathf.Abs(mat.GetInstanceID());
                        connectionsSb.AppendLine("\t;Material::" + mat.name + ", Model::" + mesh.name);
                        connectionsSb.AppendLine("\tC: \"OO\"," + referenceId + "," + modelId);
                        connectionsSb.AppendLine();
                    }
                }

            }

            // Recursively add all the other objects to the string that has been built.
            for (var i = 0; i < gameObj.transform.childCount; i++)
            {
                var childObject = gameObj.transform.GetChild(i).gameObject;

                GetMeshToString(childObject, objectsSb, connectionsSb, modelId);
            }
        }
    }
}
