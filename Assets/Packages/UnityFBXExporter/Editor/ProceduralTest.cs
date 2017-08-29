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
using UnityEditor;

public static class ProceduralTest
{
    [MenuItem("Assets/FBX Exporter/Create Object With Procedural Texture", false, 43)]
    public static void CreateObject()
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var mat = new Material(Shader.Find("Standard"))
        {
            mainTexture = ProceduralTexture(128)
        };
        cube.GetComponent<MeshRenderer>().sharedMaterial = mat;
    }

    private static Texture2D ProceduralTexture(int size)
    {
        var texture = new Texture2D(size, size);
        for (var x = 0; x < size; ++x)
        {
            for (var y = 0; y < size; ++y)
            {
                texture.SetPixel(x, y, ProceduralPixelColor(x, y));
            }
        }

        texture.Apply();
        return texture;
    }

    private static Color ProceduralPixelColor(int x, int y)
    {
        return (x - 64) * (x - 64) + (y - 64) * (y - 64) < 1000 ? Color.white : Color.black;
    }
}
