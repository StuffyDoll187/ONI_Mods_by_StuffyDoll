using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace Remote_Worker_Errands_Expanded
{
    class ResearchCenterRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(ResearchCenter), "chore");

        [MyCmpGet]
        private ResearchCenter researchCenter;

        //public override Chore RemoteDockChore => (Chore) chore.GetValue(researchCenter);
        public override Chore RemoteDockChore => researchCenter.chore;

        [HarmonyPatch(typeof(ResearchCenter), nameof(ResearchCenter.OnPrefabInit))]
        public class ResearchCenter_OnPrefabInit_Patch
        {
            public static void Postfix(ResearchCenter __instance)
            {
                __instance.FindOrAddComponent<ResearchCenterRemote>();
            }
        }
    }
}
