using HarmonyLib;
using STRINGS;
using System.Collections.Generic;

namespace Snow_Recipe_in_Rock_Crusher
{
    public class Patches
    {
        [HarmonyPatch(typeof(RockCrusherConfig))]
        [HarmonyPatch("ConfigureBuildingTemplate")]
        public class RockCrusherConfig_ConfigureBuildingTemplate_Patch
        {
            

            public static void Postfix()
            {                                
                ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement((Tag) "Ice", 100f)
                };
                ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement(SimHashes.Snow.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),                    
                };
                ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>)recipeElementArray1, (IList<ComplexRecipe.RecipeElement>)recipeElementArray2), recipeElementArray1, recipeElementArray2, DlcManager.AVAILABLE_DLC_2)
                {
                    time = 40f,
                    description = "Crushes Ice into Snow",
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                    fabricators = new List<Tag>()
                    {
                        TagManager.Create("RockCrusher")
                    }
                };
            }
        }
    }
}