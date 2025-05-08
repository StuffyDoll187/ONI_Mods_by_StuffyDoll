using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Remote_Worker_Errands_Expanded
{
    class MissionControlClusterRemote : RemoteWorkable//ManuallySetRemoteWorkTargetComponent
    {
        public Chore chore;
        public override Chore RemoteDockChore => chore;


        [HarmonyPatch(typeof(MissionControlCluster), nameof(MissionControlCluster.CreateChore))]
        public class MissionControlCluster_CreateChore_Patch
        {
            public static void Postfix(MissionControlCluster.Instance smi, ref Chore __result)
            {
                var remote = smi.master.GetComponent<MissionControlClusterRemote>();
                remote.chore = __result;
            }
        }

        [HarmonyPatch(typeof(MissionControlClusterConfig), nameof(MissionControlClusterConfig.ConfigureBuildingTemplate))]
        public class MissionControlClusterConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddComponent<MissionControlClusterRemote>();
            }
        }

    }
}