using RedLoader;
using Sons.Gameplay;
using Sons.Gui.Input;
using Sons.Input;
using System.Diagnostics;
using TheForest.Utils;
using UnityEngine;

namespace SonsAxLib;

/// <summary>
/// Class which manage the creation and usage of the in game UI interaction
/// </summary>
public class Interactable
{
    private static Func<GameObject, LinkUiElement> _getUIElement = InteractionElement => InteractionElement.GetComponentInChildren<LinkUiElement>();

    public enum InteractableType
    {
        Take = 0,
        Open = 1
    }

    /// <summary>
    /// Adds the game interact UI to the given GameObject
    /// </summary>
    /// <param name="go"></param>
    /// <param name="activeDistance">Distance from the Player and Gameobject where the UI will appear</param>
    /// <param name="type">Type of interaction: Take = quick press | Open = long press</param>
    /// <param name="tex">Icon of the interact UI</param>
    /// <returns>The <see langword="LinkUiElement"/> component associated with the given GameObject, <see langword="null"/> if the GameObject already has an interactable UI</returns>
    public static LinkUiElement Create(GameObject go, float activeDistance = 3f, InteractableType type = InteractableType.Take, Texture tex = null)//
    {
        if (_getUIElement(go))
        {
            RLog.Error("Given GameObject already has an Interactable UI");
            return null;
        }
        
        bool useTex = tex != null;
        string _type = (type == InteractableType.Take) ? "screen.take" : "screen.open";

        GameObject _PickupGui_ = new() { name = "_PickupGui_" };
        GameObject InteractionElement = new() { name = "InteractionElement" };
        LinkUiElement uiElement = InteractionElement.AddComponent<LinkUiElement>();
        uiElement._applyTexture = useTex;
        uiElement._texture = tex;
        uiElement._maxDistance = activeDistance;
        uiElement._uiElementId = _type;
        uiElement.OnEnable();

        InteractionElement.transform.SetParent(_PickupGui_.transform, false);
        _PickupGui_.transform.SetParent(go.transform, false);

        return uiElement;
    }

    /// <summary>
    /// Check if the player can interact with the given GameObject
    /// </summary>
    /// <param name="go">The GameObject to check</param>
    /// <returns><see langword="true"/> if the UI is active, <see langword="false"/> if not active or the GameObject doesn't have an interactable UI</returns>
    internal static bool CanInteract(GameObject go)
    {
        if (_getUIElement(go) == null) return false;
        return _getUIElement(go).IsActive;
    }

    /// <summary>
    /// Check if the game interaction key is short pressed on a gameobject having an interactable UI
    /// </summary>
    /// <returns><see langword="true"/> if interactable UI is active and key is pressed, <see langword="false"/> if not or gameobject doesn't have an interactable UI</returns>
    public static bool CanShortPress(GameObject go)
    {
        if (InputSystem.InputMapping.@default.Interact.triggered && CanInteract(go))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if the game interaction key is long pressed on a gameobject having an interactable UI
    /// </summary>
    /// <code>int x = 0;</code>
    /// <returns><see langword="true"/> if interactable UI is active and key is pressed, <see langword="false"/> if not or gameobject doesn't have an interactable UI</returns>
    public static bool CanLongPress(GameObject go)
    {
        if (InputSystem.InputMapping.@default.Use.triggered && CanInteract(go))
        {
            return true;
        }
        return false;
    }
}