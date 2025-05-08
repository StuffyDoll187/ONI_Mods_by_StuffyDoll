using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class DeconstructableRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(Deconstructable), "chore");

        [MyCmpGet]
        private Deconstructable deconstructable;

        //public override Chore RemoteDockChore => (Chore)chore.GetValue(deconstructable);
        public override Chore RemoteDockChore => deconstructable.chore;

        [HarmonyPatch(typeof(Deconstructable), nameof(Deconstructable.OnPrefabInit))]
        public class Deconstructable_OnPrefabInit_Patch
        {
            public static void Postfix(Deconstructable __instance)
            {
                __instance.FindOrAddComponent<DeconstructableRemote>();
            }
        }
    }
}
