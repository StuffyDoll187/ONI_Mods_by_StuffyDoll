using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using STRINGS;
using System.Reflection;
using Klei.AI;

namespace Interstellar_Remote_Workers
{
    public class Patches
    {
        [HarmonyPatch(typeof(RemoteWorkTerminalSidescreen), "RefreshOptions")]
        public class RemoteWorkTerminalSidescreen_RefreshOptions_Patch
        {            
            public static MethodInfo SetRow = AccessTools.Method(typeof(RemoteWorkTerminalSidescreen), "SetRow");

            public static bool Prefix(RemoteWorkTerminalSidescreen __instance)
            {
                int idx = 0;
                int num = idx + 1;
                SetRow.Invoke(__instance, new object[] { idx, (string)UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.NOTHING_SELECTED, Assets.GetSprite("action_building_disabled"), null });
              
                List<RemoteWorkerDock> docks = new List<RemoteWorkerDock>();

                foreach (WorldContainer world in ClusterManager.Instance.WorldContainers)
                    docks.AddRange(Components.RemoteWorkerDocks.GetItems(world.id));

                foreach (RemoteWorkerDock dock in docks)
                    SetRow.Invoke(__instance, new object[] { num++, UI.StripLinkFormatting(dock.GetProperName()), Def.GetUISprite(dock.gameObject)?.first, dock });

                for (int i = num; i < __instance.rowContainer.childCount; ++i)
                    __instance.rowContainer.GetChild(i).gameObject.SetActive(value: false);

                return false;
            }
        }

        

        /*[HarmonyPatch(typeof(RocketControlStationConfig), nameof(RocketControlStationConfig.DoPostConfigureComplete))]
        public class RocketControlStationConfig_DoPostConfigureComplete_Patch
        {
            public static void Postfix(ref GameObject go)
            {
                var cmp = go.AddComponent<ManuallySetRemoteWorkTargetComponent>();                                     
            }
        }
        [HarmonyPatch(typeof(RocketControlStation.States), "CreateChore")]
        public class RocketControlStationStates_CreateChore_Patch
        {
            public static void Postfix(Chore __result, RocketControlStation.States __instance, ref RocketControlStation.StatesInstance smi)
            {
                var cmp = __result.target.GetComponent<ManuallySetRemoteWorkTargetComponent>();
                cmp.SetChore(__result);                
            }
        }
        [HarmonyPatch(typeof(RocketControlStation.States), "CreateLaunchChore")]
        public class RocketControlStationStates_CreateLaunchChore_Patch
        {
            public static void Postfix(Chore __result, RocketControlStation.States __instance, ref RocketControlStation.StatesInstance smi)
            {
                var cmp = __result.target.GetComponent<ManuallySetRemoteWorkTargetComponent>();
                cmp.SetChore(__result);                
            }
        }
        [HarmonyPatch(typeof(ConditionPilotOnBoard), nameof(ConditionPilotOnBoard.EvaluateCondition))]
        public class ConditionPilotOnBoard_EvaluateCondition_Patch
        {
            public static void Postfix(ref ProcessCondition.Status __result)
            {
                __result = ProcessCondition.Status.Ready;
            }
        }

        [HarmonyPatch(typeof(RocketControlStation.StatesInstance), nameof(RocketControlStation.StatesInstance.SetPilotSpeedMult))]
        public class RocketControlStationStatesInstance_SetPilotSpeedMult_Patch
        {
            public static bool Prefix(WorkerBase pilot, RocketControlStation.StatesInstance __instance)
            {
                var remoteWorker = pilot.GetComponent<RemoteWorker>();
                AttributeConverterInstance converter = remoteWorker.GetAttributeConverter("PilotingSpeed");
                float a = 1f + converter.Evaluate();
                __instance.pilotSpeedMult = Mathf.Max(a, 0.1f);               
                return false;
            }
        }*/


        // inactive pilot module on load of save game causing 50% speed reduction

    }
}
