using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace More_Tempshift_Plates
{

    public class VeryDenseTempshiftPlateConfig : IBuildingConfig
    {
        public const string ID = "VeryDenseTempshiftPlate";
        /* private static readonly CellOffset[] overrideOffsets = new CellOffset[4]
         {
         new CellOffset(-1, -1),
         new CellOffset(1, -1),
         new CellOffset(-1, 1),
         new CellOffset(1, 1)
         };*/

        public override BuildingDef CreateBuildingDef()
        {
            float[] mass = new float[1] { Mathf.Max( 1f, (float) Config.Instance.Very_Dense_Tempshift_Plate_Mass) };
            string[] anyBuildable = MATERIALS.ANY_BUILDABLE;
            EffectorValues none1 = NOISE_POLLUTION.NONE;
            EffectorValues none2 = DECOR.NONE;
            EffectorValues noise = none1;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("VeryDenseTempshiftPlate", 1, 1, "verydensetempshiftplate_kanim", 30, 180f, mass, anyBuildable, 1600f, BuildLocationRule.Anywhere, none2, noise, 1f);
            buildingDef.ThermalConductivity = 0.2f;
            buildingDef.Floodable = false;
            buildingDef.Overheatable = false;
            buildingDef.Entombable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.ViewMode = OverlayModes.Temperature.ID;
            buildingDef.DefaultAnimState = "off";
            buildingDef.ObjectLayer = ObjectLayer.Backwall;
            buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
            buildingDef.ReplacementLayer = ObjectLayer.ReplacementBackwall;
            buildingDef.ReplacementCandidateLayers = new List<ObjectLayer>()
        {
            ObjectLayer.FoundationTile,
            ObjectLayer.Backwall
        };
            buildingDef.ReplacementTags = new List<Tag>()
        {
            GameTags.FloorTiles,
            GameTags.Backwall
        };
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
            go.AddComponent<ZoneTile>();
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            if (Config.Instance.Enable_Heat_Exchange_Component)
                go.AddComponent<InterPlateHeatExchange>();
           
            KPrefabID component = go.GetComponent<KPrefabID>();
            component.AddTag(GameTags.Backwall);
            component.prefabSpawnFn += (KPrefabID.PrefabFn)(game_object =>
            {
                HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
                StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
                int cell = Grid.PosToCell(game_object);
                payload.OverrideExtents( Extents.OneCell(cell));
                GameComps.StructureTemperatures.SetPayload(handle, ref payload);
            });
        }
    }
}