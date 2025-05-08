using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class ValveRemote : RemoteWorkable
    {
        [MyCmpGet]
        Valve valve;
        public override Chore RemoteDockChore => valve.chore;

        [HarmonyPatch(typeof(Valve), nameof(Valve.OnPrefabInit))]
        public class Valve_OnPrefabInit_Patch
        {
            public static void Postfix(Valve __instance)
            {
                __instance.FindOrAddComponent<ValveRemote>();
            }
        }

    }
}
