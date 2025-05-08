/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class RocketControlStationRemote
    {
        *//*[HarmonyPatch(typeof(RocketControlStationConfig), nameof(RocketControlStationConfig.DoPostConfigureComplete))]
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
        }*//*


        // inactive pilot module on load of save game causing 50% speed reduction
    }
}
*/