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
    class GeotunerSwitchGeyserRemote : RemoteWorkable
    {
        //public static FieldInfo switchGeyserChore = AccessTools.Field(typeof(GeoTuner.Instance), "switchGeyserChore");

        [MySmiGet]
        private GeoTuner.Instance geotunerInstance;

        //public override Chore RemoteDockChore => (Chore)switchGeyserChore.GetValue(geotunerInstance);
        public override Chore RemoteDockChore => geotunerInstance.switchGeyserChore;

        [HarmonyPatch(typeof(GeoTunerConfig), nameof(GeoTunerConfig.ConfigureBuildingTemplate))]
        public class GeoTunerConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddComponent<GeotunerSwitchGeyserRemote>();
            }
        }
    }
}