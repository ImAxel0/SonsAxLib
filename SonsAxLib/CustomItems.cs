using RedLoader;
using Sons.Items.Core;
using Sons.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SonsAxLib;

public class CustomItems
{
    static List<GameObject> _addedGameObjects = new();

    private static void AddItem(ItemData itemData)
    {
        ItemDatabaseManager._itemsCache.Add(itemData.Id, itemData);
        ItemDatabaseManager._instance._itemDataList.Add(itemData);
    }

    /// <summary>
    /// Add a custom gameobject to the game item list
    /// </summary>
    /// <param name="go">The gameobject to add</param>
    /// <param name="itemName">Choose a name for the gameobject</param>
    /// <param name="itemId">Choose an id for the gameobject which isn't already used by the game</param>
    /// <param name="canBeSpawned">Set if the item can be spawned with game debug console</param>
    public static void Create(GameObject go, string itemName, int itemId, bool canBeSpawned = true)
    {
        if (_addedGameObjects.Contains(go))
        {
            RLog.Error("Given gameobject is already added to the game");
            return;
        }

        if (ItemDatabaseManager.ItemById(itemId))
        {
            RLog.Error("Given id is already used in game");
            return;
        }

        ItemData itemData = new()
        {
            name = itemName,
            _canBeSpawned = canBeSpawned,
            _eachInstanceIsUnique = true,
            _editorName = itemName + " ID(" + itemId + ")",
            _heldPrefab = go.transform,
            _id = itemId,
            _name = itemName,
            _pickupPrefab = go.transform,
        };
        AddItem(itemData); _addedGameObjects.Add(go);
    }
}
