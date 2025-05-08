using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class DropAllWorkableRemote : RemoteWorkable
    {
        [MyCmpGet]
        DropAllWorkable dropAllWorkable;
        public override Chore RemoteDockChore => dropAllWorkable.Chore;

        [HarmonyPatch(typeof(DropAllWorkable), nameof(DropAllWorkable.OnPrefabInit))]
        public class DropAllWorkable_OnPrefabInit_Patch
        {
            public static void Postfix(DropAllWorkable __instance)
            {
                __instance.FindOrAddComponent<DropAllWorkableRemote>();
            }
        }

    }
}
