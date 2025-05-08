using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System;
using TUNING;
using Klei.AI;
using System.Reflection;
using STRINGS;

namespace Remote_Worker_Errands_Expanded
{
    public class Patches
    {

        /// <summary>
        /// Prevents a crash when Remote Worker tries to Dig.
        /// </summary>
        [HarmonyPatch(typeof(RemoteWorkerConfig), nameof(RemoteWorkerConfig.OnPrefabInit))]
        public class RemoteWorkerConfig_OnPrefabInit_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddComponent<SnapOn>();                
            }
        }

        /// <summary>
        /// Changes NavGrid. Might not since if a Remote Worker deconstructs the tile they are standing on they don't fall even though they have CreatureFallMonitor. Reload fixes.
        /// </summary>
        [HarmonyPatch(typeof(RemoteWorkerConfig), nameof(RemoteWorkerConfig.CreatePrefab))]
        public class RemoteWorkerConfig_CreatePrefab_Patch
        {
            public static void Postfix(ref GameObject __result)
            {
                if (__result.TryGetComponent(out Navigator navigator))
                {
                    navigator.NavGridName = "WalkerNavGrid1x2";
                    //navigator.NavGridName = "MinionNavGrid";
                    //navigator.NavGridName = "FlyerNavGrid1x2";
                    //navigator.CurrentNavType = NavType.Hover;
                    //navigator.NavGridName = "SwimmerNavGrid";
                    //navigator.CurrentNavType = NavType.Swim;
                }
                
                //__result.GetDef<CreatureFallMonitor.Def>().canSwim = true;
                

            }
        }
        

        /// <summary>
        /// Changes Area that the Remote Worker will look for errands.
        /// </summary>
        [HarmonyPatch(typeof(RemoteWorkerDock), "canWork")]
        public class RemoteWorkerDock_canWork_Patch
        {
            public static void Postfix(IRemoteDockWorkTarget provider, ref bool __result, RemoteWorkerDock __instance)
            {
                if (true)//(provider.RemoteDockChore.choreType == Db.Get().ChoreTypes.Build)
                {
                    int x1;
                    int y1;
                    Grid.CellToXY(Grid.PosToCell(__instance), out x1, out y1);
                    int x2;
                    int y2;
                    Grid.CellToXY(provider.Approachable.GetCell(), out x2, out y2);
                    __result = ( (y2 - y1 <= 4) && (y1 - y2 <= 1) && Math.Abs(x1 - x2) <= 12);
                }
            }
        }


        /// <summary>
        /// Original CropPicked method assumes worker is Duplicant. Fails to get Attribute Converter and crashes.
        /// </summary>
        [HarmonyPatch(typeof(SeedProducer), nameof(SeedProducer.CropPicked))]
        public class SeedProducer_CropPicked_Patch
        {
            public static MethodInfo ProduceSeed = AccessTools.Method(typeof(SeedProducer), "ProduceSeed");
            public static bool Prefix(object data, SeedProducer __instance)
            {                
                if (__instance.seedInfo.productionType != SeedProducer.ProductionType.Harvest)
                    return false;
                WorkerBase completedBy = __instance.GetComponent<Harvestable>().completed_by;
                float num = 0.1f;
                if (completedBy != null)
                {
                    AttributeConverterInstance converter;
                    if (completedBy.TryGetComponent(out RemoteWorker remoteWorker))
                        converter = remoteWorker.GetAttributeConverter(Db.Get().AttributeConverters.SeedHarvestChance.Id);
                    else
                        converter = completedBy.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance);
                    num += converter.Evaluate();
                }
                int units = (double)UnityEngine.Random.Range(0.0f, 1f) <= (double)num ? 1 : 0;
                if (units <= 0)
                    return false;
                object go = ProduceSeed.Invoke(__instance, new object[] { __instance.seedInfo.seedId, units, true });
                ((GameObject)go).Trigger((int)GameHashes.WorkableEntombOffset, completedBy);
                return false;
            }
        }



        /// <summary>
        /// Allows Flydo and Auto-Sweeper to ignore skill requirements.
        /// </summary>
        [HarmonyPatch(typeof(ChorePreconditions), MethodType.Constructor)]
        public class ChorePreconditions_Constructor_Patch
        {
            public static void Postfix(ChorePreconditions __instance)
            {
                
                var precondition = new Chore.Precondition();
                precondition.id = nameof(__instance.HasSkillPerk);
                precondition.description = (string)DUPLICANTS.CHORES.PRECONDITIONS.HAS_SKILL_PERK;
                precondition.fn = (ref Chore.Precondition.Context context, object data) =>
                {
                    if (context.consumerState.worker.IsFetchDrone() || context.consumerState.hasSolidTransferArm)
                        return true;
                    MinionResume resume = context.consumerState.resume;
                    if (!(bool)resume)
                        return false;
                    switch (data)
                    {
                        case Database.SkillPerk _:
                            Database.SkillPerk perk = data as Database.SkillPerk;
                            return resume.HasPerk(perk);
                        case HashedString perkId3:
                            return resume.HasPerk(perkId3);
                        case string _:
                            HashedString perkId2 = (HashedString)(string)data;
                            return resume.HasPerk(perkId2);
                        default:
                            return false;
                    }
                };
                precondition.canExecuteOnAnyThread = true;
                __instance.HasSkillPerk = precondition;
            }
        }


        [HarmonyPatch(typeof(StandardChoreBase), nameof(StandardChoreBase.AddPrecondition))]
        public class StandardchoreBase_AddPrecondition_Patch
        {
            public static Chore.PreconditionFn Fn = new Chore.PreconditionFn((ref Chore.Precondition.Context context, object data) =>
            {
                FetchChore chore = (FetchChore)context.chore;
                Pickupable pickup = (Pickupable)context.data;
                bool flag;
                if (pickup == null)
                {                    
                    pickup = chore.FindFetchTarget(context.consumerState);
                    Debug.Log("pickup was null: " + pickup);
                    flag = pickup != null;
                }
                else
                {
                    flag = FetchManager.IsFetchablePickup(pickup, chore, context.consumerState.storage);
                    Debug.Log("pickup not null, isFetchablePickup: " + flag);
                }
                    
                if (flag)
                {
                    if (pickup == null)
                    {
                        Debug.Log(string.Format("Failed to find fetch target for {0}", chore.destination));
                        return false;
                    }
                    context.data = pickup;
                    if (context.consumerState.worker.IsFetchDrone())
                    {
                        Debug.Log("worker is a fetch drone");
                        int cost;
                        if ((pickup.targetWorkable == null || pickup.targetWorkable.GetComponent<Pickupable>() != null) && context.consumerState.consumer.GetNavigationCost(pickup, out cost))
                        {
                            context.cost += cost;
                            return true;
                        }
                    }
                    else
                    {
                        Debug.Log("else");
                        int cost;
                        if (context.consumerState.consumer.GetNavigationCost(pickup, out cost))
                        {
                            context.cost += cost;
                            return true;
                        }
                    }
                }
                Debug.Log("fetchable is not a pickup");
                return false;
            });
            public static void Prefix(Chore.Precondition precondition)
            {
                if (precondition.id == FetchChore.IsFetchTargetAvailable.id)
                {
                    precondition.fn = Fn;
                }
            }
        }

        /*[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public class Db_Init_Patch
        {
            public static void Postfix()
            {


                FetchChore.IsFetchTargetAvailable = new Chore.Precondition()
                {
                    id = nameof(FetchChore.IsFetchTargetAvailable),
                    description = (string)DUPLICANTS.CHORES.PRECONDITIONS.IS_FETCH_TARGET_AVAILABLE,
                    fn = (ref Chore.Precondition.Context context, object data) =>
                    {
                        FetchChore chore = (FetchChore)context.chore;
                        Pickupable pickup = (Pickupable)context.data;
                        bool flag;
                        if (pickup == null)
                        {
                            pickup = chore.FindFetchTarget(context.consumerState);
                            flag = pickup != null;
                        }
                        else
                            flag = FetchManager.IsFetchablePickup(pickup, chore, context.consumerState.storage);
                        if (flag)
                        {
                            if (pickup == null)
                            {
                                Debug.Log(string.Format("Failed to find fetch target for {0}", chore.destination));
                                return false;
                            }
                            context.data = pickup;
                            if (context.consumerState.worker.IsFetchDrone())
                            {
                                int cost;
                                if ((pickup.targetWorkable == null || pickup.targetWorkable.GetComponent<Pickupable>() != null) && context.consumerState.consumer.GetNavigationCost(pickup, out cost))
                                {
                                    context.cost += cost;
                                    return true;
                                }
                            }
                            else
                            {
                                int cost;
                                if (context.consumerState.consumer.GetNavigationCost(pickup, out cost))
                                {
                                    context.cost += cost;
                                    return true;
                                }
                            }
                        }
                        return false;
                    }
                };

            }
        }*/
    }
}
