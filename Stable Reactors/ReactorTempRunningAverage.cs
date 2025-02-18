using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Database;
using HarmonyLib;
using KSerialization;
using UnityEngine;

namespace Stable_Reactors
{
    public class ReactorTempRunningAverage : KMonoBehaviour
    {        
        public static float MeltdownTempThreshold = Config.Instance.MeltdownTempThresh;
        [Serialize]
        public float AverageTemp;
        [Serialize]
        private List<float> TempHistory = new List<float>();//updated every 200ms

        public float UpdateAndReturnAverageTemp(float temp)
        {
            TempHistory.Add(temp);
            while (TempHistory.Count > Config.Instance.TempHistoryEntriesToKeep)
                TempHistory.RemoveAt(0);
            AverageTemp = TempHistory.Average();
            return AverageTemp;
        }        

        [HarmonyPatch(typeof(NuclearReactorConfig), nameof(NuclearReactorConfig.ConfigureBuildingTemplate))]
        public class NuclearReactorConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddComponent<ReactorTempRunningAverage>();
            }
        }

        [HarmonyPatch(typeof(Reactor), nameof(Reactor.GetDescriptors))]
        public class Reactor_GetDescriptors_Patch
        {
            public static void Postfix(ref List<Descriptor> __result, Reactor __instance, GameObject go)
            {
                if (__instance.TryGetComponent(out ReactorTempRunningAverage component))
                    __result.Add(new Descriptor("Average Fuel Temp: " + Math.Round(component.AverageTemp - 273.15f, 2) + " °C", "Over the last " + (Config.Instance.TempHistoryEntriesToKeep / 5f) + " seconds"));
                __result.Add(new Descriptor("Meltdown Threshold: " + (Config.Instance.MeltdownTempThresh - 273.15f) + " °C", ""));
            }
        }

        [HarmonyPatch(typeof(Reactor.States), "<InitializeStates>b__14_7")]
        public class Reactor__Replace_Method_Maximum_Reflection
        {
            public static MethodInfo TransferFuel = AccessTools.Method(typeof(Reactor), "TransferFuel");
            public static MethodInfo TransferCoolant = AccessTools.Method(typeof(Reactor), "TransferCoolant");
            public static MethodInfo React = AccessTools.Method(typeof(Reactor), "React");
            public static MethodInfo UpdateCoolantStatus = AccessTools.Method(typeof(Reactor), "UpdateCoolantStatus");
            public static MethodInfo UpdateVentStatus = AccessTools.Method(typeof(Reactor), "UpdateVentStatus");
            public static MethodInfo DumpSpentFuel = AccessTools.Method(typeof(Reactor), "DumpSpentFuel");
            public static MethodInfo GetActiveCoolant = AccessTools.Method(typeof(Reactor), "GetActiveCoolant");
            public static MethodInfo Cool = AccessTools.Method(typeof(Reactor), "Cool");
            public static MethodInfo GetActiveFuel = AccessTools.Method(typeof(Reactor), "GetActiveFuel");

            public static FieldInfo fuelDeliveryEnabled = AccessTools.Field(typeof(Reactor), "fuelDeliveryEnabled");
            public static FieldInfo temperatureMeter = AccessTools.Field(typeof(Reactor), "temperatureMeter");
            public static FieldInfo meterFrameScaleHack = AccessTools.Field(typeof(Reactor), "meterFrameScaleHack");
            public static FieldInfo supplyStorage = AccessTools.Field(typeof(Reactor), "supplyStorage");
            public static FieldInfo reactionStorage = AccessTools.Field(typeof(Reactor), "reactionStorage");
            public static FieldInfo wasteStorage = AccessTools.Field(typeof(Reactor), "wasteStorage");

            public static bool Prefix(Reactor.StatesInstance smi, float dt, Reactor.States __instance)
            {
                var voidParams = new object[] { };
                var dtParam = new object[] { dt };
                TransferFuel.Invoke(smi.master, voidParams);
                TransferCoolant.Invoke(smi.master, voidParams);
                React.Invoke(smi.master, dtParam);
                UpdateCoolantStatus.Invoke(smi.master, voidParams);
                UpdateVentStatus.Invoke(smi.master, voidParams);
                DumpSpentFuel.Invoke(smi.master, voidParams);                
                if (!(bool)fuelDeliveryEnabled.GetValue(smi.master))
                {
                    smi.master.refuelStausHandle = smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled);
                }
                else
                {
                    smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled);
                    smi.master.refuelStausHandle = Guid.Empty;
                }
                if ((PrimaryElement)GetActiveCoolant.Invoke(smi.master, voidParams) != null)
                {
                    Cool.Invoke(smi.master, dtParam);
                }
                PrimaryElement activeFuel = (PrimaryElement) GetActiveFuel.Invoke(smi.master, voidParams);
                if (activeFuel != null)
                {
                    ((MeterController)temperatureMeter.GetValue(smi.master)).SetPositionPercent(Mathf.Clamp01(activeFuel.Temperature / MeltdownTempThreshold) / (float)meterFrameScaleHack.GetValue(smi.master));
                    
                    if(!smi.master.TryGetComponent(out ReactorTempRunningAverage component))
                    {
                        Debug.LogWarning("[StableReactor] Reactor has no ReactorTempRunningAverage component\nCrash inbound.");
                    }                    
                    if (component.UpdateAndReturnAverageTemp(activeFuel.Temperature) >= MeltdownTempThreshold)
                    {
                        smi.sm.meltdownMassRemaining.Set(10f + ((Storage)supplyStorage.GetValue(smi.master)).MassStored() + ((Storage)reactionStorage.GetValue(smi.master)).MassStored() + ((Storage)wasteStorage.GetValue(smi.master)).MassStored(), smi);
                        ((Storage)supplyStorage.GetValue(smi.master)).ConsumeAllIgnoringDisease();
                        ((Storage)reactionStorage.GetValue(smi.master)).ConsumeAllIgnoringDisease();
                        ((Storage)wasteStorage.GetValue(smi.master)).ConsumeAllIgnoringDisease();
                        smi.GoTo(__instance.meltdown.pre);
                        Debug.Log("Reactor Meltdown with an average fuel temp of: " + component.AverageTemp + " Kelvin");
                        for (int i = 0; i < component.TempHistory.Count; i++)
                        {
                            Debug.Log(component.TempHistory[i]);
                        }
                    }
                    else if (activeFuel.Mass <= 0.25f)
                    {
                        smi.GoTo(__instance.off_pre);
                        ((MeterController)temperatureMeter.GetValue(smi.master)).SetPositionPercent(0f);
                    }
                }
                else
                {
                    smi.GoTo(__instance.off_pre);
                    ((MeterController)temperatureMeter.GetValue(smi.master)).SetPositionPercent(0f);
                }

                return false;
            }
        }

        



















        /*//[HarmonyPatch(typeof(Reactor.States), "<InitializeStates>b__14_7")]
        public class Uhh_Yea_Transpiler
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                Debug.Log("Reactor Transpiler Begin");

                foreach (CodeInstruction inst in instructions)
                {
                    Debug.Log(inst);
                }               
                var primaryElement_TemperatureGetter = typeof(PrimaryElement).GetMethod(nameof(PrimaryElement.Temperature));
                var codeMatcher = new CodeMatcher(instructions);
                var codeMatch = new CodeMatch[]
                {
                    new CodeMatch(OpCodes.Ldloc_0),
                    new CodeMatch(OpCodes.Callvirt, primaryElement_TemperatureGetter),
                    new CodeMatch(OpCodes.Ldc_R4, 3000f),
                    new CodeMatch(OpCodes.Blt_Un)
                };
                codeMatcher.MatchStartForward(codeMatch);
                if (codeMatcher.Remaining == 0)
                {
                    Debug.Log("Transpiler Failed, CodeMatch not found");
                    return instructions;
                }
                                
                var newInstructionBlock = new CodeInstruction[]
                {
                    //stuff

                };
                codeMatcher.Insert(newInstructionBlock);
                Debug.Log("Transpiler succeeded?");
                return codeMatcher.Instructions();
            }
        }*/


    }
}
