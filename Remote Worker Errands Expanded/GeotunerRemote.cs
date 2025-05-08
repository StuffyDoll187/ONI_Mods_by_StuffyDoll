using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Remote_Worker_Errands_Expanded
{
    class GeotunerRemote : RemoteWorkable//ManuallySetRemoteWorkTargetComponent
    {
        public Chore chore;
        public override Chore RemoteDockChore => chore;


        [HarmonyPatch(typeof(GeoTunerConfig), nameof(GeoTunerConfig.ConfigureBuildingTemplate))]
        public class GeoTunerConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddComponent<GeotunerRemote>();
            }
        }
        
        /// <summary>
        /// Skip because this crashes if you touch anything related to the GeoTuner. Even an empty Postfix to CreateResearchChore in an empty mod caused failure to find the sound. Cursed.
        /// </summary>
        [HarmonyPatch(typeof(GeoTuner), nameof(GeoTuner.TriggerSoundsForGeyserChange))]
        public class GeoTuner_TriggerSounds_Patch
        {
            public static bool Prefix()
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(GeoTuner), "CreateResearchChore")]
        public class GeoTuner_CreateResearchChore_Patch
        {
            public static void Postfix(GeoTuner.Instance smi, ref Chore __result)
            {
                var remote = smi.master.GetComponent<GeotunerRemote>();                
                remote.chore = __result;
            }
        }

        /*[HarmonyPatch(typeof(GeoTuner), nameof(GeoTuner.InitializeStates))]
        public class GeoTuner_InitializeStates_Patch
        {
            
            public static bool Prefix(out StateMachine.BaseState default_state,GeoTuner __instance)
            {
                default_state = __instance.operational;
                __instance.serializable = StateMachine.SerializeType.ParamsOnly;
                __instance.root.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType));
                __instance.nonOperational.DefaultState(__instance.nonOperational.off).OnSignal(__instance.geyserSwitchSignal, __instance.nonOperational.switchingGeyser).Enter(smi => smi.RefreshLogicOutput()).TagTransition(GameTags.Operational, __instance.operational);
                __instance.nonOperational.off.PlayAnim(GeoTuner.OffAnimName);
                __instance.nonOperational.switchingGeyser.QueueAnim(GeoTuner.anim_switchGeyser_down).OnAnimQueueComplete(__instance.nonOperational.down);
                __instance.nonOperational.down.PlayAnim(GeoTuner.anim_switchGeyser_up).QueueAnim(GeoTuner.OffAnimName).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange));
                __instance.operational.PlayAnim(GeoTuner.OnAnimName).Enter(smi => smi.RefreshLogicOutput()).DefaultState(__instance.operational.idle).TagTransition(GameTags.Operational, __instance.nonOperational, true);
                __instance.operational.idle.ParamTransition<GameObject>(__instance.AssignedGeyser, __instance.operational.geyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNotNull).ParamTransition<GameObject>(__instance.AssignedGeyser, __instance.operational.noGeyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNull);
                __instance.operational.noGeyserSelected.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerNoGeyserSelected).ParamTransition<GameObject>(__instance.AssignedGeyser, __instance.operational.geyserSelected.switchingGeyser, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNotNull).Enter(smi => smi.RefreshLogicOutput()).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.DropStorage)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshStorageRequirements)).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).QueueAnim(GeoTuner.anim_switchGeyser_down).OnAnimQueueComplete(__instance.operational.noGeyserSelected.idle);
                __instance.operational.noGeyserSelected.idle.PlayAnim(GeoTuner.anim_switchGeyser_up).QueueAnim(GeoTuner.OnAnimName).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange));
                __instance.operational.geyserSelected.DefaultState(__instance.operational.geyserSelected.idle).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoTunerGeyserStatus).ParamTransition<GameObject>(__instance.AssignedGeyser, __instance.operational.noGeyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNull).OnSignal(__instance.geyserSwitchSignal, __instance.operational.geyserSelected.switchingGeyser).Enter(smi => smi.RefreshLogicOutput());
                __instance.operational.geyserSelected.idle.ParamTransition<bool>(__instance.hasBeenWorkedByResearcher, __instance.operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsTrue).ParamTransition<bool>(__instance.hasBeenWorkedByResearcher, __instance.operational.geyserSelected.researcherInteractionNeeded, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsFalse);
                __instance.operational.geyserSelected.switchingGeyser.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.DropStorageIfNotMatching)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshStorageRequirements)).Enter(smi => smi.RefreshLogicOutput()).QueueAnim(GeoTuner.anim_switchGeyser_down).OnAnimQueueComplete(__instance.operational.geyserSelected.switchingGeyser.down);
                __instance.operational.geyserSelected.switchingGeyser.down.QueueAnim(GeoTuner.anim_switchGeyser_up).QueueAnim(GeoTuner.OnAnimName).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType))*//*.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange))*//*.ScheduleActionNextFrame("Switch Animation Completed", smi => smi.GoTo(__instance.operational.geyserSelected.idle));
                __instance.operational.geyserSelected.researcherInteractionNeeded.EventTransition(GameHashes.UpdateRoom, __instance.operational.geyserSelected.researcherInteractionNeeded.blocked, smi => !GeoTuner.WorkRequirementsMet(smi)).EventTransition(GameHashes.UpdateRoom, __instance.operational.geyserSelected.researcherInteractionNeeded.available, new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Transition.ConditionCallback(GeoTuner.WorkRequirementsMet)).EventTransition(GameHashes.OnStorageChange, __instance.operational.geyserSelected.researcherInteractionNeeded.blocked, smi => !GeoTuner.WorkRequirementsMet(smi)).EventTransition(GameHashes.OnStorageChange, __instance.operational.geyserSelected.researcherInteractionNeeded.available, new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Transition.ConditionCallback(GeoTuner.WorkRequirementsMet)).ParamTransition<bool>(__instance.hasBeenWorkedByResearcher, __instance.operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsTrue).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer));                
                __instance.operational.geyserSelected.researcherInteractionNeeded.blocked.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded).DoNothing();

                //__instance.operational.geyserSelected.researcherInteractionNeeded.available.DefaultState(__instance.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe).ToggleRecurringChore(new Func<GeoTuner.Instance, Chore>(__instance.CreateResearchChore)).WorkableCompleteTransition(smi => smi.workable, __instance.operational.geyserSelected.researcherInteractionNeeded.completed);
                __instance.operational.geyserSelected.researcherInteractionNeeded.available.DefaultState(__instance.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe).ToggleRecurringChore(new Func<GeoTuner.Instance, Chore>(__instance.CreateResearchChore), new Action<GeoTuner.Instance, Chore>(SetRemoteChore)).WorkableCompleteTransition(smi => smi.workable, __instance.operational.geyserSelected.researcherInteractionNeeded.completed);

                __instance.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded).WorkableStartTransition(smi => smi.workable, __instance.operational.geyserSelected.researcherInteractionNeeded.available.inProgress);
                __instance.operational.geyserSelected.researcherInteractionNeeded.available.inProgress.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchInProgress).WorkableStopTransition(smi => smi.workable, __instance.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe);
                __instance.operational.geyserSelected.researcherInteractionNeeded.completed.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.OnResearchCompleted));
                __instance.operational.geyserSelected.broadcasting.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerBroadcasting).Toggle("Tuning", new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ApplyTuning), new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RemoveTuning));
                __instance.operational.geyserSelected.broadcasting.onHold.PlayAnim(GeoTuner.BroadcastingOnHoldAnimationName).UpdateTransition(__instance.operational.geyserSelected.broadcasting.active, (smi, dt) => !GeoTuner.GeyserExitEruptionTransition(smi, dt));
                __instance.operational.geyserSelected.broadcasting.active.Toggle("EnergyConsumption", smi => smi.operational.SetActive(true), smi => smi.operational.SetActive(false)).Toggle("BroadcastingAnimations", new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.PlayBroadcastingAnimation), new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.StopPlayingBroadcastingAnimation)).Update(new System.Action<GeoTuner.Instance, float>(GeoTuner.ExpirationTimerUpdate)).UpdateTransition(__instance.operational.geyserSelected.broadcasting.onHold, new Func<GeoTuner.Instance, float, bool>(GeoTuner.GeyserExitEruptionTransition)).ParamTransition<float>(__instance.expirationTimer, __instance.operational.geyserSelected.broadcasting.expired, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsLTEZero);
                __instance.operational.geyserSelected.broadcasting.expired.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).ScheduleActionNextFrame("Expired", smi => smi.GoTo(__instance.operational.geyserSelected.researcherInteractionNeeded));
                return false;
            }
            
            public static void SetRemoteChore(GeoTuner.Instance smi, Chore chore)
            {
                smi.master.GetComponent<GeotunerRemote>().SetChore(chore);
            }
        }*/
    }

}
