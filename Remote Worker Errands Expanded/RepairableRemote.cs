using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Remote_Worker_Errands_Expanded
{
    class RepairableRemote : RemoteWorkable
    {
        public Chore chore;
        public override Chore RemoteDockChore => chore;


        [HarmonyPatch(typeof(Repairable.States), "CreateRepairChore")]
        public class Repairable_CreateRepairChore_Patch
        {
            public static void Postfix(Repairable.SMInstance smi, ref Chore __result)
            {
                var remote = smi.master.GetComponent<RepairableRemote>();
                remote.chore = __result;
            }
        }

        [HarmonyPatch(typeof(Repairable), nameof(Repairable.OnPrefabInit))]
        public class Constructable_OnPrefabInit_Patch
        {
            public static void Postfix(Repairable __instance)
            {
                __instance.FindOrAddComponent<RepairableRemote>();
            }
        }

        [HarmonyPatch(typeof(Repairable), "OnWorkTick")]
        public class Repairable_OnWorkTick_Patch
        {
            public static bool Prefix(WorkerBase worker, float dt,Repairable __instance, ref bool __result)
            {
                if (worker != null && worker.TryGetComponent(out RemoteWorker remoteWorker))
                {
                    float num1 = Mathf.Sqrt(__instance.GetComponent<PrimaryElement>().Mass);
                    float num2 = (float)((__instance.expectedRepairTime < 0.0 ? (double)num1 : __instance.expectedRepairTime) * 0.10000000149011612);
                    if (__instance.timeSpentRepairing >= (double)num2)
                    {
                        __instance.timeSpentRepairing -= num2;
                        var num3 = remoteWorker.GetAttributes().Get(Db.Get().Attributes.Machinery).GetTotalValue();
                        __instance.hp.Repair(Mathf.CeilToInt((10 + Math.Max(0, num3 * 10)) * 0.1f));
                        if (__instance.hp.HitPoints >= __instance.hp.MaxHitPoints)
                        {
                            __result = true;
                            return false;
                        }
                            
                    }
                    __instance.timeSpentRepairing += dt;
                    __result = false;
                    return false;
                }
                else
                    return true;

            }
        }

    }
}
