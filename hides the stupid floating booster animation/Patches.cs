using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace hides_the_stupid_floating_booster_animation
{
    public class Patches
    {
        [HarmonyPatch(typeof(BionicUpgrade_SkilledWorker),nameof(BionicUpgrade_SkilledWorker.CreateFX))]
        public class Hide_Booster_Anim_CreateFX_Patch
        {
            public static bool Prefix()
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(BionicUpgrade_SkilledWorker), nameof(BionicUpgrade_SkilledWorker.ClearFX))]
        public class Hide_Booster_Anim_ClearFX_Patch
        {
            public static bool Prefix(BionicUpgrade_SkilledWorker.Instance smi)
            {
                if (smi.fx != null)
                    return true;
                return false;
            }
        }
    }
}
