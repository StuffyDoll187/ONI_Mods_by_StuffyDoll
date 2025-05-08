/*using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Remote_Worker_Errands_Expanded
{
    class MorbRoverMakerRemote : RemoteWorkable
    {
        //public static FieldInfo chore = AccessTools.Field(typeof(MorbRoverMaker.Instance), "workChore_releaseRover");

        [MySmiReq]
        private MorbRoverMaker.Instance morbRoverMakerInstance;

        //public override Chore RemoteDockChore => (Chore)chore.GetValue(morbRoverMakerInstance);
        public override Chore RemoteDockChore => morbRoverMakerInstance.workChore_releaseRover;

        //Doesn't work????



        //[HarmonyPatch(typeof(MorbRoverMakerConfig), nameof(MorbRoverMakerConfig.ConfigureBuildingTemplate))]
        public class MorbRoverMakerConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var original = typeof(MorbRoverMakerConfig).GetMethod(nameof(MorbRoverMakerConfig.ConfigureBuildingTemplate));
                var postfix = typeof(MorbRoverMakerConfig_ConfigureBuildingTemplate_Patch).GetMethod(nameof(Postfix));
                harmony.Patch(original, null, new HarmonyMethod(postfix), null, null);
            }            
            public static void Postfix(GameObject go)
            {
                go.AddComponent<MorbRoverMakerRemote>();
            }
        }
        [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public class Db_Initialize_Patch
        {
            public static Harmony Harmony;
            public static void Postfix()
            {
                MorbRoverMakerConfig_ConfigureBuildingTemplate_Patch.Patch(Harmony);
            }
        }

    }
}
*/