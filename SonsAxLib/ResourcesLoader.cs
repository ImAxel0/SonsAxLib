using RedLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SonsAxLib;

public class ResourcesLoader
{
    /// <summary>
    /// Get raw bytes of embedded resources like images, sounds etc.
    /// </summary>
    /// <param name="name">Name of the resource</param>
    /// <param name="bytes">Outputted byte array</param>
    /// <returns><see langword="true"/> if successfull, <see langword="false"/> if not</returns>
    public static bool TryGetEmbeddedResourceBytes(string name, out byte[] bytes)
    {
        bytes = null;

        var executingAssembly = Assembly.GetExecutingAssembly();

        var desiredManifestResources = executingAssembly.GetManifestResourceNames().FirstOrDefault(resourceName => {
            var assemblyName = executingAssembly.GetName().Name;
            return !string.IsNullOrEmpty(assemblyName) && resourceName.StartsWith(assemblyName) && resourceName.Contains(name);
        });

        if (string.IsNullOrEmpty(desiredManifestResources))
            return false;

        using (var ms = new MemoryStream())
        {
            executingAssembly.GetManifestResourceStream(desiredManifestResources).CopyTo(ms);
            bytes =  ms.ToArray();
            return true;
        }
    }

    /// <summary>
    /// Convert raw bytes to Texture2D
    /// </summary>
    /// <param name="imgBytes"></param>
    /// <returns><see langword="Texture2D"/> from the given byte array</returns>
    public static Texture2D ByteToTex(byte[] imgBytes)
    {
        Texture2D tex = new(2, 2);
        tex.LoadImage(imgBytes);
        return tex;
    }

    /// <summary>
    /// Convert Texture2D to Sprite
    /// </summary>
    /// <param name="texture"></param>
    /// <returns><see langword="Sprite"/> from the given Texture2D</returns>
    public static Sprite ToSprite(Texture2D texture)
    {
        var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
        var pivot = new Vector2(0.5f, 0.5f);
        var border = Vector4.zero;
        var sprite = Sprite.CreateSprite_Injected(texture, ref rect, ref pivot, 100, 0, SpriteMeshType.Tight, ref border, false, null);
        sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        return sprite;
    }
}
