using HarmonyLib;
using UnityEngine;

namespace Atmo_Suit_Capacity_Config
{
    public class Patches
    {
        [HarmonyPatch(typeof(AtmoSuitConfig), nameof(AtmoSuitConfig.DoPostConfigure))]
        public class AtmoSuitConfig_DoPostConfigure_Patch
        {
            public static void Postfix(GameObject go)
            {
                if(go.TryGetComponent(out SuitTank suitTank))
                {
                    suitTank.capacity = Config.Instance.capacityInKg;
                }
            }
        }

    }
}
