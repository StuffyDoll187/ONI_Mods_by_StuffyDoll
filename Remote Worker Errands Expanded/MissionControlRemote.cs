using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Remote_Worker_Errands_Expanded
{
    class MissionControlRemote : RemoteWorkable//ManuallySetRemoteWorkTargetComponent
    {
        public Chore chore;
        public override Chore RemoteDockChore => chore;


        [HarmonyPatch(typeof(MissionControl), nameof(MissionControl.CreateChore))]
        public class MissionControl_CreateChore_Patch
        {
            public static void Postfix(MissionControl.Instance smi, ref Chore __result)
            {
                var remote = smi.master.GetComponent<MissionControlRemote>();
                remote.chore = __result;
            }
        }

        [HarmonyPatch(typeof(MissionControlConfig), nameof(MissionControlConfig.ConfigureBuildingTemplate))]
        public class MissionControlConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddComponent<MissionControlRemote>();
            }
        }

    }
}
