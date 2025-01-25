using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Engies_Tuneup_Inactivity_Refund
{
    public class Patches
    {        
        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {                
                Db.Get().effects.Get("PowerTinker").duration = Mathf.Max( (float) Config.Instance.powerTinkerDuration, 1f);
            }
        }
                    
        public class Tinkerable_MakePowerTinkerable_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var original = typeof(Tinkerable).GetMethod(nameof(Tinkerable.MakePowerTinkerable));
                var postfix = typeof(Tinkerable_MakePowerTinkerable_Patch).GetMethod(nameof(Postfix));
                harmony.Patch(original, null, new HarmonyMethod(postfix), null, null);
            }
            public static void Postfix(ref GameObject prefab)
            {                
                    prefab.AddComponent<EngiesTuneupRefundTracker>();
            }            
        }
        [HarmonyPatch(typeof(Tinkerable), "OnCompleteWork")]
        public class Tinkerable_OnCompleteWork_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var original = typeof(Tinkerable).GetMethod("OnCompleteWork");
                var postfix = typeof(Tinkerable_OnCompleteWork_Patch).GetMethod(nameof(Postfix));
            }
            public static void Postfix(Tinkerable __instance)
            {
                if (__instance.TryGetComponent<EngiesTuneupRefundTracker>(out EngiesTuneupRefundTracker tracker))
                {
                    tracker.powerTinkerStartTime = GameClock.Instance.GetTime();
                }
            }
        }
    }
}
