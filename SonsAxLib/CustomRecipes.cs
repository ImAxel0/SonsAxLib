using RedLoader;
using Sons.Crafting;
using Sons.Items.Core;
using SonsSdk;
using System.Collections;
using System.Linq;
using static Sons.Crafting.CraftingRecipe;

namespace SonsAxLib;

/// <summary>
/// Class to create and add new crafting recipes to the game
/// </summary>
public class CustomRecipes
{
    static List<ItemIdManager.ItemsId> _addedRecipes = new();

    /// <summary>
    /// <para>Let add new recipes to the game</para>
    /// <para>Usage: CreateRecipe("name", <see langword="new"/>() { { id, count }, ... }, resulting item id)</para>
    /// </summary>
    /// <param name="recipeName">Name of the recipe</param>
    /// <param name="idCountPair">Dictionary containing the required item id and it's required count</param>
    /// <param name="obtainedItemID">The resulting item from the craft</param>
    public static bool CreateRecipe(string recipeName, Dictionary<ItemIdManager.ItemsId, int> idCountPair, ItemIdManager.ItemsId obtainedItemID)
    {
        if (_addedRecipes.Contains(obtainedItemID))
        {
            RLog.Warning($"Recipe with obtained item id = {obtainedItemID} was already added to the game");
            SonsTools.ShowMessage($"<color=orange>Error</color>, recipe for {ItemDatabaseManager.ItemById((int)obtainedItemID).Name} already added");
            return false;
        }
        _addedRecipes.Add(obtainedItemID);
        CreateRecipeInternal(recipeName, idCountPair, obtainedItemID).RunCoro();
        return true;
    }

    internal static IEnumerator CreateRecipeInternal(string recipeName, Dictionary<ItemIdManager.ItemsId, int> idCountPair, ItemIdManager.ItemsId obtainedItemID)
    {
        while (!CraftingSystem.Instance) yield return null;

        Il2CppSystem.Collections.Generic.List<ResultingItem> resultingItems = new();
        Il2CppSystem.Collections.Generic.List<CraftingIngredient> ingredients = new();

        // recipe parameters
        CraftingRecipe craftingRecipe = new()
        {
            name = recipeName,
            _recipeType = CraftingRecipe.Type.CraftNewItem,
            _forceNewItemInstance = false,
            _craftCompleteAudioEvent = "event:/SotF_Music/crafting Music/crafting_weapons_end",
            _duringCraftJingleAudioEvent = "event:/SotF_Music/crafting Music/crafting_weapons_start"
        };

        // the obtained item ID
        ResultingItem resultingItem = new() { Id = (int)obtainedItemID };

        // adding obtained item to obtained list & setting it to the recipe
        resultingItems.Add(resultingItem);
        craftingRecipe._resultingItems = resultingItems;

        // adding required ingredients to the recipe
        foreach (var pair in idCountPair)
        {
            CraftingIngredient craftingIngredient = new()
            {
                ItemId = (int)pair.Key,
                Count = pair.Value,
                IsReusable = false
            };
            ingredients.Add(craftingIngredient);
        };

        // adding ingredients to the final recipe
        craftingRecipe._ingredients = ingredients;

        // Adding to game database
        CraftingSystem.Instance._recipeDatabase._recipes.Add(craftingRecipe);
    }
}
