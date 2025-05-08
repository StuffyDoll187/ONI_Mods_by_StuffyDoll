using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace Remote_Worker_Errands_Expanded
{
    class MilkFatSeperatorRemote : RemoteWorkable
    {
        public Chore chore;
        public override Chore RemoteDockChore => chore;



        [HarmonyPatch(typeof(MilkSeparator), nameof(MilkSeparator.CreateEmptyChore))]
        public class MilkSeparator_CreateEmptyChore_Patch
        {
            public static void Postfix(MilkSeparator.Instance smi, Chore __result)
            {
                smi.GetComponent<MilkFatSeperatorRemote>().chore = __result;
            }
        }

        [HarmonyPatch(typeof(MilkFatSeparatorConfig), nameof(MilkFatSeparatorConfig.ConfigureBuildingTemplate))]
        public class MilkFatSeparatorConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddComponent<MilkFatSeperatorRemote>();
            }
        }

    }
}
