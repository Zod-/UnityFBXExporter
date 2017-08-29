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
using UnityEngine;
using System.Text;

namespace UnityFBXExporter
{
    public static class FBXMeshSerializer
    {
        /// <summary>
        /// Gets all the meshes and outputs to a string (even grabbing the child of each gameObject)
        /// </summary>
        /// <returns>The mesh to string.</returns>
        /// <param name="gameObject">GameObject Parent.</param>
        /// <param name="objectsSb">The StringBuilder to create objects for the FBX file.</param>
        /// <param name="connectionsSb">The StringBuilder to create connections for the FBX file.</param>
        /// <param name="geometryIds">Cache of previously defined geometries</param>
        /// <param name="parentModelId">Parent model id, 0 if top parent.</param>
        public static void Serialize(GameObject gameObject, StringBuilder objectsSb, StringBuilder connectionsSb, Dictionary<string, long> geometryIds, long parentModelId = 0)
        {
            EnsureMeshFilterInSkinnedRenderer(gameObject);
            SerializeCore(gameObject, parentModelId, objectsSb, connectionsSb, geometryIds);
        }

        private static void SerializeCore(GameObject gameObject, long parentModelId, StringBuilder objectsSb, StringBuilder connectionsSb, Dictionary<string, long> geometryIds)
        {
            var modelId = FBXExporter.GetRandomFBXId();
            var mesh = GetMesh(gameObject);

            SerializeObject(gameObject, mesh, modelId, parentModelId, objectsSb, connectionsSb);
            if (mesh)
            {
                FBXMeshPropertySerializer.Serialize(mesh, gameObject, modelId, objectsSb, connectionsSb, geometryIds);
            }

            // Recursively add all the other objects to the string that has been built.
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var childObject = gameObject.transform.GetChild(i).gameObject;
                SerializeCore(childObject, modelId, objectsSb, connectionsSb, geometryIds);
            }
        }

        private static void SerializeObject(GameObject gameObject, Mesh mesh, long modelId, long parentModelId, StringBuilder objectsSb, StringBuilder connectionsSb)
        {
            Header(gameObject, mesh, parentModelId, modelId, objectsSb, connectionsSb);
            LocalTransformOffset(gameObject, objectsSb);
            Footer(objectsSb);
        }

        private static Mesh GetMesh(GameObject gameObject)
        {
            var filter = gameObject.GetComponent<MeshFilter>();
            return filter == null ? null : filter.sharedMesh;
        }

        private static void Header(GameObject gameObject, Mesh mesh, long parentModelId, long modelId, StringBuilder objectsSb, StringBuilder connectionsSb)
        {
            var meshName = mesh == null ? gameObject.name : mesh.name;
            var modelType = mesh == null ? "Null" : "Mesh";
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
            objectsSb.AppendLine("\tModel: " + modelId + ", \"Model::" + gameObject.name + "\", \"" + modelType + "\" {");
            objectsSb.AppendLine("\t\tVersion: 232");
            objectsSb.AppendLine("\t\tProperties70:  {");
            objectsSb.AppendLine("\t\t\tP: \"RotationOrder\", \"enum\", \"\", \"\",4");
            objectsSb.AppendLine("\t\t\tP: \"RotationActive\", \"bool\", \"\", \"\",1");
            objectsSb.AppendLine("\t\t\tP: \"InheritType\", \"enum\", \"\", \"\",1");
            objectsSb.AppendLine("\t\t\tP: \"ScalingMax\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            objectsSb.AppendLine("\t\t\tP: \"DefaultAttributeIndex\", \"int\", \"Integer\", \"\",0");
        }

        /// <summary>
        /// Ensures that skinned mesh renders have a meshFilter on all gameObjects in the hierarchy
        /// </summary>
        /// <param name="gameObject">Root gameObject where to start traversing</param>
        private static void EnsureMeshFilterInSkinnedRenderer(GameObject gameObject)
        {
            var meshFilterRender = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var skinnedMeshRenderer in meshFilterRender)
            {
                if (skinnedMeshRenderer.GetComponent<MeshFilter>() != null) { continue; }
                skinnedMeshRenderer.gameObject.AddComponent<MeshFilter>();
                skinnedMeshRenderer.GetComponent<MeshFilter>().sharedMesh = Object.Instantiate(skinnedMeshRenderer.sharedMesh);
            }
        }

        private static void LocalTransformOffset(GameObject gameObject, StringBuilder objectsSb)
        {
            // ===== Local Translation Offset =========
            var position = gameObject.transform.localPosition;

            objectsSb.Append("\t\t\tP: \"Lcl Translation\", \"Lcl Translation\", \"\", \"A+\",");

            // Append the X Y Z coords to the system
            objectsSb.AppendFormat("{0},{1},{2}", position.x * -1, position.y, position.z);
            objectsSb.AppendLine();

            // Rotates the object correctly from Unity space
            var localRotation = gameObject.transform.localEulerAngles;
            objectsSb.AppendFormat("\t\t\tP: \"Lcl Rotation\", \"Lcl Rotation\", \"\", \"A+\",{0},{1},{2}", localRotation.x, localRotation.y * -1, -1 * localRotation.z);
            objectsSb.AppendLine();

            // Adds the local scale of this object
            var localScale = gameObject.transform.localScale;
            objectsSb.AppendFormat("\t\t\tP: \"Lcl Scaling\", \"Lcl Scaling\", \"\", \"A\",{0},{1},{2}", localScale.x, localScale.y, localScale.z);
            objectsSb.AppendLine();
        }

        private static void Footer(StringBuilder objectsSb)
        {
            objectsSb.AppendLine("\t\t\tP: \"currentUVSet\", \"KString\", \"\", \"U\", \"map1\"");
            objectsSb.AppendLine("\t\t}");
            objectsSb.AppendLine("\t\tShading: T");
            objectsSb.AppendLine("\t\tCulling: \"CullingOff\"");
            objectsSb.AppendLine("\t}");
        }
    }
}
