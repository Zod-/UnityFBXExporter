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
        /// <param name="objectSb">The material objects to write to the file.</param>
        /// <param name="connectionsSb">The connections to write to the file.</param>
        /// <param name="copyMaterials">Should materials be copied</param>
        public static void Serialize(GameObject gameObj, string newPath, StringBuilder objectSb, StringBuilder connectionsSb, bool copyMaterials = false, bool copyTextures = false)
        {
            foreach (var mat in GetUniqueMaterials(gameObj))
            {
                var materialName = GetMaterialName(gameObj.name, mat.name, copyMaterials);
                MaterialHeader(mat, materialName, objectSb);
                FBXMaterialPropertySerializer.Serialize(mat, objectSb);
                MaterialFooter(objectSb);
                FBXTextureSerializer.Serialize(newPath, mat, materialName, objectSb, connectionsSb, copyTextures);
            }
        }

        private static string GetMaterialName(string objectName, string materialName, bool copyMaterials = false)
        {
            // We rename the material if it is being copied
            if (copyMaterials)
            {
                materialName = objectName + "_" + materialName;
            }
            return materialName;
        }

        private static void MaterialHeader(Material mat, string materialName, StringBuilder tempObjectSb)
        {
            var referenceId = Mathf.Abs(mat.GetInstanceID());

            tempObjectSb.AppendLine();
            tempObjectSb.AppendLine(string.Format("\tMaterial: {0}, \"Material::{1}\", \"\" {{", referenceId, materialName));
            tempObjectSb.AppendLine("\t\tVersion: 102");
            tempObjectSb.AppendLine("\t\tShadingModel: \"phong\"");
            tempObjectSb.AppendLine("\t\tMultiLayer: 0");
            tempObjectSb.AppendLine("\t\tProperties70:  {");
            tempObjectSb.AppendFormat("\t\t\tP: \"Diffuse\", \"Vector3D\", \"Vector\", \"\",{0},{1},{2}", mat.color.r, mat.color.g, mat.color.b);
            tempObjectSb.AppendLine();
            tempObjectSb.AppendFormat("\t\t\tP: \"DiffuseColor\", \"Color\", \"\", \"A\",{0},{1},{2}", mat.color.r, mat.color.g, mat.color.b);
            tempObjectSb.AppendLine();
        }

        private static void MaterialFooter(StringBuilder tempObjectSb)
        {
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
        public static IEnumerable<Material> GetUniqueMaterials(GameObject gameObj)
        {
            return gameObj.GetComponentsInChildren<Renderer>().Select(r => r.sharedMaterial).Where(m => m).Distinct();
        }
    }
}
