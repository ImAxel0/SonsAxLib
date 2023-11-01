using HarmonyLib;
using System.Reflection;

namespace SonsAxLib;

public class HarmonyTools
{
    static List<MethodInfo> _patches = new();

    /// <summary>
    /// Creates and run a prefix patch on the given method
    /// </summary>
    /// <param name="instance">Harmony instance, use Redloader instance whenever possible</param>
    /// <param name="ogMethodClass">The game class containing the method to patch, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the method to patch, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="patchedMethodClass">The mod class containg the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="patchedMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    public static MethodInfo CreatePrefix(HarmonyLib.Harmony instance, Type ogMethodClass, string ogMethodName, Type patchedMethodClass, string patchedMethodName)
    {
        MethodInfo mt = instance.Patch(AccessTools.Method(ogMethodClass, ogMethodName), new HarmonyMethod(patchedMethodClass, patchedMethodName));
        _patches.Add(mt);
        return mt;
    }

    /// <summary>
    /// Creates and run a prefix patch on the given method with the specified argument types (needed for overloaded methods)
    /// </summary>
    /// <param name="instance">Harmony instance, use Redloader instance whenever possible</param>
    /// <param name="ogMethodClass">The game class containing the method to patch, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the method to patch, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="argumentTypes">Argument types of the original method, use <see langword="new"/>[] { <see langword="typeof"/>(type), ... }</param>
    /// <param name="patchedMethodClass">The mod class containg the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="patchedMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    public static MethodInfo CreatePrefix(HarmonyLib.Harmony instance, Type ogMethodClass, string ogMethodName, Type[] argumentTypes, Type patchedMethodClass, string patchedMethodName)
    {
        MethodInfo mt = instance.Patch(AccessTools.Method(ogMethodClass, ogMethodName, argumentTypes), new HarmonyMethod(patchedMethodClass, patchedMethodName));
        _patches.Add(mt);
        return mt;
    }

    /// <summary>
    /// Creates and run a postfix patch on the given method
    /// </summary>
    /// <param name="instance">Harmony instance, use Redloader instance whenever possible</param>
    /// <param name="ogMethodClass">The game class containing the method to patch, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the method to patch, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="patchedMethodClass">The mod class containg the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="patchedMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    public static MethodInfo CreatePostfix(HarmonyLib.Harmony instance, Type ogMethodClass, string ogMethodName, Type patchedMethodClass, string patchedMethodName)
    {
        MethodInfo mt = instance.Patch(AccessTools.Method(ogMethodClass, ogMethodName), null, new HarmonyMethod(patchedMethodClass, patchedMethodName));
        _patches.Add(mt);
        return mt;
    }

    /// <summary>
    /// Creates and run a postfix patch on the given method with the specified argument types (needed for overloaded methods)
    /// </summary>
    /// <param name="instance">Harmony instance, use Redloader instance whenever possible</param>
    /// <param name="ogMethodClass">The game class containing the method to patch, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the method to patch, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="argumentTypes">Argument types of the original method, use <see langword="new"/>[] { <see langword="typeof"/>(type), ... }</param>
    /// <param name="patchedMethodClass">The mod class containg the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="patchedMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    public static MethodInfo CreatePostfix(HarmonyLib.Harmony instance, Type ogMethodClass, string ogMethodName, Type[] argumentTypes, Type patchedMethodClass, string patchedMethodName)
    {
        MethodInfo mt = instance.Patch(AccessTools.Method(ogMethodClass, ogMethodName, argumentTypes), null, new HarmonyMethod(patchedMethodClass, patchedMethodName));
        _patches.Add(mt);
        return mt;
    }

    /// <summary>
    /// Removes the patch from a given method
    /// </summary>
    /// <param name="instance">Harmony instance, use Redloader instance whenever possible</param>
    /// <param name="ogMethodClass">The game class containing the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="patchType">[Optional] patch type to remove</param>
    public static void RemovePatch(HarmonyLib.Harmony instance, Type ogMethodClass, string ogMethodName, HarmonyPatchType patchType = HarmonyPatchType.All)
    {
        var original = AccessTools.Method(ogMethodClass, ogMethodName);
        instance.Unpatch(original, patchType);
        _patches.Remove(original);
    }

    /// <summary>
    /// Removes the patch from a given method with the specified argument types (needed for overloaded methods)
    /// </summary>
    /// <param name="instance">Harmony instance, use Redloader instance whenever possible</param>
    /// <param name="ogMethodClass">The game class containing the patched method, use <see langword="typeof"/>(ClassName)</param>
    /// <param name="ogMethodName">The name of the patched method, use <see langword="nameof"/>(ClassName.Method)</param>
    /// <param name="argumentTypes">Argument types of the original method, use <see langword="new"/>[] { <see langword="typeof"/>(type), ... }</param>
    /// <param name="patchType">[Optional] patch type to remove</param>
    public static void RemovePatch(HarmonyLib.Harmony instance, Type ogMethodClass, string ogMethodName, Type[] argumentTypes, HarmonyPatchType patchType = HarmonyPatchType.All)
    {
        var original = AccessTools.Method(ogMethodClass, ogMethodName, argumentTypes);
        instance.Unpatch(original, patchType);
        _patches.Remove(original);
    }

    /// <summary>
    /// Check if an harmony patch is running on the given method
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <returns><see langword="true"/> if running, <see langword="false"/> if not</returns>
    public static bool IsPatched(MethodInfo methodInfo)
    {
        return _patches.Contains(methodInfo);
    }

    /// <summary>
    /// Removes all running patches
    /// </summary>
    /// <param name="instance">Harmony instance, use Redloader instance whenever possible</param>
    public static void RemovePatches(HarmonyLib.Harmony instance)
    {
        instance.UnpatchSelf();
        _patches.Clear();
    }
}
