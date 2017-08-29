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
