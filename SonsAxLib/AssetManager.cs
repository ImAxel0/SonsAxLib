using RedLoader;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace SonsAxLib;

public class AssetManager
{
    /// <summary>
    /// Spawns a loaded assetbundle gameobject by name at a given position and rotation
    /// </summary>
    /// <param name="gameObjectName"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public static GameObject SpawnAsset(string gameObjectName, Vector3 position, Quaternion rotation)
    {
        GameObject go = AssetLoader.LoadedGameObjects.Find(x => x.name == gameObjectName + "(Clone)");
        if (go == null)
        {
            RLog.Warning("Spawned asset gameobject was null or couldn't be found");
            return null;
        }
        return UnityEngine.Object.Instantiate(go, position, rotation);
    }

    /// <summary>
    /// <para>Empties the loaded assetbundle gameobjects list (needed if switching to another save without closing the game)</para>
    /// <para>Call it on Sons main title scene</para>
    /// </summary>
    public static void ClearLoadedGameobjects()
    {
        AssetLoader.LoadedGameObjects.Clear();
    }
}
