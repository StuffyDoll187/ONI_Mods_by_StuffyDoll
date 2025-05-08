using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Reflection;

namespace Starmap_Shenanigans
{
    public class ClusterGridEntityMoveToolPatches
    {

        
        [HarmonyPatch(typeof(DebugHandler), nameof(DebugHandler.SetDebugEnabled))]
        public class DebugHandler_SetDebugEnabled_Patch
        {
            public static FieldInfo m_selected = AccessTools.Field(typeof(ClusterMapSelectTool), "m_selected");
            public static void Postfix(bool debugEnabled)
            {
                if (debugEnabled)
                {
                    if (ClusterGridEntityMoveTool.Instance.isClusterMapScreenActive)
                    {
                        Add_Debug_Button.ClusterGridEntityMoveButton.ChangeState(1);
                        Add_Debug_Button.ClusterGridEntityMoveButtonTooltip.SetSimpleTooltip("");

                        var kSelectable = m_selected.GetValue(ClusterMapSelectTool.Instance);

                        if (kSelectable != null)
                        {                            
                            ClusterMapSelectTool.Instance.Select(null, true);
                            ClusterMapSelectTool.Instance.Select((KSelectable)kSelectable, true);
                        }
                    }
                        
                }
                else
                {
                    Add_Debug_Button.ClusterGridEntityMoveButton.ChangeState(0);
                    Add_Debug_Button.ClusterGridEntityMoveButtonTooltip.SetSimpleTooltip("Must Enable Debug Mode");
                    if (ClusterGridEntityMoveTool.Instance.isActive)
                    {
                        ClusterMapSelectTool.Instance.Activate();
                    }
                    var kSelectable = m_selected.GetValue(ClusterMapSelectTool.Instance);

                    if (kSelectable != null)
                    {
                        ClusterMapSelectTool.Instance.Select(null, true);
                        ClusterMapSelectTool.Instance.Select((KSelectable)kSelectable, true);
                    }

                }
            }
        }




        [HarmonyPatch(typeof(ClusterMapScreen), "OnShow")]
        public class ClusterMapScreen_OnShow_Patch
        {
            public static void Postfix(bool show)
            {
                

                ClusterGridEntityMoveTool.Instance.isClusterMapScreenActive = show;

                if (!DebugHandler.enabled)
                    return;

                if (show)
                    Add_Debug_Button.ClusterGridEntityMoveButton.ChangeState(1);
                else
                    Add_Debug_Button.ClusterGridEntityMoveButton.ChangeState(0);

            }
        }

        [HarmonyPatch(typeof(ClusterMapScreen), nameof(ClusterMapScreen.SelectHex))]
        public class ClusterMapScreen_SelectHex_Patch
        {
            public static bool Prefix()
            {
                if (!DebugHandler.enabled)
                    return true;
                
                if (ClusterGridEntityMoveTool.Instance.isActive)
                {
                    if (ClusterGridEntityMoveTool.clusterGridEntityToMove == null)
                    {
                        ClusterGridEntityMoveTool.StoreHoveredEntityAndCoord();
                    }
                    else
                    {
                        ClusterGridEntityMoveTool.SetNewLocation();
                    }
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(TopLeftControlScreen), "OnActivate")]
        public static class Add_Debug_Button
        {
            public static MultiToggle ClusterGridEntityMoveButton = null;
            public static ToolTip ClusterGridEntityMoveButtonTooltip = null;
            public static void OnClickClusterGridEntityMoveButton()
            {
                if (!DebugHandler.enabled ||!ClusterGridEntityMoveTool.Instance.isClusterMapScreenActive)
                {
                    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
                    return;
                }
                ClusterGridEntityMoveButton.ChangeState(ClusterGridEntityMoveButton.CurrentState == 2 ? 1 : 2);
                if (ClusterGridEntityMoveButton.CurrentState == 2)
                {
                    ClusterGridEntityMoveTool.Instance.Activate();
                    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));
                }
                else
                {
                    ClusterMapSelectTool.Instance.Activate();
                    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect"));
                }

            }
            public static void Postfix(TopLeftControlScreen __instance, MultiToggle ___sandboxToggle)
            {
                //if (!DebugHandler.enabled)
                    //return;

                var clusterGridEntityMoveButton = Util.KInstantiateUI(___sandboxToggle.gameObject, ___sandboxToggle.transform.parent.gameObject, true).transform;
                clusterGridEntityMoveButton.SetSiblingIndex(___sandboxToggle.transform.GetSiblingIndex() + 1);
                clusterGridEntityMoveButton.Find("FG").GetComponent<Image>().sprite = Assets.GetSprite("targetIcon");
                clusterGridEntityMoveButton.Find("Label").GetComponent<LocText>().text = "Move Starmap Entity";
                clusterGridEntityMoveButton.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 170f);
                clusterGridEntityMoveButton.TryGetComponent<MultiToggle>(out ClusterGridEntityMoveButton);
                clusterGridEntityMoveButton.TryGetComponent<ToolTip>(out ClusterGridEntityMoveButtonTooltip);
                ClusterGridEntityMoveButton.ChangeState(0);
                ClusterGridEntityMoveButton.onClick = (System.Action)Delegate.Combine(ClusterGridEntityMoveButton.onClick, new System.Action(OnClickClusterGridEntityMoveButton));
                ClusterGridEntityMoveButtonTooltip.SetSimpleTooltip(DebugHandler.enabled ? "" : "Must Enable Debug Mode");

                
            }

        }

        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public class PlayerController_OnPrefabInit_Patch
        {
            public static void Postfix(PlayerController __instance)
            {
                //if (!DebugHandler.enabled)
                    //return;

                var interfaceTools = new List<InterfaceTool>(__instance.tools);
                var clusterGridEntityMoveTool = new GameObject(nameof(ClusterGridEntityMoveTool), typeof(ClusterGridEntityMoveTool));
                var tool = clusterGridEntityMoveTool.AddComponent<ClusterGridEntityMoveTool>();
                clusterGridEntityMoveTool.transform.SetParent(__instance.gameObject.transform);
                clusterGridEntityMoveTool.SetActive(true);
                clusterGridEntityMoveTool.SetActive(false);
                interfaceTools.Add(tool);
                __instance.tools = interfaceTools.ToArray();
            }
        }
    
}
}
