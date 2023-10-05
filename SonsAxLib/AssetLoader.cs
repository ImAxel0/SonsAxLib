using Il2CppInterop.Runtime;
using RedLoader.Utils;
using UnityEngine;

namespace SonsAxLib;

public class AssetLoader
{
    private static List<AssetBundle> _bundles = new();
    public static List<GameObject> _loadedGameObjects = new();

    public static List<GameObject> LoadedGameObjects
    {
        get { return _loadedGameObjects; }
    }

    /// <summary>
    /// Load all bundle files at given folder path [path must contain assetbundles only], call this on <see langword="OnSdkInitialized"/>
    /// </summary>
    public static void LoadAllBundles(string path)
    {
        string[] bundleFiles = Directory.GetFiles(path);
        bundleFiles.ToList().ForEach(bundleFile => { _bundles.Add(AssetBundle.LoadFromFile(bundleFile)); });
    }

    /// <summary>
    /// Load all assets of the given bundle files and stores them as gameobjects in <see langword="LoadedGameObjects"/>, call this on <see langword="OnGameStart"/> after calling <see langword="LoadAllBundles"/> first
    /// </summary>
    public static void LoadAllAssets()
    {
        int counter = 0;
        foreach (AssetBundle bundle in _bundles)
        {
            string[] assetPath = bundle.GetAllAssetNames();
            GameObject original = bundle.LoadAsset(assetPath.Last(), Il2CppType.Of<GameObject>()).Cast<GameObject>();
            _loadedGameObjects.Add(UnityEngine.Object.Instantiate(original));
            LoadShaders(_loadedGameObjects.ElementAt(counter));
            counter++;
        }
    }

    private static void LoadShaders(GameObject gameObject)
    {
        MeshRenderer goMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        goMeshRenderer?.materials.ToList().ForEach(material => { 
                if (material.shader.isSupported) material.shader = Shader.Find("Sons/HDRPLit"); 
            });

        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            MeshRenderer meshRenderer = gameObject.transform.GetChild(j).GetComponent<MeshRenderer>();
            if (meshRenderer == null) continue;

            meshRenderer.materials.ToList().ForEach(material => { 
                if (material.shader.isSupported) material.shader = Shader.Find("Sons/HDRPLit"); 
            });
        }
    }
}
