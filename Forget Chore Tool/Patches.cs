using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Forget_Chore_Tool
{
    public class Patches
    {
        [HarmonyPatch(typeof(ToolMenu), "CreateBasicTools")]
        public class ToolMenu_CreateBasicTools_Patch
        {
            public static void Postfix(ToolMenu __instance)
            {
                __instance.basicTools.Add(ToolMenu.CreateToolCollection("ForgetChoreTool", "cancel", Action.BuildMenuKeyB, "ForgetChoreTool", "Any Duplicants in the area forget what they are doing for a moment.", false));
                
            }
        }


        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public class PlayerController_OnPrefabInit_Patch
        {
            public static void Postfix(PlayerController __instance)
            {
                var interfaceTools = new List<InterfaceTool>(__instance.tools);
                var forgetChoreTool = new GameObject(nameof(ForgetChoreTool), typeof(ForgetChoreTool));
                var tool = forgetChoreTool.AddComponent<ForgetChoreTool>();
                forgetChoreTool.transform.SetParent(__instance.gameObject.transform);
                forgetChoreTool.SetActive(true);
                forgetChoreTool.SetActive(false);
                interfaceTools.Add(tool);
                __instance.tools = interfaceTools.ToArray();
            }
        }


    }
}
