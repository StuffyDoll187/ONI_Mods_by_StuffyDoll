using HarmonyLib;
using UnityEngine;

namespace SpaceHeaterTargetTemp
{
    public class SpaceHeaterTargetPatch
    {
        [HarmonyPatch(typeof(SpaceHeaterConfig))]
        [HarmonyPatch("ConfigureBuildingTemplate")]
        public static class SpaceHeaterConfig_Configure_Building_Template_Patch
        {
            public static void Postfix(ref GameObject go, Tag prefab_tag)
            {
                go.AddOrGet<SpaceHeater>().targetTemperature = 500f;
                Debug.Log("Set Space Heater Target Temperature to" + go.AddOrGet<SpaceHeater>().targetTemperature );
            }
        }
    }
}
