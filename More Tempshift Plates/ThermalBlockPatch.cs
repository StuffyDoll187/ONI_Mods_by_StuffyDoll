/*using HarmonyLib;

using UnityEngine;

namespace More_Tempshift_Plates
{
    public class ThermalBlockPatch
    {

        [HarmonyPatch(typeof(ThermalBlockConfig))]
        [HarmonyPatch("DoPostConfigureComplete")]
        public class ThermalBlockConfig_DoPostConfigure_Patch
        {            
            public static void Postfix(ref GameObject go, CellOffset[] ___overrideOffsets)
            {
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
        }

        

        [HarmonyPatch(typeof(ThermalBlockConfig))]
        [HarmonyPatch("CreateBuildingDef")]        
        public class ThermalBlockConfig_CreateBuildingDef_Patch
        {           
            public static void Postfix(ref BuildingDef __result)
            {               
                __result.BuildLocationRule = BuildLocationRule.Anywhere;
            }
        }
    }
}
*/