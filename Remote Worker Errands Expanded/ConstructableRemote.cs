using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class ConstructableRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(Constructable), "buildChore");

        [MyCmpGet]
        private Constructable constructable;

        //public override Chore RemoteDockChore => (Chore)chore.GetValue(constructable);
        public override Chore RemoteDockChore => constructable.buildChore;

        [HarmonyPatch(typeof(Constructable), nameof(Constructable.OnPrefabInit))]
        public class Constructable_OnPrefabInit_Patch
        {
            public static void Postfix(Constructable __instance)
            {
                __instance.FindOrAddComponent<ConstructableRemote>();
            }
        }
    }
}
