using HarmonyLib;
using System.Collections.Generic;

namespace Lumen_Quartz_Dig_Site
{
    public class Patches
    {
        [HarmonyPatch(typeof(FossilDigSiteConfig))]
        [HarmonyPatch("ConfigureRecipes")]
        public class FossilDigSiteConfig_ConfigureRecipes
        {
            public static void Postfix()
            {
                ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement(SimHashes.Diamond.CreateTag(), 10f)
                };
                ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement("PinkRockCarved", 1f)
                };
                ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("FossilDig", (IList<ComplexRecipe.RecipeElement>)recipeElementArray1, (IList<ComplexRecipe.RecipeElement>)recipeElementArray2), recipeElementArray1, recipeElementArray2, dlcIds: DlcManager.AVAILABLE_DLC_2)
                {
                    time = 80f,
                    description = "Mine Lumen Quartz",
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                    fabricators = new List<Tag>() { (Tag)"FossilDig" },
                    sortOrder = 22
                };
            }
        }
               

    }
}
