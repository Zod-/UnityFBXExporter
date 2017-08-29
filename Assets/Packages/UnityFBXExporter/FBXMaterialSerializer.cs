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
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityFBXExporter
{
    public static class FBXMaterialSerializer
    {
        /// <summary>
        /// Finds all materials in a gameObject and writes them to a string that can be read by the FBX writer
        /// </summary>
        /// <param name="gameObj">Parent GameObject being exported.</param>
        /// <param name="newPath">The path to export to.</param>
        /// <param name="copyTextures">Should textures be copied</param>
        /// <param name="materials">Materials which were written to this fbx file.</param>
        /// <param name="matObjects">The material objects to write to the file.</param>
        /// <param name="connections">The connections to write to the file.</param>
        /// <param name="copyMaterials">Should materials be copied</param>
        public static void GetAllMaterialsToString(GameObject gameObj, string newPath, bool copyMaterials, bool copyTextures, out Material[] materials, out string matObjects, out string connections)
        {
            var tempObjectSb = new StringBuilder();
            var tempConnectionsSb = new StringBuilder();

            // Need to get all unique materials for the subMesh here and then write them in
            //@cartzhang modify.As meshRenderer and skinnedRenderer is same level in inherit relation shape.
            // if not check,skinned render ,may lost some materials.
            // Gets all the unique materials within this GameObject Hierarchy
            var uniqueMaterials = GetUniqueMaterials(gameObj);

            foreach (var mat in uniqueMaterials)
            {
                var materialName = GetMaterialName(gameObj, copyMaterials, mat);
                SerializeMaterial(mat, materialName, ref tempObjectSb);

                string textureObjects;
                string textureConnections;

                FBXTextureSerializer.SerializedTextures(gameObj, newPath, mat, materialName, copyTextures, out textureObjects, out textureConnections);

                tempObjectSb.Append(textureObjects);
                tempConnectionsSb.Append(textureConnections);
            }

            materials = uniqueMaterials.ToArray<Material>();

            matObjects = tempObjectSb.ToString();
            connections = tempConnectionsSb.ToString();
        }

        private static string GetMaterialName(GameObject gameObj, bool copyMaterials, Material mat)
        {
            var materialName = mat.name;
            // We rename the material if it is being copied
            if (copyMaterials)
            {
                materialName = gameObj.name + "_" + mat.name;
            }

            return materialName;
        }

        private static void SerializeMaterial(Material mat, string materialName, ref StringBuilder tempObjectSb)
        {
            var referenceId = Mathf.Abs(mat.GetInstanceID());

            tempObjectSb.AppendLine();
            tempObjectSb.AppendLine("\tMaterial: " + referenceId + ", \"Material::" + materialName + "\", \"\" {");
            tempObjectSb.AppendLine("\t\tVersion: 102");
            tempObjectSb.AppendLine("\t\tShadingModel: \"phong\"");
            tempObjectSb.AppendLine("\t\tMultiLayer: 0");
            tempObjectSb.AppendLine("\t\tProperties70:  {");
            tempObjectSb.AppendFormat("\t\t\tP: \"Diffuse\", \"Vector3D\", \"Vector\", \"\",{0},{1},{2}", mat.color.r, mat.color.g, mat.color.b);
            tempObjectSb.AppendLine();
            tempObjectSb.AppendFormat("\t\t\tP: \"DiffuseColor\", \"Color\", \"\", \"A\",{0},{1},{2}", mat.color.r, mat.color.g, mat.color.b);
            tempObjectSb.AppendLine();

            // TODO: Figure out if this property can be written to the FBX file
            //			if(mat.HasProperty("_MetallicGlossMap"))
            //			{
            //				Debug.Log("has metallic gloss map");
            //				Color color = mat.GetColor("_Color");
            //				tempObjectSb.AppendFormat("\t\t\tP: \"Specular\", \"Vector3D\", \"Vector\", \"\",{0},{1},{2}", color.r, color.g, color.r);
            //				tempObjectSb.AppendLine();
            //				tempObjectSb.AppendFormat("\t\t\tP: \"SpecularColor\", \"ColorRGB\", \"Color\", \" \",{0},{1},{2}", color.r, color.g, color.b);
            //				tempObjectSb.AppendLine();
            //			}

            if (mat.HasProperty("_SpecColor"))
            {
                var color = mat.GetColor("_SpecColor");
                tempObjectSb.AppendFormat("\t\t\tP: \"Specular\", \"Vector3D\", \"Vector\", \"\",{0},{1},{2}", color.r, color.g, color.r);
                tempObjectSb.AppendLine();
                tempObjectSb.AppendFormat("\t\t\tP: \"SpecularColor\", \"ColorRGB\", \"Color\", \" \",{0},{1},{2}", color.r, color.g, color.b);
                tempObjectSb.AppendLine();
            }

            if (mat.HasProperty("_Mode"))
            {
                Color color;

                switch ((int)mat.GetFloat("_Mode"))
                {
                    case 0: // Map is opaque

                        break;

                    case 1: // Map is a cutout
                        //  TODO: Add option if it is a cutout
                        break;

                    case 2: // Map is a fade
                        color = mat.GetColor("_Color");

                        tempObjectSb.AppendFormat("\t\t\tP: \"TransparentColor\", \"Color\", \"\", \"A\",{0},{1},{2}", color.r, color.g, color.b);
                        tempObjectSb.AppendLine();
                        tempObjectSb.AppendFormat("\t\t\tP: \"Opacity\", \"double\", \"Number\", \"\",{0}", color.a);
                        tempObjectSb.AppendLine();
                        break;

                    case 3: // Map is transparent
                        color = mat.GetColor("_Color");

                        tempObjectSb.AppendFormat("\t\t\tP: \"TransparentColor\", \"Color\", \"\", \"A\",{0},{1},{2}", color.r, color.g, color.b);
                        tempObjectSb.AppendLine();
                        tempObjectSb.AppendFormat("\t\t\tP: \"Opacity\", \"double\", \"Number\", \"\",{0}", color.a);
                        tempObjectSb.AppendLine();
                        break;
                }
            }

            // NOTE: Unity doesn't currently import this information (I think) from an FBX file.
            if (mat.HasProperty("_EmissionColor"))
            {
                var color = mat.GetColor("_EmissionColor");

                tempObjectSb.AppendFormat("\t\t\tP: \"Emissive\", \"Vector3D\", \"Vector\", \"\",{0},{1},{2}", color.r, color.g, color.b);
                tempObjectSb.AppendLine();

                var averageColor = (color.r + color.g + color.b) / 3f;

                tempObjectSb.AppendFormat("\t\t\tP: \"EmissiveFactor\", \"Number\", \"\", \"A\",{0}", averageColor);
                tempObjectSb.AppendLine();
            }

            // TODO: Add these to the file based on their relation to the PBR files
            //				tempObjectSb.AppendLine("\t\t\tP: \"AmbientColor\", \"Color\", \"\", \"A\",0,0,0");
            //				tempObjectSb.AppendLine("\t\t\tP: \"ShininessExponent\", \"Number\", \"\", \"A\",6.31179285049438");
            //				tempObjectSb.AppendLine("\t\t\tP: \"Ambient\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            //				tempObjectSb.AppendLine("\t\t\tP: \"Shininess\", \"double\", \"Number\", \"\",6.31179285049438");
            //				tempObjectSb.AppendLine("\t\t\tP: \"Reflectivity\", \"double\", \"Number\", \"\",0");

            tempObjectSb.AppendLine("\t\t}");
            tempObjectSb.AppendLine("\t}");
        }

        /// <summary>
        /// Returns unique materials for all the renderers in the gameObject
        /// </summary>
        /// <param name="gameObj">Root gameObject to search renderers</param>
        /// <returns>List of unique materials</returns>
        public static List<Material> GetUniqueMaterials(GameObject gameObj)
        {
            return gameObj.GetComponentsInChildren<Renderer>().Select(r => r.sharedMaterial).Where(m => m).Distinct().ToList();
        }
    }
}
