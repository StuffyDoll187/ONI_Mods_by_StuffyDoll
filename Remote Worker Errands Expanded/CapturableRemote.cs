using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Remote_Worker_Errands_Expanded
{
    class CapturableRemote : RemoteWorkable
    {
        [MyCmpGet]
        Capturable capturable;
        public override Chore RemoteDockChore => capturable.chore;

        [HarmonyPatch(typeof(Capturable), nameof(Capturable.OnPrefabInit))]
        public class Capturable_OnPrefabInit_Patch
        {
            public static void Postfix(Capturable __instance)
            {
                __instance.FindOrAddComponent<CapturableRemote>();
            }
        }

    }
}
