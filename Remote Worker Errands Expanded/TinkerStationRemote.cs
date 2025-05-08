using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class TinkerStationRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(TinkerStation), "chore");

        [MyCmpGet]
        private TinkerStation tinkerStation;

        //public override Chore RemoteDockChore => (Chore)chore.GetValue(TinkerStation);
        public override Chore RemoteDockChore => tinkerStation.chore;

        [HarmonyPatch(typeof(TinkerStation), nameof(TinkerStation.OnPrefabInit))]
        public class Tinkerable_OnPrefabInit_Patch
        {
            public static void Postfix(TinkerStation __instance)
            {
                __instance.FindOrAddComponent<TinkerStationRemote>();
            }
        }
    }
}
