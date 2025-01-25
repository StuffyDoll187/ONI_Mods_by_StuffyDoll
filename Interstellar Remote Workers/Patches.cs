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
                int num = 0;
                SetRow.Invoke(__instance, new object[] { num++, (string)UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.NOTHING_SELECTED, Assets.GetSprite("action_building_disabled"), null });
              
                List<RemoteWorkerDock> items = new List<RemoteWorkerDock>();
                
                var worldContainerList = ClusterManager.Instance.WorldContainers;
                for (int i = 0; i < worldContainerList.Count; i++)
                    items.AddRange(Components.RemoteWorkerDocks.GetItems(worldContainerList[i].id));
                

                foreach (RemoteWorkerDock item2 in items)
                {
                    string properName = item2.GetProperName();
                    GameObject item = item2.gameObject;
                    Tuple<Sprite, Color> uISprite = Def.GetUISprite(item);
                    Sprite first = uISprite.first;
                    SetRow.Invoke(__instance, new object[] { num++, UI.StripLinkFormatting(item2.GetProperName()), Def.GetUISprite(item2.gameObject)?.first, item2 });
                }

                for (int j = num; j < __instance.rowContainer.childCount; j++)
                {
                    __instance.rowContainer.GetChild(j).gameObject.SetActive(value: false);
                }

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
