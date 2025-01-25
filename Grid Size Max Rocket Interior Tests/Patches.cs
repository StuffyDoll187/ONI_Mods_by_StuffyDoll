using HarmonyLib;
using ProcGenGame;
using System.Collections.Generic;
using UnityEngine;

namespace Grid_Size_Max_Rocket_Interior_Tests
{
    public class Patches
    {
        [HarmonyPatch(typeof(NoFreeRocketInterior),nameof(NoFreeRocketInterior.EvaluateCondition))]
        public class NoFreeRocketInterior_EvaluateCondition_Patch
        {
            public static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }
        

        [HarmonyPatch(typeof(GridSettings), nameof(GridSettings.Reset))]
        public class GridSettings_Reset_Patch_Post
        {
            public static void Postfix(ref int width, ref int height)
            {
                Debug.Log(width.ToString() + " " + height.ToString());                
            }
        }        

        [HarmonyPatch(typeof(Cluster), nameof(Cluster.Load))]
        public class Cluster_Load_Patch
        {
            public static void Postfix(ref Cluster __result)
            {
                Debug.Log(__result.size.y.ToString());
                __result.size.y += 96;
                //__result.size.y *= 2;
                Debug.Log(__result.size.y.ToString());
            }
        }
    }
}
