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
    public static class FBXObjectSerializer
    {
        public static void Serialize(GameObject[] gameObjects, string newPath, bool copyMaterials, bool copyTextures, StringBuilder objectProps, StringBuilder objectConnections)
        {
            ObjectHeader(objectProps);
            ConnectionsHeader(objectConnections);
            foreach (var gameObject in gameObjects)
            {
                // Run recursive FBX Mesh grab over the entire gameObject
                FBXMeshSerializer.Serialize(gameObject, objectProps, objectConnections);
                // First finds all unique materials and compiles them (and writes to the object connections) for funzies
                FBXMaterialSerializer.Serialize(gameObject, newPath, objectProps, objectConnections, copyMaterials, copyTextures);
            }

            ObjectFooter(objectProps);
            ConnectionFooter(objectConnections);
        }

        private static void ConnectionFooter(StringBuilder objectConnections)
        {
            objectConnections.AppendLine("}");
        }

        private static void ObjectFooter(StringBuilder objectProps)
        {
            objectProps.AppendLine("}");
        }

        private static void ConnectionsHeader(StringBuilder objectConnections)
        {
            objectConnections.AppendLine("; Object connections");
            objectConnections.AppendLine(";------------------------------------------------------------------");
            objectConnections.AppendLine("");
            objectConnections.AppendLine("Connections:  {");
            objectConnections.AppendLine("\t");
        }

        private static void ObjectHeader(StringBuilder objectProps)
        {
            objectProps.AppendLine("; Object properties");
            objectProps.AppendLine(";------------------------------------------------------------------");
            objectProps.AppendLine("");
            objectProps.AppendLine("Objects:  {");
        }
    }
}
