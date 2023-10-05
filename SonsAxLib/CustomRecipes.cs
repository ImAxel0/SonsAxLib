using RedLoader;
using Sons.Crafting;
using System.Collections;
using static Sons.Crafting.CraftingRecipe;

namespace SonsAxLib;

/// <summary>
/// Class to create and add new crafting recipes to the game
/// </summary>
public class CustomRecipes
{
    static List<int> _addedRecipes = new();

    /// <summary>
    /// <para>Let add new recipes to the game</para>
    /// <para>Usage: CreateRecipe("name", <see langword="new"/>() { { id, count }, ... }, resulting item id)</para>
    /// </summary>
    /// <param name="recipeName">Name of the recipe</param>
    /// <param name="idCountPair">Dictionary containing the required item id and it's required count</param>
    /// <param name="obtainedItemID">The resulting item from the craft</param>
    public static void CreateRecipe(string recipeName, Dictionary<int, int> idCountPair, int obtainedItemID)
    {
        if (_addedRecipes.Contains(obtainedItemID))
        {
            RLog.Warning($"Recipe with obtained item id = {obtainedItemID} was already added to the game");
            return;
        }
        CreateRecipeInternal(recipeName, idCountPair, obtainedItemID).RunCoro();
    }

    internal static IEnumerator CreateRecipeInternal(string recipeName, Dictionary<int, int> idCountPair, int obtainedItemID)
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
        ResultingItem resultingItem = new() { Id = obtainedItemID };

        // adding obtained item to obtained list & setting it to the recipe
        resultingItems.Add(resultingItem);
        craftingRecipe._resultingItems = resultingItems;

        // adding required ingredients to the recipe
        foreach (var pair in idCountPair)
        {
            CraftingIngredient craftingIngredient = new()
            {
                ItemId = pair.Key,
                Count = pair.Value,
                IsReusable = false
            };
            ingredients.Add(craftingIngredient);
        };

        // adding ingredients to the final recipe
        craftingRecipe._ingredients = ingredients;

        // Adding to game database
        CraftingSystem.Instance._recipeDatabase._recipes.Add(craftingRecipe);
        _addedRecipes.Add(obtainedItemID);
    }
}
