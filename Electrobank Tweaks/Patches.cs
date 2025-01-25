using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Electrobank_Tweaks
{
    public class Patches
    {
        [HarmonyPatch(typeof(Electrobank),"OnPrefabInit")]
        public class Electrobank_OnPreabInit_Patch
        {
            public static void Postfix(ref float ___capacity)
            {
                ___capacity = Config.Instance.PowerBankCapacity * 1000;
            }
        }
        [HarmonyPatch(typeof(SelfChargingElectrobank),"OnSpawn")]
        public class SelfChargingElectrobank_OnSpawn_Patch
        {
            public static void Postfix(ref float ___lifetimeRemaining, KSelectable ___selectable)
            {                                
                ___selectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElectrobankSelfCharging);
                ___selectable.AddStatusItem(Db.Get().MiscStatusItems.ElectrobankSelfCharging, (float)Config.Instance.DuraBankSelfChargingRate); 
                
                if (Config.Instance.EternalDuraBank)
                    ___selectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElectrobankLifetimeRemaining);
            }
        }
        /*[HarmonyPatch(typeof(SelfChargingElectrobankConfig), nameof(SelfChargingElectrobankConfig.CreatePrefab))]
        public class SelfChargingElectrobankConfig_CreatePrefab_Patch
        {
            public static void Postfix(GameObject __result)
            {
                //__result.GetComponent<SelfChargingElectrobank>().LifetimeRemaining = 900000f;                
            }
        }*/
        [HarmonyPatch(typeof(SelfChargingElectrobank), nameof(SelfChargingElectrobank.Sim200ms))]
        public class SelfChargingElectrobank_Sim200ms_Patch
        {
            public static bool Prefix(ref float ___lifetimeRemaining, float dt, SelfChargingElectrobank __instance)
            {                
                if (___lifetimeRemaining > 0)
                {
                    __instance.AddPower(dt * Config.Instance.DuraBankSelfChargingRate);
                    if (!Config.Instance.EternalDuraBank)
                        ___lifetimeRemaining -= dt;
                }
                else
                    __instance.Explode();

                return false;
            }
        }
    }
}
