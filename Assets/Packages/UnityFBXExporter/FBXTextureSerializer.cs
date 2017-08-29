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
using System;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UnityFBXExporter
{
    public static class FBXTextureSerializer
    {
        /// <summary>
        /// Serializes textures to FBX format.
        /// </summary>
        /// <param name="gameObj">Parent GameObject being exported.</param>
        /// <param name="newPath">The path to export to.</param>
        /// <param name="copyTextures"></param>
        /// <param name="objects"></param>
        /// <param name="connections">The string to connect this to the  material.</param>
        /// <param name="material"></param>
        /// <param name="materialName"></param>
        public static void SerializedTextures(GameObject gameObj, string newPath, Material material, string materialName, bool copyTextures, out string objects, out string connections)
        {
            // TODO: FBX import currently only supports Diffuse Color and Normal Map
            // Because it is undocumented, there is no way to easily find out what other textures
            // can be attached to an FBX file so it is imported into the PBR shaders at the same time.
            // Also NOTE, Unity 5.1.2 will import FBX files with legacy shaders. This is fix done
            // in at least 5.3.4.

            var objectsSb = new StringBuilder();
            var connectionsSb = new StringBuilder();

            var materialId = Mathf.Abs(material.GetInstanceID());

            var mainTexture = material.GetTexture("_MainTex");

            string newObjects;
            string newConnections;

            // Serializes the Main Texture, one of two textures that can be stored in FBX's system
            if (mainTexture != null)
            {
                SerializeOneTexture(gameObj, newPath, material, materialName, materialId, copyTextures, "_MainTex", "DiffuseColor", out newObjects, out newConnections);
                objectsSb.AppendLine(newObjects);
                connectionsSb.AppendLine(newConnections);
            }

            if (SerializeOneTexture(gameObj, newPath, material, materialName, materialId, copyTextures, "_BumpMap", "NormalMap", out newObjects, out newConnections))
            {
                objectsSb.AppendLine(newObjects);
                connectionsSb.AppendLine(newConnections);
            }

            connections = connectionsSb.ToString();
            objects = objectsSb.ToString();
        }

        public static bool SerializeOneTexture(GameObject gameObj,
            string newPath,
            Material material,
            string materialName,
            int materialId,
            bool copyTextures,
            string unityExtension,
            string textureType,
            out string objects,
            out string connections)
        {
            var objectsSb = new StringBuilder();
            var connectionsSb = new StringBuilder();

            var texture = material.GetTexture(unityExtension);

            if (texture == null)
            {
                objects = "";
                connections = "";
                return false;
            }

#if UNITY_EDITOR
            var originalAssetPath = AssetDatabase.GetAssetPath(texture);
#else
            Debug.LogError("Unity FBX Exporter can not serialize textures at runtime (yet). Look in FBXUnityMaterialGetter around line 250ish. Fix it and contribute to the project!");
            objects = "";
            connections = "";
            return false;
#endif
            var fullDataFolderPath = Application.dataPath;
            var textureFilePathFullName = originalAssetPath;
            var textureName = Path.GetFileNameWithoutExtension(originalAssetPath);
            var textureExtension = Path.GetExtension(originalAssetPath);

            // If we are copying the textures over, we update the relative positions
            if (copyTextures)
            {
                var indexOfAssetsFolder = fullDataFolderPath.LastIndexOf("/Assets", StringComparison.Ordinal);
                fullDataFolderPath = fullDataFolderPath.Remove(indexOfAssetsFolder, fullDataFolderPath.Length - indexOfAssetsFolder);

                var newPathFolder = newPath.Remove(newPath.LastIndexOf('/') + 1, newPath.Length - newPath.LastIndexOf('/') - 1);
                textureName = gameObj.name + "_" + material.name + unityExtension;

                textureFilePathFullName = fullDataFolderPath + "/" + newPathFolder + textureName + textureExtension;
            }

            var textureReference = FBXExporter.GetRandomFBXId();

            // TODO - test out different reference names to get one that doesn't load a _MainTex when importing.

            objectsSb.AppendLine("\tTexture: " + textureReference + ", \"Texture::" + materialName + "\", \"\" {");
            objectsSb.AppendLine("\t\tType: \"TextureVideoClip\"");
            objectsSb.AppendLine("\t\tVersion: 202");
            objectsSb.AppendLine("\t\tTextureName: \"Texture::" + materialName + "\"");
            objectsSb.AppendLine("\t\tProperties70:  {");
            objectsSb.AppendLine("\t\t\tP: \"CurrentTextureBlendMode\", \"enum\", \"\", \"\",0");
            objectsSb.AppendLine("\t\t\tP: \"UVSet\", \"KString\", \"\", \"\", \"map1\"");
            objectsSb.AppendLine("\t\t\tP: \"UseMaterial\", \"bool\", \"\", \"\",1");
            objectsSb.AppendLine("\t\t}");
            objectsSb.AppendLine("\t\tMedia: \"Video::" + materialName + "\"");

            // Sets the absolute path for the copied texture
            objectsSb.Append("\t\tFileName: \"");
            objectsSb.Append(textureFilePathFullName);
            objectsSb.AppendLine("\"");

            // Sets the relative path for the copied texture
            // TODO: If we don't copy the textures to a relative path, we must find a relative path to write down here
            if (copyTextures)
            {
                objectsSb.AppendLine("\t\tRelativeFilename: \"/Textures/" + textureName + textureExtension + "\"");
            }

            objectsSb.AppendLine("\t\tModelUVTranslation: 0,0"); // TODO: Figure out how to get the UV translation into here
            objectsSb.AppendLine("\t\tModelUVScaling: 1,1"); // TODO: Figure out how to get the UV scaling into here
            objectsSb.AppendLine("\t\tTexture_Alpha_Source: \"None\""); // TODO: Add alpha source here if the file is a cutout.
            objectsSb.AppendLine("\t\tCropping: 0,0,0,0");
            objectsSb.AppendLine("\t}");

            connectionsSb.AppendLine("\t;Texture::" + textureName + ", Material::" + materialName + "\"");
            connectionsSb.AppendLine("\tC: \"OP\"," + textureReference + "," + materialId + ", \"" + textureType + "\"");

            connectionsSb.AppendLine();

            objects = objectsSb.ToString();
            connections = connectionsSb.ToString();

            return true;
        }
    }
}
