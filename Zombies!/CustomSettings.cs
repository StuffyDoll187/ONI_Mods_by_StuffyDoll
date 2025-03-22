using HarmonyLib;
using Klei.CustomSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zombies
{
    class CustomSettings
    {
        
        public static SettingConfig Zombies = new ToggleSettingConfig(
                id: nameof(Zombies),
                label: "Zombies!",
                tooltip: "All Duplicants start with a Zombie Spore infection of infinite duration.\nMust be cured by Serum Vial.",
                off_level: new SettingLevel("Disabled", "Unused", "Unchecked: No Effect", 1L),
                on_level: new SettingLevel("Enabled", "Unused", "Checked: Zombie Challenge"),
                default_level_id: "Disabled",
                nosweat_default_level_id: "Disabled",
                coordinate_range: 5L,  //idk , number of different levels the setting can be maybe, 5 is what Klei has for other checkboxes
                required_content: null)
        ;
        
        [HarmonyPatch(typeof(CustomGameSettings), "OnPrefabInit")]
        public class CustomGameSettings_OnPrefabInit_Patch
        {
            public static void Postfix()
            {

                CustomGameSettings.Instance.AddQualitySettingConfig(Zombies);
                if (Zombies.coordinate_range >= 0L)
                    CustomGameSettings.Instance.CoordinatedQualitySettings.Add(Zombies.id);

            }
        }

    }
}
