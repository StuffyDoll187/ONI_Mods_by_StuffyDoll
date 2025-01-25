using HarmonyLib;
using Klei.AI;
using UnityEngine;

namespace Fartomic_Bomb
{
    public class Patches
    {

        [HarmonyPatch(typeof(MinionConfig))]
        [HarmonyPatch("CreatePrefab")]
        internal class MinionConfig_CreatePrefab_Patch
        {
            private static GameObject Postfix(GameObject __result)
            {                
                __result.AddComponent<SaveOnDuplicant>();
                return __result;
            }
        }

        [HarmonyPatch(typeof(MinionConfig))]
        [HarmonyPatch("AddMinionTraits")]
        internal class MinionConfig_AddMinionTraits_Patch
        {
            private static void Postfix(Modifiers modifiers)
            {                
                modifiers.initialTraits.Add("FartomicBomb");               
            }
        }
        
    }
}
