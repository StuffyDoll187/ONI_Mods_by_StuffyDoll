using HarmonyLib;
using PeterHan.PLib.Options;
using System.Collections.Generic;

namespace Geyser_Cracking_Tool
{
    public class Mod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            //base.OnLoad(harmony);
            //new POptions().RegisterOptions(this, typeof(Config));
            Patches.DbInitialize_Patch.Patch(harmony);
            Patches.GameInputMapping_SetDefaultKeyBindings_Patch.Patch(harmony);
            
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            bool foundGeyserCrackingMod = false;
            foreach (KMod.Mod mod in mods)
            {
                if (!mod.IsActive())
                    continue;
                
                
                if (mod.staticID == "2874440740.Steam")
                {
                    //harmony.PatchAll();
                    Patches.ToolMenu_CreateBasicTools_Patch.Patch(harmony);
                    Patches.PlayerController_OnPrefabInit_Patch.Patch(harmony);
                    foundGeyserCrackingMod = true;                    
                    break;
                }
                /*Debug.Log("StaticID: " + mod.staticID);
                Debug.Log("Title: " + mod.title);
                Debug.Log(mod.label.id +" " + mod.label.title + " " + mod.label.defaultStaticID);*/
            }
            if (!foundGeyserCrackingMod)
                Debug.Log("[GeyserCrackingTool] Geyser Cracking Mod not found or inactive.");
            else
                Debug.Log("[GeyserCrackingTool] Geyser Cracking Mod found. Patching to create tool.");
        }


    }
}
