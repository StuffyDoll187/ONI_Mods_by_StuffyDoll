using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace No_Rocket_Height_Limit
{
    public class Patches
    {
        [HarmonyPatch(typeof(CraftModuleInterface), nameof(CraftModuleInterface.RocketHeight), MethodType.Getter)]
        public class CraftModuleInterface_RocketHeight_Patch
        {
            public static void Postfix(ref int __result)
            {
                __result = 0;
            }
        }

    }
}
