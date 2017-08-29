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
using System.Text;
using UnityEngine;

namespace UnityFBXExporter
{
    public static class FBXMaterialPropertySerializer
    {
        public static void Serialize(Material mat, StringBuilder tempObjectSb)
        {
            SerializeMetallicGlossMap();
            SerializeSpecColor(mat, tempObjectSb);
            SerializeMode(mat, tempObjectSb);
            SerializeEmissionColor(mat, tempObjectSb);
        }

        private static void SerializeEmissionColor(Material mat, StringBuilder tempObjectSb)
        {
            // NOTE: Unity doesn't currently import this information (I think) from an FBX file.
            if (!mat.HasProperty("_EmissionColor")) { return; }
            var color = mat.GetColor("_EmissionColor");

            tempObjectSb.AppendFormat("\t\t\tP: \"Emissive\", \"Vector3D\", \"Vector\", \"\",{0},{1},{2}", color.r, color.g, color.b);
            tempObjectSb.AppendLine();

            var averageColor = (color.r + color.g + color.b) / 3f;

            tempObjectSb.AppendFormat("\t\t\tP: \"EmissiveFactor\", \"Number\", \"\", \"A\",{0}", averageColor);
            tempObjectSb.AppendLine();
        }

        private static void SerializeMode(Material mat, StringBuilder tempObjectSb)
        {
            if (!mat.HasProperty("_Mode")) { return; }

            switch ((int)mat.GetFloat("_Mode"))
            {
                case 0: // Map is opaque
                    break;

                case 1: // Map is a cutout //  TODO: Add option if it is a cutout
                    break;

                case 2: // Map is a fade
                case 3: // Map is transparent
                    var color = mat.GetColor("_Color");

                    tempObjectSb.AppendFormat("\t\t\tP: \"TransparentColor\", \"Color\", \"\", \"A\",{0},{1},{2}", color.r, color.g, color.b);
                    tempObjectSb.AppendLine();
                    tempObjectSb.AppendFormat("\t\t\tP: \"Opacity\", \"double\", \"Number\", \"\",{0}", color.a);
                    tempObjectSb.AppendLine();
                    break;
            }
        }

        private static void SerializeSpecColor(Material mat, StringBuilder tempObjectSb)
        {
            if (!mat.HasProperty("_SpecColor")) { return; }
            var color = mat.GetColor("_SpecColor");
            tempObjectSb.AppendFormat("\t\t\tP: \"Specular\", \"Vector3D\", \"Vector\", \"\",{0},{1},{2}", color.r, color.g, color.r);
            tempObjectSb.AppendLine();
            tempObjectSb.AppendFormat("\t\t\tP: \"SpecularColor\", \"ColorRGB\", \"Color\", \" \",{0},{1},{2}", color.r, color.g, color.b);
            tempObjectSb.AppendLine();
        }

        private static void SerializeMetallicGlossMap()
        {
            // TODO: Figure out if this property can be written to the FBX file
            //if (!mat.HasProperty("_MetallicGlossMap")) { return; }
            //Debug.Log("has metallic gloss map");
            //Color color = mat.GetColor("_Color");
            //tempObjectSb.AppendFormat("\t\t\tP: \"Specular\", \"Vector3D\", \"Vector\", \"\",{0},{1},{2}", color.r, color.g, color.r);
            //tempObjectSb.AppendLine();
            //tempObjectSb.AppendFormat("\t\t\tP: \"SpecularColor\", \"ColorRGB\", \"Color\", \" \",{0},{1},{2}", color.r, color.g, color.b);
            //tempObjectSb.AppendLine();
        }
    }
}
