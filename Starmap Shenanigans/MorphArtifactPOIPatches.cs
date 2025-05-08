using HarmonyLib;
using System.Collections.Generic;

namespace Starmap_Shenanigans
{
    public class MorphArtifactPOIPatches
    {
        /// <summary>
        /// Adds the buttons for Morphing and Duplicating Artifact POIs.
        /// </summary>
        [HarmonyPatch(typeof(ClusterGridEntity), "OnSpawn")]
        public class ClusterGridEntity_OnSpawn_Patch
        {
            public static void Postfix(ClusterGridEntity __instance)
            {
                //if (!DebugHandler.enabled)
                    //return;
                if (__instance.TryGetComponent(out ArtifactPOIClusterGridEntity _))
                {
                    __instance.FindOrAddComponent<DuplicateArtifactPOI>();
                    __instance.FindOrAddComponent<MorphArtifactPOI>();
                }
            }
        }
        /// <summary>
        /// Grabs the list of Artifact POIs.
        /// </summary>
        [HarmonyPatch(typeof(ArtifactPOIConfig), "GenerateConfigs")]
        public class ArtifactPOIConfig_GenerateConfigs_Patch
        {
            public static void Postfix(List<ArtifactPOIConfig.ArtifactPOIParams> __result)
            {                
                MorphArtifactPOI.artifactPOIParams = __result;
            }
        }
    }
}
