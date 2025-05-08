using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class DiggableRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(Diggable), "chore");

        [MyCmpGet]
        private Diggable diggable;

        //public override Chore RemoteDockChore => (Chore)chore.GetValue(diggable);
        public override Chore RemoteDockChore => diggable.chore;

        [HarmonyPatch(typeof(Diggable), nameof(Diggable.OnPrefabInit))]
        public class Diggable_OnPrefabInit_Patch
        {
            public static void Postfix(Diggable __instance)
            {
                __instance.FindOrAddComponent<DiggableRemote>();
            }
        }
    }
}
