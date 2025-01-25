// Decompiled with JetBrains decompiler
// Type: AdvancedCureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B86E23FE-3B43-4053-84B0-ABB90493789E
// Assembly location: E:\SteamLibrary\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

namespace Upgradeable_Dupes_And_Critters
{
    public class RadioactiveSerumConfig : IEntityConfig
    {
        public const string ID = "RadioactiveSerum";
        public const string DESC = "Used to modify Duplicants' and Critters' DNA";
        public static Tag Tag = TagManager.Create(ID);
        public static ComplexRecipe recipe;

        public static MedicineInfo MedicineInfo = new MedicineInfo("RadioactiveSerum", "InjectedWithRadioactiveSerum", MedicineInfo.MedicineType.Booster, "InjectionChamber");

        public string[] GetDlcIds() => DlcManager.AVAILABLE_EXPANSION1_ONLY;

        public GameObject CreatePrefab()
        {
            //GameObject looseentity = EntityTemplates.ExtendEntityToMedicine(EntityTemplates.CreateLooseEntity("RadioactiveSerum", "Radioactive Serum", RadioactiveSerumConfig.DESC, 1f, true, Assets.GetAnim((HashedString)"vial_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), MedicineInfo);
            GameObject looseentity = EntityTemplates.CreateLooseEntity("RadioactiveSerum", "Radioactive Serum", RadioactiveSerumConfig.DESC, 1f, true, Assets.GetAnim((HashedString)"vial_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true);

            ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[3]
            {
                new ComplexRecipe.RecipeElement(SimHashes.Glass.CreateTag(), 1f),
                new ComplexRecipe.RecipeElement(SimHashes.SaltWater.CreateTag(), 1f),
                new ComplexRecipe.RecipeElement(SimHashes.Steel.CreateTag(), 1f)
            };
            ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
            {
      new ComplexRecipe.RecipeElement((Tag) "RadioactiveSerum", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            string fabricator = "NuclearApothecary";
            RadioactiveSerumConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(fabricator, (IList<ComplexRecipe.RecipeElement>)recipeElementArray1, (IList<ComplexRecipe.RecipeElement>)recipeElementArray2), recipeElementArray1, recipeElementArray2)
            {
                time = 200f,
                description = RadioactiveSerumConfig.DESC,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators = new List<Tag>() { (Tag)fabricator },
                sortOrder = 20,
                //requiredTech = "MedicineIV"
            };
            //looseentity.AddTag(GameTags.Medicine);
            return looseentity;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}