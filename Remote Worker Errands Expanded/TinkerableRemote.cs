using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class TinkerableRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(Tinkerable), "chore");

        [MyCmpGet]
        private Tinkerable tinkerable;

        //public override Chore RemoteDockChore => (Chore)chore.GetValue(tinkerable);
        public override Chore RemoteDockChore => tinkerable.chore;

        [HarmonyPatch(typeof(Tinkerable), nameof(Tinkerable.OnPrefabInit))]
        public class Tinkerable_OnPrefabInit_Patch
        {
            public static void Postfix(Tinkerable __instance)
            {
                __instance.FindOrAddComponent<TinkerableRemote>();
            }
        }
    }
}
