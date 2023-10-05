using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Sons.Gameplay;

namespace SonsAxLib;

public class HarmonyTools
{
    static HarmonyLib.Harmony? _harmony;

    /// <summary>
    /// Creates the needed harmony instance, MUST be called one time before using others <see langword="HarmonyTools"/> methods
    /// </summary>
    /// <param name="id"></param>
    public static void CreateInstance(string id)
    {
        _harmony = new(id);
    }

    /// <summary>
    /// Creates and run a prefix patch on the given method
    /// </summary>
    /// <param name="ogMethodClass">The game class containing the method to patch, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the method to patch, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="patchedMethodClass">The mod class containg the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="patchedMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    public static void CreatePrefix(Type ogMethodClass, string ogMethodName, Type patchedMethodClass, string patchedMethodName)
    {
        _harmony?.Patch(AccessTools.Method(ogMethodClass, ogMethodName), new HarmonyMethod(patchedMethodClass, patchedMethodName));
    }

    /// <summary>
    /// Creates and run a prefix patch on the given method with the specified argument types (needed for overloaded methods)
    /// </summary>
    /// <param name="ogMethodClass">The game class containing the method to patch, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the method to patch, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="argumentTypes">Argument types of the original method, use <see langword="new"/>[] { <see langword="typeof"/>(type), ... }</param>
    /// <param name="patchedMethodClass">The mod class containg the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="patchedMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    public static void CreatePrefix(Type ogMethodClass, string ogMethodName, Type[] argumentTypes, Type patchedMethodClass, string patchedMethodName)
    {
        _harmony?.Patch(AccessTools.Method(ogMethodClass, ogMethodName, argumentTypes), new HarmonyMethod(patchedMethodClass, patchedMethodName));
    }

    /// <summary>
    /// Creates and run a postfix patch on the given method
    /// </summary>
    /// <param name="ogMethodClass">The game class containing the method to patch, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the method to patch, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="patchedMethodClass">The mod class containg the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="patchedMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    public static void CreatePostfix(Type ogMethodClass, string ogMethodName, Type patchedMethodClass, string patchedMethodName)
    {
        _harmony?.Patch(AccessTools.Method(ogMethodClass, ogMethodName), null, new HarmonyMethod(patchedMethodClass, patchedMethodName));
    }

    /// <summary>
    /// Creates and run a postfix patch on the given method with the specified argument types (needed for overloaded methods)
    /// </summary>
    /// <param name="ogMethodClass">The game class containing the method to patch, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the method to patch, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="argumentTypes">Argument types of the original method, use <see langword="new"/>[] { <see langword="typeof"/>(type), ... }</param>
    /// <param name="patchedMethodClass">The mod class containg the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="patchedMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    public static void CreatePostfix(Type ogMethodClass, string ogMethodName, Type[] argumentTypes, Type patchedMethodClass, string patchedMethodName)
    {
        _harmony?.Patch(AccessTools.Method(ogMethodClass, ogMethodName, argumentTypes), null, new HarmonyMethod(patchedMethodClass, patchedMethodName));
    }

    /// <summary>
    /// Removes the patch from a given method
    /// </summary>
    /// <param name="ogMethodClass">The game class containing the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="patchType">[Optional] patch type to remove</param>
    public static void RemovePatch(Type ogMethodClass, string ogMethodName, HarmonyPatchType patchType = HarmonyPatchType.All)
    {
        var original = AccessTools.Method(ogMethodClass, ogMethodName);
        _harmony?.Unpatch(original, patchType);
    }

    /// <summary>
    /// Removes the patch from a given method with the specified argument types (needed for overloaded methods)
    /// </summary>
    /// <param name="ogMethodClass">The game class containing the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="argumentTypes">Argument types of the original method, use <see langword="new"/>[] { <see langword="typeof"/>(type), ... }</param>
    /// <param name="patchType">[Optional] patch type to remove</param>
    public static void RemovePatch(Type ogMethodClass, string ogMethodName, Type[] argumentTypes, HarmonyPatchType patchType = HarmonyPatchType.All)
    {
        var original = AccessTools.Method(ogMethodClass, ogMethodName, argumentTypes);
        _harmony?.Unpatch(original, patchType);
    }

    /// <summary>
    /// Removes all running patches
    /// </summary>
    public static void RemovePatches()
    {
        _harmony?.UnpatchSelf();
    }
}
