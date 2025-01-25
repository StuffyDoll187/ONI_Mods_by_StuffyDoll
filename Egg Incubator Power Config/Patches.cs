using HarmonyLib;

namespace Egg_Incubator_Power_Config
{
    public class Patches
    {

        [HarmonyPatch(typeof(EggIncubatorConfig))]
        [HarmonyPatch(nameof(EggIncubatorConfig.CreateBuildingDef))]
        internal class EggIncubatoroConfig_CreateBuildingDefPatch
        {
            public static BuildingDef Postfix(BuildingDef __result)
            {
                __result.RequiresPowerInput = Config.Instance.Requires_Power;
                __result.EnergyConsumptionWhenActive = (float) Config.Instance.Watts;
                return __result;
            }
        }

    }
}
