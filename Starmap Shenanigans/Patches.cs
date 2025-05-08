using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace Starmap_Shenanigans
{
    class Patches
    {


        /// <summary>
        /// Disables POI Morph Options Screen if debug not enabled
        /// </summary>
        [HarmonyPatch(typeof(FewOptionSideScreen), nameof(FewOptionSideScreen.IsValidForTarget))]
        public class FewOptionSideScreen_IsValidForTarget
        {
            public static void Postfix(GameObject target, ref bool __result)
            {
                if (!DebugHandler.enabled && (target.TryGetComponent(out MorphArtifactPOI _) || target.TryGetComponent(out MorphHarvestablePOI _)))
                    __result &= false;
            }
        }


    }
}
