using HarmonyLib;
using STRINGS;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Buildings_Anywhere
{
    public class Patches : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            
        }

        

        [HarmonyPatch(typeof(BuildingTemplates))]
        [HarmonyPatch("CreateBuildingDef")]
        public class BuildingTemplates_CreateBuildingDef_Patch
        {
            public static void Postfix(ref BuildingDef __result)
            {               
                __result.BuildLocationRule = BuildLocationRule.Anywhere;                
            }
        }

        [HarmonyPatch(typeof(BuildingConfigManager))]
        [HarmonyPatch("OnPrefabInit")]
        public class BuildingConfigManager_OnPrefabInit_Patch
        {
            public static void Postfix(ref HashSet<System.Type> ___defaultBuildingCompleteKComponents)
            {
                ___defaultBuildingCompleteKComponents.Remove(typeof(RequiresFoundation));                
            }
        }

        
    }

}