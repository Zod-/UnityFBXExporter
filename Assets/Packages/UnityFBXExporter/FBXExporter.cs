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
using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using Object = UnityEngine.Object;

namespace UnityFBXExporter
{
    public static class FBXExporter
    {
        public static string MeshToString(GameObject[] gameObjects, string newPath, bool copyMaterials = false, bool copyTextures = false)
        {
            var sb = new StringBuilder();
            FBXFileHeaderDefinition.Serialize(newPath, sb);

            var objectProps = new StringBuilder();
            var objectConnections = new StringBuilder();
            FBXObjectSerializer.Serialize(gameObjects, newPath, copyMaterials, copyTextures, objectProps, objectConnections);

            sb.Append(objectProps);
            sb.Append(objectConnections);

            return sb.ToString();
        }

        public static bool ExportGameObjToFBX(GameObject[] gameObjects, string newPath, bool copyMaterials = false, bool copyTextures = false)
        {
            // Check to see if the extension is right
            if (newPath.Remove(0, newPath.LastIndexOf('.')) != ".fbx")
            {
                Debug.LogError("The end of the path wasn't \".fbx\"");
                return false;
            }

            if (copyMaterials)
            {
                foreach (var gameObject in gameObjects)
                {
                    CopyComplexMaterialsToPath(gameObject, newPath, copyTextures);
                }
            }

            var buildMesh = MeshToString(gameObjects, newPath, copyMaterials, copyTextures);

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }

            File.WriteAllText(newPath, buildMesh);

#if UNITY_EDITOR
            // Import the model properly so it looks for the material instead of by the texture name
            // TODO: By calling refresh, it imports the model with the wrong materials, but we can't find the model to import without
            // refreshing the database. A chicken and the egg issue
            AssetDatabase.Refresh();
            var localPath = newPath.Remove(0, newPath.LastIndexOf("/Assets", StringComparison.Ordinal) + 1);
            var modelImporter = AssetImporter.GetAtPath(localPath) as ModelImporter;
            if (modelImporter != null)
            {
                modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;
#if UNITY_5_1
                modelImporter.normalImportMode = ModelImporterTangentSpaceMode.Import;
#else
                modelImporter.importNormals = ModelImporterNormals.Import;
#endif
                if (copyMaterials == false)
                {
                    modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;
                }

                AssetDatabase.ImportAsset(localPath, ImportAssetOptions.ForceUpdate);
            }
            else
            {
                Debug.Log("Model Importer is null and can't import");
            }

            AssetDatabase.Refresh();
#endif
            return true;
        }

        public static string VersionInformation
        {
            get { return "FBX Unity Export version 1.1.1 (Originally created for the Unity Asset, Building Crafter)"; }
        }

        public static long GetRandomFBXId()
        {
            return BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
        }

        private static void CopyComplexMaterialsToPath(GameObject gameObj, string path, bool copyTextures, string texturesFolder = "/Textures", string materialsFolder = "/Materials")
        {
#if UNITY_EDITOR
            var folderIndex = path.LastIndexOf('/');
            path = path.Remove(folderIndex, path.Length - folderIndex);

            // 1. First create the directories that are needed
            var texturesPath = path + texturesFolder;
            var materialsPath = path + materialsFolder;

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            if (Directory.Exists(materialsPath) == false)
            {
                Directory.CreateDirectory(materialsPath);
            }


            // 2. Copy every distinct Material into the Materials folder
            //@cartzhang modify.As meshrender and skinnedrender is same level in inherit relation shape.
            // if not check,skinned render ,may lost some materials.
            var meshRenderers = gameObj.GetComponentsInChildren<Renderer>();
            var everyMaterial = new List<Material>();
            foreach (var renderer in meshRenderers)
            {
                everyMaterial.AddRange(renderer.sharedMaterials);
            }

            var everyDistinctMaterial = everyMaterial.Distinct().ToArray();
            everyDistinctMaterial = everyDistinctMaterial.OrderBy(o => o.name).ToArray();

            // Log warning if there are multiple assets with the same name
            for (var i = 0; i < everyDistinctMaterial.Length; i++)
            {
                for (var n = 0; n < everyDistinctMaterial.Length; n++)
                {
                    if (i == n) { continue; }

                    if (everyDistinctMaterial[i].name != everyDistinctMaterial[n].name) { continue; }
                    Debug.LogErrorFormat("Two distinct materials {0} and {1} have the same name, this will not work with the FBX Exporter", everyDistinctMaterial[i], everyDistinctMaterial[n]);
                    return;
                }
            }

            var everyMaterialName = new List<string>();
            // Structure of materials naming, is used when packaging up the package
            // PARENTNAME_ORIGINALMATNAME.mat
            foreach (var mat in everyDistinctMaterial)
            {
                var newName = gameObj.name + "_" + mat.name;
                var fullPath = materialsPath + "/" + newName + ".mat";

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                if (CopyAndRenameAsset(mat, newName, materialsPath))
                {
                    everyMaterialName.Add(newName);
                }
            }

            // 3. Go through newly moved materials and copy every texture and update the material
            AssetDatabase.Refresh();

            var allNewMaterials = new List<Material>();

            foreach (var name in everyMaterialName)
            {
                var assetPath = materialsPath;
                if (assetPath[assetPath.Length - 1] != '/')
                {
                    assetPath += "/";
                }

                assetPath += name + ".mat";

                var sourceMat = (Material)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Material));

                if (sourceMat != null)
                {
                    allNewMaterials.Add(sourceMat);
                }
            }

            // Get all the textures from the mesh renderer

            if (copyTextures)
            {
                if (Directory.Exists(texturesPath) == false)
                {
                    Directory.CreateDirectory(texturesPath);
                }

                AssetDatabase.Refresh();

                for (var i = 0; i < allNewMaterials.Count; i++)
                {
                    allNewMaterials[i] = CopyTexturesAndAssignCopiesToMaterial(allNewMaterials[i], texturesPath);
                }
            }

            AssetDatabase.Refresh();
#endif
        }

        private static bool CopyAndRenameAsset(Object obj, string newName, string newFolderPath)
        {
#if UNITY_EDITOR
            var path = newFolderPath;

            if (path[path.Length - 1] != '/')
            {
                path += "/";
            }
            path.Remove(path.Length - 1);

            var assetPath = AssetDatabase.GetAssetPath(obj);
            var extension = Path.GetExtension(assetPath);

            var newFileName = path + newName + extension;

            return !File.Exists(newFileName) && AssetDatabase.CopyAsset(assetPath, newFileName);

#else
            return false;

#endif
        }

        /// <summary>
        /// Strips the full path of a file
        /// </summary>
        /// <returns>The file name.</returns>
        /// <param name="path">Path.</param>
        private static string GetFileName(string path)
        {
            var fileName = path;
            fileName = fileName.Remove(0, fileName.LastIndexOf('/') + 1);

            return fileName;
        }

        private static Material CopyTexturesAndAssignCopiesToMaterial(Material material, string newPath)
        {
            if (material.shader.name == "Standard" || material.shader.name == "Standard (Specular setup)")
            {
                GetTextureUpdateMaterialWithPath(material, "_MainTex", newPath);

                if (material.shader.name == "Standard")
                {
                    GetTextureUpdateMaterialWithPath(material, "_MetallicGlossMap", newPath);
                }

                if (material.shader.name == "Standard (Specular setup)")
                {
                    GetTextureUpdateMaterialWithPath(material, "_SpecGlossMap", newPath);
                }

                GetTextureUpdateMaterialWithPath(material, "_BumpMap", newPath);
                GetTextureUpdateMaterialWithPath(material, "_BumpMap", newPath);
                GetTextureUpdateMaterialWithPath(material, "_BumpMap", newPath);
                GetTextureUpdateMaterialWithPath(material, "_BumpMap", newPath);
                GetTextureUpdateMaterialWithPath(material, "_ParallaxMap", newPath);
                GetTextureUpdateMaterialWithPath(material, "_OcclusionMap", newPath);
                GetTextureUpdateMaterialWithPath(material, "_EmissionMap", newPath);
                GetTextureUpdateMaterialWithPath(material, "_DetailMask", newPath);
                GetTextureUpdateMaterialWithPath(material, "_DetailAlbedoMap", newPath);
                GetTextureUpdateMaterialWithPath(material, "_DetailNormalMap", newPath);

            }
            else
            {
                Debug.LogError("WARNING: " + material.name + " is not a physically based shader, may not export to package correctly");
            }

            return material;
        }

        /// <summary>
        /// Copies and renames the texture and assigns it to the material provided.
        /// NAME FORMAT: Material.name + textureShaderName
        /// </summary>
        /// <param name="material">Material.</param>
        /// <param name="textureShaderName">Texture shader name.</param>
        /// <param name="newPath">New path.</param>
        private static void GetTextureUpdateMaterialWithPath(Material material, string textureShaderName, string newPath)
        {
            var textureInQ = material.GetTexture(textureShaderName);
            if (textureInQ == null) { return; }
            var name = material.name + textureShaderName;

            var newTexture = (Texture)CopyAndRenameAssetReturnObject(textureInQ, name, newPath);
            if (newTexture != null)
            {
                material.SetTexture(textureShaderName, newTexture);
            }
        }

        private static Object CopyAndRenameAssetReturnObject(Object obj, string newName, string newFolderPath)
        {
#if UNITY_EDITOR
            var path = newFolderPath;

            if (path[path.Length - 1] != '/')
            {
                path += "/";
            }
            var testPath = path.Remove(path.Length - 1);

            if (Directory.Exists(testPath) == false)
            {
                Debug.LogError("This folder does not exist " + testPath);
                return null;
            }

            var assetPath = AssetDatabase.GetAssetPath(obj);
            var fileName = GetFileName(assetPath);
            var extension = fileName.Remove(0, fileName.LastIndexOf('.'));

            var newFullPathName = path + newName + extension;

            if (AssetDatabase.CopyAsset(assetPath, newFullPathName) == false) { return null; }

            AssetDatabase.Refresh();

            return AssetDatabase.LoadAssetAtPath(newFullPathName, typeof(Texture));
#else
			return null;
#endif
        }
    }
}
