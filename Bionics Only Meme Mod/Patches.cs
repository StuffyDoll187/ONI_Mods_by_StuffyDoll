using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Bionics_Only_Meme_Mod
{
    public class Patches
    {
        private static bool dlc = DlcManager.IsContentSubscribed("DLC3_ID");
        [HarmonyPatch(typeof(CharacterContainer), nameof(CharacterContainer.GenerateCharacter))]
        public class CharacterContainer_GenerateCharacter_Patch
        {
            
            private static List<Tag> bionicsOnly = new List<Tag>()
            {
                GameTags.Minions.Models.Bionic
            };
                
            public static void Prefix(ref List<Tag> ___permittedModels)
            {
                if (dlc)                
                    ___permittedModels = bionicsOnly;                
            }
        }

        [HarmonyPatch(typeof(MinionConfig), nameof(MinionConfig.OnSpawn))]
        public class MinionConfig_OnSpawn_Patch 
        {
            public static void Postfix(GameObject go) 
            {
                if (dlc)
                    go.DeleteObject();
            }

        }
    }
}
