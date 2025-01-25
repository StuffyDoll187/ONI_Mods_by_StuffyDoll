using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace More_Tempshift_Plates
{
    public class MorphTempshiftPlateConfig : IBuildingConfig
    {
        public const string ID = "MorphTempshiftPlate";
        

        public override BuildingDef CreateBuildingDef()
        {
            float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
            string[] anyBuildable = MATERIALS.ANY_BUILDABLE;
            EffectorValues none1 = NOISE_POLLUTION.NONE;
            EffectorValues none2 = DECOR.NONE;
            EffectorValues noise = none1;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MorphTempshiftPlate", 1, 1, "morphtempshiftplate_kanim", 30, 120f, tieR5, anyBuildable, 1600f, BuildLocationRule.Anywhere, none2, noise);
            buildingDef.ThermalConductivity = 1f;
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
            go.AddOrGet<RangeVisualizer>().TestLineOfSight = false;                        
            go.GetComponent<KPrefabID>().instantiateFn += (KPrefabID.PrefabFn)(go1 => go1.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(cell => false));            
            KPrefabID component = go.GetComponent<KPrefabID>();
            component.AddTag(GameTags.Backwall);
            go.AddComponent<MultiSliderExtents>();
            if (Config.Instance.Enable_Heat_Exchange_Component)
                go.AddComponent<InterPlateHeatExchange>();
            
        }
    }
}