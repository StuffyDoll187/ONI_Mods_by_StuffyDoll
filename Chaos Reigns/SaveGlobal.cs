using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSerialization;
using HarmonyLib;

namespace Chaos_Reigns
{
    [SerializationConfig(MemberSerialization.OptIn)]
    internal class SaveGlobal : KMonoBehaviour
    {
        [Serialize]
        public bool IsYAMLorCGMsave = false;


        [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
        public class SaveGame_OnPrefabInit_Patch
        {
            public static void Postfix(SaveGame __instance)
            {
                __instance.gameObject.AddComponent<SaveGlobal>();
            }
        }
    }
}
