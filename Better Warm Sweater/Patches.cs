using HarmonyLib;
using Klei.AI;
using STRINGS;

namespace Better_Warm_Sweater
{
    public class Patches
    {
        [HarmonyPatch(typeof(ClothingWearer),nameof(ClothingWearer.ChangeClothes))]        
        public class ClothingWearer_ChangeClothes_Patch
        {            
            public static void Postfix(ClothingWearer __instance, ref AttributeModifier ___conductivityModifier, ref AttributeModifier ___decorModifier)
            {                
                if (__instance.currentClothing.name == (string)EQUIPMENT.PREFABS.WARM_VEST.NAME) 
                {                   
                    ___conductivityModifier.SetValue(0.05f);                    
                    ___decorModifier.SetValue(30);                    
                };               
            }
        }
    }
}
