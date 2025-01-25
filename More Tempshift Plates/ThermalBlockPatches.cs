using HarmonyLib;
using UnityEngine;

namespace More_Tempshift_Plates
{
    public class ThermalBlockPatches
    {

        public static class ThermalBlockConfig_DoPostConfigureComplete_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var original = typeof(ThermalBlockConfig).GetMethod(nameof(ThermalBlockConfig.DoPostConfigureComplete));
                var postfix = typeof(ThermalBlockConfig_DoPostConfigureComplete_Patch).GetMethod(nameof(Postfix));
                harmony.Patch(original, null, new HarmonyMethod(postfix), null, null);
            }

            public static void Postfix(ref GameObject go, CellOffset[] ___overrideOffsets)
            {

                if (Config.Instance.Enable_Vanilla_Plate_Heat_Exchange_Component)
                    go.AddComponent<InterPlateHeatExchange>();
                if (Config.Instance.Enable_Vanilla_Plate_Extents_BugFix)
                {
                    KPrefabID component = go.GetComponent<KPrefabID>();
                    component.prefabSpawnFn += (KPrefabID.PrefabFn)(game_object =>
                    {
                        HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
                        StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
                        int cell = Grid.PosToCell(game_object);
                        //payload.OverrideExtents(new Extents(cell, ThermalBlockConfig.overrideOffsets, Extents.BoundsCheckCoords));
                        payload.OverrideExtents(SafeExtents.Extents(cell, ___overrideOffsets));
                        GameComps.StructureTemperatures.SetPayload(handle, ref payload);
                    });
                }
            }

        }

        /*[HarmonyPatch(typeof(ThermalBlockConfig))]
        [HarmonyPatch("DoPostConfigureComplete")]
        public class ThermalBlockConfig_DoPostConfigure_Patch
        {            
            public static void Postfix(ref GameObject go, CellOffset[] ___overrideOffsets)
            {
                
                if (Config.Instance.Enable_Heat_Exchange_Component)
                    go.AddComponent<InterPlateHeatExchange>();
                KPrefabID component = go.GetComponent<KPrefabID>();

                component.prefabSpawnFn += (KPrefabID.PrefabFn)(game_object =>
                {
                    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
                    StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
                    int cell = Grid.PosToCell(game_object);
                    //payload.OverrideExtents(new Extents(cell, ThermalBlockConfig.overrideOffsets, Extents.BoundsCheckCoords));
                    payload.OverrideExtents(SafeExtents.Extents(cell, ___overrideOffsets));
                    GameComps.StructureTemperatures.SetPayload(handle, ref payload);
                });
            }
        }*/

        public static class ThermalBlockConfig_CreateBuildingDef_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var original = typeof(ThermalBlockConfig).GetMethod(nameof(ThermalBlockConfig.CreateBuildingDef));
                var postfix = typeof(ThermalBlockConfig_CreateBuildingDef_Patch).GetMethod(nameof(Postfix));
                harmony.Patch(original, null, new HarmonyMethod(postfix), null, null);
            }

            public static void Postfix(ref BuildingDef __result)
            {
                __result.BuildLocationRule = BuildLocationRule.Anywhere;
            }

        }

        /*[HarmonyPatch(typeof(ThermalBlockConfig))]
        [HarmonyPatch("CreateBuildingDef")]
        public class ThermalBlockConfig_CreateBuildingDef_Patch
        {           
            public static void Postfix(ref BuildingDef __result)
            {               
                __result.BuildLocationRule = BuildLocationRule.Anywhere;
            }
        }*/
    }
}
