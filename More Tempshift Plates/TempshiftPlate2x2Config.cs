using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace More_Tempshift_Plates
{
    public class TempshiftPlate2x2Config : IBuildingConfig
    {
        public const string ID = "TempshiftPlate2x2";

        private static CellOffset[] overrideOffsets;


        public override BuildingDef CreateBuildingDef()
        {
            float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
            string[] anyBuildable = MATERIALS.ANY_BUILDABLE;
            EffectorValues none1 = NOISE_POLLUTION.NONE;
            EffectorValues none2 = DECOR.NONE;
            EffectorValues noise = none1;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("TempshiftPlate2x2", 1, 1, "tempshiftplate2x2_kanim", 30, 80f, tieR5, anyBuildable, 1600f, BuildLocationRule.Anywhere, none2, noise);
            buildingDef.Floodable = false;
            buildingDef.Overheatable = false;
            buildingDef.Entombable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.ViewMode = OverlayModes.Temperature.ID;
            buildingDef.PermittedRotations = PermittedRotations.R360;
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
        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            TempshiftPlate2x2Config.AddVisualizer(go);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            TempshiftPlate2x2Config.AddVisualizer(go);
        }
        public override void DoPostConfigureComplete(GameObject go)
        {
            if (Config.Instance.Enable_Heat_Exchange_Component)
                go.AddComponent<InterPlateHeatExchange>();
            
            go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn)(inst =>
            {
                Rotatable component1 = inst.GetComponent<Rotatable>();

                switch (component1.GetOrientation())
                {
                    case Orientation.Neutral:
                        TempshiftPlate2x2Config.overrideOffsets = new CellOffset[4]
                        {  
                            new CellOffset(0, 0),
                            new CellOffset(1, 0),
                            new CellOffset(0, 1),
                            new CellOffset(1, 1)
                        };
                        break;
                    case Orientation.R90:
                        TempshiftPlate2x2Config.overrideOffsets = new CellOffset[4]
                        {  
                            new CellOffset(0, -1),
                            new CellOffset(1, -1),
                            new CellOffset(0, 0),
                            new CellOffset(1, 0)
                        };
                        break;
                    case Orientation.R180:
                        TempshiftPlate2x2Config.overrideOffsets = new CellOffset[4]
                        {  
                            new CellOffset(-1, -1),
                            new CellOffset(0, -1),
                            new CellOffset(-1, 0),
                            new CellOffset(0, 0)
                        };
                        break;
                    case Orientation.R270:
                        TempshiftPlate2x2Config.overrideOffsets = new CellOffset[4]
                        {  
                            new CellOffset(-1, 0),
                            new CellOffset(0, 0),
                            new CellOffset(-1, 1),
                            new CellOffset(0, 1)
                        };
                        break;

                }
            });


            KPrefabID component = go.GetComponent<KPrefabID>();
            component.AddTag(GameTags.Backwall);
            component.prefabSpawnFn += (KPrefabID.PrefabFn)(game_object =>
            {
                HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
                StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
                int cell = Grid.PosToCell(game_object);
                payload.OverrideExtents( SafeExtents.Extents(cell, TempshiftPlate2x2Config.overrideOffsets));
                GameComps.StructureTemperatures.SetPayload(handle, ref payload);
            });
            TempshiftPlate2x2Config.AddVisualizer(go);
        }


        private static void AddVisualizer(GameObject prefab)
        {
            RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
            rangeVisualizer.OriginOffset = new Vector2I(0, 0);
            rangeVisualizer.RangeMin.x = 0;
            rangeVisualizer.RangeMin.y = 0;
            rangeVisualizer.RangeMax.x = 1;
            rangeVisualizer.RangeMax.y = 1;
            rangeVisualizer.TestLineOfSight = false;
           // rangeVisualizer.BlockingTileVisible = false;
           // rangeVisualizer.AllowLineOfSightInvalidCells = true;
            prefab.GetComponent<KPrefabID>().instantiateFn += (KPrefabID.PrefabFn)(go => go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(cell => false));
        }        
    }
}