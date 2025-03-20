using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Starmap_Shenanigans
{
    public class MorphHarvestablePOIPatches
    {
        /// <summary>
        /// Fix for Carbon Asteroid Field having the incorrect animation.
        /// </summary>
        [HarmonyPatch(typeof(HarvestablePOIConfig.HarvestablePOIParams), MethodType.Constructor, new Type[] { typeof(string), typeof(HarvestablePOIConfigurator.HarvestablePOIType) })]
        public class HarvestablePOIParams_Constructor_Patch
        {
            public static void Prefix(ref string anim, HarvestablePOIConfigurator.HarvestablePOIType poiType)
            {
                if (poiType.id == "CarbonAsteroidField")
                    anim = "carbon_asteroid_field";
            }
        }

        /// <summary>
        /// Adds the buttons for Morphing and Duplicating Harvestable POIs.
        /// </summary>
        [HarmonyPatch(typeof(ClusterGridEntity), "OnSpawn")]
        public class ClusterGridEntity_OnSpawn_Patch
        {
            public static void Postfix(ClusterGridEntity __instance)
            {
                if (!DebugHandler.enabled)
                    return;
                if (__instance.TryGetComponent(out HarvestablePOIClusterGridEntity _))
                {
                    __instance.FindOrAddComponent<DuplicateHarvestablePOI>();
                    __instance.FindOrAddComponent<MorphHarvestablePOI>();
                }
            }
        }

        
        /// <summary>
        /// Grabs the list of Harvestable POIs.
        /// </summary>
        public class HarvestablePOIConfig_GenerateConfigs_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var original = typeof(HarvestablePOIConfig).GetMethod("GenerateConfigs", BindingFlags.NonPublic | BindingFlags.Instance);
                var postfix = typeof(HarvestablePOIConfig_GenerateConfigs_Patch).GetMethod(nameof(Postfix));
                harmony.Patch(original, null, new HarmonyMethod(postfix), null, null);
            }
            public static void Postfix(ref List<HarvestablePOIConfig.HarvestablePOIParams> __result)
            {                
                MorphHarvestablePOI.harvestablePOIParams = __result;
            }
        }
        /// <summary>
        /// Must do a manual patch after Db Init since the type initializer for HarvestablePOIConfig includes calls to Db
        /// </summary>
        [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public class Db_Initialize_Patch
        {
            public static Harmony Harmony;
            public static void Postfix()
            {
                HarvestablePOIConfig_GenerateConfigs_Patch.Patch(Harmony);
            }
        }
    }
}
