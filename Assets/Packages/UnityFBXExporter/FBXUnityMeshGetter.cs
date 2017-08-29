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
    public class FBXUnityMeshGetter
    {

        /// <summary>
        /// Gets all the meshes and outputs to a string (even grabbing the child of each gameObject)
        /// </summary>
        /// <returns>The mesh to string.</returns>
        /// <param name="gameObj">GameObject Parent.</param>
        /// <param name="materials">Every Material in the parent that can be accessed.</param>
        /// <param name="objects">The StringBuidler to create objects for the FBX file.</param>
        /// <param name="connections">The StringBuidler to create connections for the FBX file.</param>
        /// <param name="parentObject">Parent object, if left null this is the top parent.</param>
        /// <param name="parentModelId">Parent model id, 0 if top parent.</param>
        public static long GetMeshToString(GameObject gameObj,
                                           Material[] materials,
                                           ref StringBuilder objects,
                                           ref StringBuilder connections,
                                           GameObject parentObject = null,
                                           long parentModelId = 0)
        {
            var tempObjectSb = new StringBuilder();
            var tempConnectionsSb = new StringBuilder();

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
                tempConnectionsSb.AppendLine("\t;Model::" + meshName + ", Model::RootNode");
            }
            else
            {
                tempConnectionsSb.AppendLine("\t;Model::" + meshName + ", Model::USING PARENT");
            }
            tempConnectionsSb.AppendLine("\tC: \"OO\"," + modelId + "," + parentModelId);
            tempConnectionsSb.AppendLine();
            tempObjectSb.AppendLine("\tModel: " + modelId + ", \"Model::" + gameObj.name + "\", \"" + isMesh + "\" {");
            tempObjectSb.AppendLine("\t\tVersion: 232");
            tempObjectSb.AppendLine("\t\tProperties70:  {");
            tempObjectSb.AppendLine("\t\t\tP: \"RotationOrder\", \"enum\", \"\", \"\",4");
            tempObjectSb.AppendLine("\t\t\tP: \"RotationActive\", \"bool\", \"\", \"\",1");
            tempObjectSb.AppendLine("\t\t\tP: \"InheritType\", \"enum\", \"\", \"\",1");
            tempObjectSb.AppendLine("\t\t\tP: \"ScalingMax\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            tempObjectSb.AppendLine("\t\t\tP: \"DefaultAttributeIndex\", \"int\", \"Integer\", \"\",0");
            // ===== Local Translation Offset =========
            var position = gameObj.transform.localPosition;

            tempObjectSb.Append("\t\t\tP: \"Lcl Translation\", \"Lcl Translation\", \"\", \"A+\",");

            // Append the X Y Z coords to the system
            tempObjectSb.AppendFormat("{0},{1},{2}", position.x * -1, position.y, position.z);
            tempObjectSb.AppendLine();

            // Rotates the object correctly from Unity space
            var localRotation = gameObj.transform.localEulerAngles;
            tempObjectSb.AppendFormat("\t\t\tP: \"Lcl Rotation\", \"Lcl Rotation\", \"\", \"A+\",{0},{1},{2}", localRotation.x, localRotation.y * -1, -1 * localRotation.z);
            tempObjectSb.AppendLine();

            // Adds the local scale of this object
            var localScale = gameObj.transform.localScale;
            tempObjectSb.AppendFormat("\t\t\tP: \"Lcl Scaling\", \"Lcl Scaling\", \"\", \"A\",{0},{1},{2}", localScale.x, localScale.y, localScale.z);
            tempObjectSb.AppendLine();

            tempObjectSb.AppendLine("\t\t\tP: \"currentUVSet\", \"KString\", \"\", \"U\", \"map1\"");
            tempObjectSb.AppendLine("\t\t}");
            tempObjectSb.AppendLine("\t\tShading: T");
            tempObjectSb.AppendLine("\t\tCulling: \"CullingOff\"");
            tempObjectSb.AppendLine("\t}");


            // Adds in geometry if it exists, if it it does not exist, this is a empty gameObject file and skips over this
            if (filter != null)
            {
                var mesh = filter.sharedMesh;

                // =================================
                //         General Geometry Info
                // =================================
                // Generate the geometry information for the mesh created

                tempObjectSb.AppendLine("\tGeometry: " + geometryId + ", \"Geometry::\", \"Mesh\" {");

                // ===== WRITE THE VERTICIES =====
                var verticies = mesh.vertices;
                var vertCount = mesh.vertexCount * 3; // <= because the list of points is just a list of comma seperated values, we need to multiply by three

                tempObjectSb.AppendLine("\t\tVertices: *" + vertCount + " {");
                tempObjectSb.Append("\t\t\ta: ");
                for (var i = 0; i < verticies.Length; i++)
                {
                    if (i > 0)
                    {
                        tempObjectSb.Append(",");
                    }

                    // Points in the verticies. We also reverse the x value because Unity has a reverse X coordinate
                    tempObjectSb.AppendFormat("{0},{1},{2}", verticies[i].x * -1, verticies[i].y, verticies[i].z);
                }

                tempObjectSb.AppendLine();
                tempObjectSb.AppendLine("\t\t} ");

                // ======= WRITE THE TRIANGLES ========
                var triangleCount = mesh.triangles.Length;
                var triangles = mesh.triangles;

                tempObjectSb.AppendLine("\t\tPolygonVertexIndex: *" + triangleCount + " {");

                // Write triangle indexes
                tempObjectSb.Append("\t\t\ta: ");
                for (var i = 0; i < triangleCount; i += 3)
                {
                    if (i > 0)
                    {
                        tempObjectSb.Append(",");
                    }

                    // To get the correct normals, must rewind the triangles since we flipped the x direction
                    tempObjectSb.AppendFormat("{0},{1},{2}",
                                              triangles[i],
                                              triangles[i + 2],
                                              (triangles[i + 1] * -1) - 1); // <= Tells the poly is ended

                }

                tempObjectSb.AppendLine();

                tempObjectSb.AppendLine("\t\t} ");
                tempObjectSb.AppendLine("\t\tGeometryVersion: 124");
                tempObjectSb.AppendLine("\t\tLayerElementNormal: 0 {");
                tempObjectSb.AppendLine("\t\t\tVersion: 101");
                tempObjectSb.AppendLine("\t\t\tName: \"\"");
                tempObjectSb.AppendLine("\t\t\tMappingInformationType: \"ByPolygonVertex\"");
                tempObjectSb.AppendLine("\t\t\tReferenceInformationType: \"Direct\"");

                // ===== WRITE THE NORMALS ==========
                var normals = mesh.normals;

                tempObjectSb.AppendLine("\t\t\tNormals: *" + (triangleCount * 3) + " {");
                tempObjectSb.Append("\t\t\t\ta: ");

                for (var i = 0; i < triangleCount; i += 3)
                {
                    if (i > 0)
                    {
                        tempObjectSb.Append(",");
                    }

                    // To get the correct normals, must rewind the normal triangles like the triangles above since x was flipped
                    var newNormal = normals[triangles[i]];

                    tempObjectSb.AppendFormat("{0},{1},{2},",
                                             newNormal.x * -1, // Switch normal as is tradition
                                             newNormal.y,
                                             newNormal.z);

                    newNormal = normals[triangles[i + 2]];

                    tempObjectSb.AppendFormat("{0},{1},{2},",
                                              newNormal.x * -1, // Switch normal as is tradition
                                              newNormal.y,
                                              newNormal.z);

                    newNormal = normals[triangles[i + 1]];

                    tempObjectSb.AppendFormat("{0},{1},{2}",
                                              newNormal.x * -1, // Switch normal as is tradition
                                              newNormal.y,
                                              newNormal.z);
                }

                tempObjectSb.AppendLine();
                tempObjectSb.AppendLine("\t\t\t}");
                tempObjectSb.AppendLine("\t\t}");

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

                    tempObjectSb.AppendLine("\t\tLayerElementColor: 0 {");
                    tempObjectSb.AppendLine("\t\t\tVersion: 101");
                    tempObjectSb.AppendLine("\t\t\tName: \"Col\"");
                    tempObjectSb.AppendLine("\t\t\tMappingInformationType: \"ByPolygonVertex\"");
                    tempObjectSb.AppendLine("\t\t\tReferenceInformationType: \"IndexToDirect\"");
                    tempObjectSb.AppendLine("\t\t\tColors: *" + colorTable.Count * 4 + " {");
                    tempObjectSb.Append("\t\t\t\ta: ");

                    var first = true;
                    foreach (var color in colorTable)
                    {
                        if (!first)
                        {
                            tempObjectSb.Append(",");
                        }

                        tempObjectSb.AppendFormat("{0},{1},{2},{3}", color.Key.r, color.Key.g, color.Key.b, color.Key.a);
                        first = false;
                    }
                    tempObjectSb.AppendLine();

                    tempObjectSb.AppendLine("\t\t\t\t}");

                    // Color index
                    tempObjectSb.AppendLine("\t\t\tColorIndex: *" + triangles.Length + " {");
                    tempObjectSb.Append("\t\t\t\ta: ");

                    for (var i = 0; i < triangles.Length; i += 3)
                    {
                        if (i > 0)
                        {
                            tempObjectSb.Append(",");
                        }

                        // Triangles need to be fliped for the x flip
                        var index1 = triangles[i];
                        var index2 = triangles[i + 2];
                        var index3 = triangles[i + 1];

                        // Find the color index related to that vertice index
                        index1 = colorTable[colors[index1]];
                        index2 = colorTable[colors[index2]];
                        index3 = colorTable[colors[index3]];

                        tempObjectSb.AppendFormat("{0},{1},{2}", index1, index2, index3);
                    }

                    tempObjectSb.AppendLine();

                    tempObjectSb.AppendLine("\t\t\t}");
                    tempObjectSb.AppendLine("\t\t}");
                }
                else
                {
                    Debug.LogWarning("Mesh contains " + mesh.vertices.Length + " vertices for " + mesh.colors.Length + " colors. Skip color export");
                }


                // ================ UV CREATION =========================

                // -- UV 1 Creation
                var uvLength = mesh.uv.Length;
                var uvs = mesh.uv;

                tempObjectSb.AppendLine("\t\tLayerElementUV: 0 {"); // the Zero here is for the first UV map
                tempObjectSb.AppendLine("\t\t\tVersion: 101");
                tempObjectSb.AppendLine("\t\t\tName: \"map1\"");
                tempObjectSb.AppendLine("\t\t\tMappingInformationType: \"ByPolygonVertex\"");
                tempObjectSb.AppendLine("\t\t\tReferenceInformationType: \"IndexToDirect\"");
                tempObjectSb.AppendLine("\t\t\tUV: *" + uvLength * 2 + " {");
                tempObjectSb.Append("\t\t\t\ta: ");

                for (var i = 0; i < uvLength; i++)
                {
                    if (i > 0)
                    {
                        tempObjectSb.Append(",");
                    }

                    tempObjectSb.AppendFormat("{0},{1}", uvs[i].x, uvs[i].y);

                }
                tempObjectSb.AppendLine();

                tempObjectSb.AppendLine("\t\t\t\t}");

                // UV tile index coords
                tempObjectSb.AppendLine("\t\t\tUVIndex: *" + triangleCount + " {");
                tempObjectSb.Append("\t\t\t\ta: ");

                for (var i = 0; i < triangleCount; i += 3)
                {
                    if (i > 0)
                    {
                        tempObjectSb.Append(",");
                    }

                    // Triangles need to be fliped for the x flip
                    var index1 = triangles[i];
                    var index2 = triangles[i + 2];
                    var index3 = triangles[i + 1];

                    tempObjectSb.AppendFormat("{0},{1},{2}", index1, index2, index3);
                }

                tempObjectSb.AppendLine();

                tempObjectSb.AppendLine("\t\t\t}");
                tempObjectSb.AppendLine("\t\t}");

                // -- UV 2 Creation
                // TODO: Add UV2 Creation here

                // -- Smoothing
                // TODO: Smoothing doesn't seem to do anything when importing. This maybe should be added. -KBH

                // ============ MATERIALS =============

                tempObjectSb.AppendLine("\t\tLayerElementMaterial: 0 {");
                tempObjectSb.AppendLine("\t\t\tVersion: 101");
                tempObjectSb.AppendLine("\t\t\tName: \"\"");
                tempObjectSb.AppendLine("\t\t\tMappingInformationType: \"ByPolygon\"");
                tempObjectSb.AppendLine("\t\t\tReferenceInformationType: \"IndexToDirect\"");

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

                tempObjectSb.AppendLine("\t\t\tMaterials: *" + totalFaceCount + " {");
                tempObjectSb.Append("\t\t\t\ta: ");
                tempObjectSb.AppendLine(submeshesSb.ToString());
                tempObjectSb.AppendLine("\t\t\t} ");
                tempObjectSb.AppendLine("\t\t}");

                // ============= INFORMS WHAT TYPE OF LATER ELEMENTS ARE IN THIS GEOMETRY =================
                tempObjectSb.AppendLine("\t\tLayer: 0 {");
                tempObjectSb.AppendLine("\t\t\tVersion: 100");
                tempObjectSb.AppendLine("\t\t\tLayerElement:  {");
                tempObjectSb.AppendLine("\t\t\t\tType: \"LayerElementNormal\"");
                tempObjectSb.AppendLine("\t\t\t\tTypedIndex: 0");
                tempObjectSb.AppendLine("\t\t\t}");
                tempObjectSb.AppendLine("\t\t\tLayerElement:  {");
                tempObjectSb.AppendLine("\t\t\t\tType: \"LayerElementMaterial\"");
                tempObjectSb.AppendLine("\t\t\t\tTypedIndex: 0");
                tempObjectSb.AppendLine("\t\t\t}");
                tempObjectSb.AppendLine("\t\t\tLayerElement:  {");
                tempObjectSb.AppendLine("\t\t\t\tType: \"LayerElementTexture\"");
                tempObjectSb.AppendLine("\t\t\t\tTypedIndex: 0");
                tempObjectSb.AppendLine("\t\t\t}");
                if (containsColors)
                {
                    tempObjectSb.AppendLine("\t\t\tLayerElement:  {");
                    tempObjectSb.AppendLine("\t\t\t\tType: \"LayerElementColor\"");
                    tempObjectSb.AppendLine("\t\t\t\tTypedIndex: 0");
                    tempObjectSb.AppendLine("\t\t\t}");
                }
                tempObjectSb.AppendLine("\t\t\tLayerElement:  {");
                tempObjectSb.AppendLine("\t\t\t\tType: \"LayerElementUV\"");
                tempObjectSb.AppendLine("\t\t\t\tTypedIndex: 0");
                tempObjectSb.AppendLine("\t\t\t}");
                // TODO: Here we would add UV layer 1 for ambient occlusion UV file
                //			tempObjectSb.AppendLine("\t\t\tLayerElement:  {");
                //			tempObjectSb.AppendLine("\t\t\t\tType: \"LayerElementUV\"");
                //			tempObjectSb.AppendLine("\t\t\t\tTypedIndex: 1");
                //			tempObjectSb.AppendLine("\t\t\t}");
                tempObjectSb.AppendLine("\t\t}");
                tempObjectSb.AppendLine("\t}");

                // Add the connection for the model to the geometry so it is attached the right mesh
                tempConnectionsSb.AppendLine("\t;Geometry::, Model::" + mesh.name);
                tempConnectionsSb.AppendLine("\tC: \"OO\"," + geometryId + "," + modelId);
                tempConnectionsSb.AppendLine();

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
                        tempConnectionsSb.AppendLine("\t;Material::" + mat.name + ", Model::" + mesh.name);
                        tempConnectionsSb.AppendLine("\tC: \"OO\"," + referenceId + "," + modelId);
                        tempConnectionsSb.AppendLine();
                    }
                }

            }

            // Recursively add all the other objects to the string that has been built.
            for (var i = 0; i < gameObj.transform.childCount; i++)
            {
                var childObject = gameObj.transform.GetChild(i).gameObject;

                GetMeshToString(childObject, materials, ref tempObjectSb, ref tempConnectionsSb, gameObj, modelId);
            }

            objects.Append(tempObjectSb);
            connections.Append(tempConnectionsSb);

            return modelId;
        }
    }
}
