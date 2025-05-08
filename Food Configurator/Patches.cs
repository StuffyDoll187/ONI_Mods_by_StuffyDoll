using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using STRINGS;

namespace Food_Configurator
{
    public class Patches
    {
        [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public class Db_Init_Patch
        {
            public static void Postfix()
            {
                Config.LoadConfig();
                Config.CheckForAdditionalFoods();
                Config.ApplyConfig();
            }
        }

        [HarmonyPatch(typeof(GameUtil), nameof(GameUtil.GetFormattedFoodQuality))]
        public class GameUtil_GetFormattedFoodQuality_IndexOutOfBoundsFix
        {
            public static bool Prefix(int quality, ref string __result)
            {
                if (GameUtil.adjectives == null)
                    GameUtil.adjectives = LocString.GetStrings(typeof(DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVES));
                LocString format = quality >= 0 ? DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_POSITIVE : DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_NEGATIVE;
                int index = Mathf.Clamp(quality - DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_INDEX_OFFSET, 0, GameUtil.adjectives.Length - 1);
                __result = string.Format((string)format, GameUtil.adjectives[index], GameUtil.AddPositiveSign(quality.ToString(), quality > 0));
                return false;
            }
        }
    }
}
