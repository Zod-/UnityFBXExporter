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
using System.IO;
using System.Text;

namespace UnityFBXExporter
{
    public static class FbxExporter
    {
        public static string MeshToString(GameObject[] gameObjects, string newPath)
        {
            var stringBuilder = new StringBuilder();
            foreach (var lines in FbxAsciiWriter.SerializeDocument(new FbxDocument(gameObjects, newPath)))
            {
                stringBuilder.AppendLine(lines);
            }
            return stringBuilder.ToString();
        }

        public static bool ExportGameObjToFbx(GameObject[] gameObjects, string newPath)
        {
            // Check to see if the extension is right
            if (newPath.Remove(0, newPath.LastIndexOf('.')) != ".fbx")
            {
                Debug.LogError("The end of the path wasn't \".fbx\"");
                return false;
            }

            var buildMesh = MeshToString(gameObjects, newPath);

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }

            File.WriteAllText(newPath, buildMesh);
            return true;
        }

        public static int[] FlipYZTriangles(int[] triangles, bool endPoly = false)
        {
            for (var i = 0; i < triangles.Length; i += 3)
            {
                var swap = triangles[i + 2];
                if (endPoly)
                {
                    triangles[i + 2] = -1 + triangles[i + 1] * -1;
                }
                else
                {
                    triangles[i + 2] = triangles[i + 1];
                }
                triangles[i + 1] = swap;
            }
            return triangles;
        }
    }
}
