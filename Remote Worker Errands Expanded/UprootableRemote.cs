using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class UprootableRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(Uprootable), "chore");

        [MyCmpGet]
        private Uprootable uprootable;

        //public override Chore RemoteDockChore => (Chore)chore.GetValue(uprootable);
        public override Chore RemoteDockChore => uprootable.chore;

        [HarmonyPatch(typeof(Uprootable), nameof(Uprootable.OnPrefabInit))]
        public class Uprootable_OnPrefabInit_Patch
        {
            public static void Postfix(Uprootable __instance)
            {
                __instance.FindOrAddComponent<UprootableRemote>();
            }
        }
    }
}
