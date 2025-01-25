/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace Chaos_Reigns
{
    internal class TelepadButton : KMonoBehaviour, ISidescreenButtonControl
    {
        [HarmonyPatch(typeof(Telepad), "OnPrefabInit")]
        public class Telepad_OnPrefabInit_Patch
        {
            public static void Postfix(Telepad __instance)
            {
                __instance.FindOrAddComponent<TelepadButton>();
            }
        }

        public string SidescreenButtonText { get => "Magma Rain"; }

        public string SidescreenButtonTooltip { get; }

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

        public bool SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable() => true;

        public void OnSidescreenButtonPressed()
        {
            object data = null;
            if (Config.Instance.EnableTwitchMagmaRain)
                TwitchPatch.MagmaRain(data);
            
        }

        public int HorizontalGroupID() => -1;

        public int ButtonSideScreenSortOrder() => 1;

    }
    internal class TelepadButton2 : KMonoBehaviour, ISidescreenButtonControl
    {
        [HarmonyPatch(typeof(Telepad), "OnPrefabInit")]
        public class Telepad_OnPrefabInit_Patch
        {
            public static void Postfix(Telepad __instance)
            {
                __instance.FindOrAddComponent<TelepadButton2>();
            }
        }

        public string SidescreenButtonText { get => "Zoological Meteors"; }

        public string SidescreenButtonTooltip { get; }

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

        public bool SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable() => true;

        public void OnSidescreenButtonPressed()
        {
            object data = null;            
            if (Config.Instance.EnableTwitchZoological)
                TwitchPatch.ZoologicalMeteors(data);            
        }

        public int HorizontalGroupID() => -1;

        public int ButtonSideScreenSortOrder() => 1;

    }
    internal class TelepadButton3 : KMonoBehaviour, ISidescreenButtonControl
    {
        [HarmonyPatch(typeof(Telepad), "OnPrefabInit")]
        public class Telepad_OnPrefabInit_Patch
        {
            public static void Postfix(Telepad __instance)
            {
                __instance.FindOrAddComponent<TelepadButton3>();
            }
        }

        public string SidescreenButtonText { get => "Water Balloons"; }

        public string SidescreenButtonTooltip { get; }

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

        public bool SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable() => true;

        public void OnSidescreenButtonPressed()
        {
            object data = null;            
            if (Config.Instance.EnableTwitchWaterBalloons)
                TwitchPatch.WaterBalloons(data);            
        }

        public int HorizontalGroupID() => -1;

        public int ButtonSideScreenSortOrder() => 1;

    }
    internal class TelepadButton4 : KMonoBehaviour, ISidescreenButtonControl
    {
        [HarmonyPatch(typeof(Telepad), "OnPrefabInit")]
        public class Telepad_OnPrefabInit_Patch
        {
            public static void Postfix(Telepad __instance)
            {
                __instance.FindOrAddComponent<TelepadButton4>();
            }
        }

        public string SidescreenButtonText { get => "Molten Slugs"; }

        public string SidescreenButtonTooltip { get; }

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

        public bool SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable() => true;

        public void OnSidescreenButtonPressed()
        {
            object data = null;
            
            if (Config.Instance.EnableTwitchMoltenSlugs)
                TwitchPatch.MoltenSlugs(data);
            
        }

        public int HorizontalGroupID() => -1;

        public int ButtonSideScreenSortOrder() => 1;

    }
    internal class TelepadButton5 : KMonoBehaviour, ISidescreenButtonControl
    {
        [HarmonyPatch(typeof(Telepad), "OnPrefabInit")]
        public class Telepad_OnPrefabInit_Patch
        {
            public static void Postfix(Telepad __instance)
            {
                __instance.FindOrAddComponent<TelepadButton5>();
            }
        }

        public string SidescreenButtonText { get => "Nuclear Waste Rain"; }

        public string SidescreenButtonTooltip { get; }

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

        public bool SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable() => true;

        public void OnSidescreenButtonPressed()
        {
            object data = null;            
            if (Config.Instance.EnableTwitchNuclearWasteRain)
                TwitchPatch.NuclearWasteRain(data);
        }

        public int HorizontalGroupID() => -1;

        public int ButtonSideScreenSortOrder() => 1;

    }
}
*/