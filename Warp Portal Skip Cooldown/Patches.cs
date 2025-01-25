using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Warp_Portal_Skip_Cooldown
{
    public class Patches
    {
        [HarmonyPatch(typeof(WarpPortal.WarpPortalSM), nameof(WarpPortal.WarpPortalSM.InitializeStates))]
        public class WarpPortal_SkipCooldown_Hopefully
        {
            public static void Postfix(WarpPortal.WarpPortalSM __instance)
            {                
                __instance.recharging.Enter((smi) => { __instance.isCharged.Set(true, smi); smi.GoTo(__instance.idle); });                
            }
        }        
    }
}
