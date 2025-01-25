using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace Instant_Switch
{
    public class Patches
    {        
        
        [HarmonyPatch(typeof(SelectTool), nameof(SelectTool.OnLeftClickDown))]
        public class SelectTool_OnLeftClickDown_Patch
        {
            private static List<KSelectable> kSelectables = new List<KSelectable>();
            public static bool Prefix(SelectTool __instance)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    kSelectables.Clear();
                    __instance.GetSelectablesUnderCursor(kSelectables);
                    foreach (KSelectable selectable in kSelectables)
                    {
                        if (!selectable.TryGetComponent<KPrefabID>(out KPrefabID kPrefabID))
                            continue;
                        Tag ID = kPrefabID.PrefabID();

                        //Signal Switch 
                        if (ID == LogicSwitchConfig.ID)
                        {
                            if (selectable.TryGetComponent<LogicSwitch>(out LogicSwitch logicSwitch))
                            {
                                logicSwitch.ToggledByPlayer();
                                if (SpeedControlScreen.Instance.IsPaused)
                                {
                                    if (logicSwitch.TryGetComponent<KBatchedAnimController>(out KBatchedAnimController animController))
                                    {
                                        animController.Play((HashedString)(logicSwitch.IsSwitchedOn ? "on" : "off"));
                                    }

                                }
                                return false;
                            }
                        }
                        //Power Shutoff 
                        if (ID == LogicPowerRelayConfig.ID)
                        {
                            if (selectable.TryGetComponent<CircuitSwitch>(out CircuitSwitch circuitSwitch))
                            {
                                circuitSwitch.ToggledByPlayer();
                                if (SpeedControlScreen.Instance.IsPaused)
                                {
                                    if (circuitSwitch.TryGetComponent<KBatchedAnimController>(out KBatchedAnimController animController))
                                    {
                                        animController.Play((HashedString)(circuitSwitch.IsSwitchedOn ? "on" : "off"));
                                    }
                                }
                                return false;
                            }
                        }
                        //Switch
                        if (ID == SwitchConfig.ID)
                        {
                            if (selectable.TryGetComponent<CircuitSwitch>(out CircuitSwitch circuitSwitch))
                            {
                                circuitSwitch.ToggledByPlayer();
                                if (SpeedControlScreen.Instance.IsPaused)
                                {
                                    if (circuitSwitch.TryGetComponent<KBatchedAnimController>(out KBatchedAnimController animController))
                                    {
                                        animController.Play((HashedString)(circuitSwitch.IsSwitchedOn ? "on" : "off"));
                                    }
                                }
                                return false;

                            }
                        }                      
                    };
                }
                return true;
            }
        }




    }
}

