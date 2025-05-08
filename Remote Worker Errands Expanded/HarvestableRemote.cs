using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class HarvestableRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(Harvestable), "chore");

        [MyCmpGet]
        private Harvestable harvestable;

        //public override Chore RemoteDockChore => (Chore)chore.GetValue(harvestable);
        public override Chore RemoteDockChore => harvestable.chore;

        [HarmonyPatch(typeof(Harvestable), nameof(Harvestable.OnPrefabInit))]
        public class Harvestable_OnPrefabInit_Patch
        {
            public static void Postfix(Harvestable __instance)
            {
                __instance.FindOrAddComponent<HarvestableRemote>();
            }
        }
    }
}
