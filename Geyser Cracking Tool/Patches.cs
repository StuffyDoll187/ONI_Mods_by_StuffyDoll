using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Geyser_Cracking_Tool
{
    internal class Patches
    {
        //[HarmonyPatch(typeof(ToolMenu), "CreateBasicTools")]
        public class ToolMenu_CreateBasicTools_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var targetMethod = AccessTools.Method(typeof(ToolMenu), "CreateBasicTools");
                var m_postfix = AccessTools.Method(typeof(ToolMenu_CreateBasicTools_Patch), "Postfix");
                harmony.Patch(targetMethod, postfix: new HarmonyMethod(m_postfix));
            }

            public static void Postfix(ToolMenu __instance)
            {
                 
                __instance.basicTools.Add(ToolMenu.CreateToolCollection("GeyserCrackingTool", "cancel", Action.BuildMenuUp, "GeyserCrackingTool", "Marks All Geysers in an area for Cracking", false));

            }
        }


        //[HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public class PlayerController_OnPrefabInit_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var targetMethod = AccessTools.Method(typeof(PlayerController), "OnPrefabInit");
                var m_postfix = AccessTools.Method(typeof(PlayerController_OnPrefabInit_Patch), "Postfix");
                harmony.Patch(targetMethod, postfix: new HarmonyMethod(m_postfix));
            }
            public static void Postfix(PlayerController __instance)
            {
                var interfaceTools = new List<InterfaceTool>(__instance.tools);
                var geyserCrackingTool = new GameObject(nameof(GeyserCrackingTool), typeof(GeyserCrackingTool));
                var tool = geyserCrackingTool.AddComponent<GeyserCrackingTool>();
                geyserCrackingTool.transform.SetParent(__instance.gameObject.transform);
                geyserCrackingTool.SetActive(true);
                geyserCrackingTool.SetActive(false);
                interfaceTools.Add(tool);
                __instance.tools = interfaceTools.ToArray();
            }
        }
        
        //[HarmonyPatch(typeof(Db), "Initialize")]
        public class DbInitialize_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var targetMethod = AccessTools.Method(typeof(Db), "Initialize");
                var m_postfix = AccessTools.Method(typeof(DbInitialize_Patch), "Postfix");
                harmony.Patch(targetMethod, postfix: new HarmonyMethod(m_postfix));
            }
            private static void Postfix()
            {
                Strings.Add("STRINGS.INPUT_BINDINGS.TOOL.BUILDMENUUP", "Geyser Cracking Tool");
            }
        }

        //[HarmonyPatch(typeof(GameInputMapping), nameof(GameInputMapping.SetDefaultKeyBindings))]
        public class GameInputMapping_SetDefaultKeyBindings_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var targetMethod = AccessTools.Method(typeof(GameInputMapping), nameof(GameInputMapping.SetDefaultKeyBindings));
                var m_prefix = AccessTools.Method(typeof(GameInputMapping_SetDefaultKeyBindings_Patch), "Prefix");
                harmony.Patch(targetMethod, prefix: new HarmonyMethod(m_prefix));
            }
            private static void Prefix(ref BindingEntry[] default_keybindings)
            {
                
                List<BindingEntry> defaultBindings = default_keybindings.ToList();
                BindingEntry binding = new BindingEntry("Tool", GamepadButton.NumButtons, KKeyCode.G, Modifier.Shift, Action.BuildMenuUp);
                defaultBindings.Add(binding);
                default_keybindings = defaultBindings.ToArray();
            }
        }
    }
}
