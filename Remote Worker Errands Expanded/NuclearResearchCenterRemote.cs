using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Remote_Worker_Errands_Expanded
{
    class NuclearResearchCenterRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(NuclearResearchCenter.StatesInstance), "chore");

        [MySmiGet]
        private NuclearResearchCenter.StatesInstance nuclearStatesInstance;

        //public override Chore RemoteDockChore => (Chore)chore.GetValue(nuclearStatesInstance);
        public override Chore RemoteDockChore => nuclearStatesInstance.chore;

        [HarmonyPatch(typeof(NuclearResearchCenterConfig), nameof(NuclearResearchCenterConfig.ConfigureBuildingTemplate))]
        public class NuclearResearchCenterConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddComponent<NuclearResearchCenterRemote>();
            }
        }
    }
}
