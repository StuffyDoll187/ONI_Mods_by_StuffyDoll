using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace More_Tempshift_Plates {
    // Decompiled with JetBrains decompiler
    // Type: TileConfig
    // Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
    // MVID: B86E23FE-3B43-4053-84B0-ABB90493789E
    // Assembly location: E:\SteamLibrary\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

    using TUNING;
    using UnityEngine;

    public class DenseTileConfig : IBuildingConfig
    {
        public const string ID = "DenseTile";
        public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_solid_tops");

        public override BuildingDef CreateBuildingDef()
        {
            float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
            float[] mass = new float[1] { Mathf.Max(1f,(float) Config.Instance.Dense_Tile_Mass) };
            string[] rawMinerals = MATERIALS.RAW_MINERALS;
            EffectorValues none = NOISE_POLLUTION.NONE;
            EffectorValues tieR0 = BUILDINGS.DECOR.BONUS.TIER0;
            EffectorValues noise = none;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DenseTile", 1, 1, "floor_basic_kanim", 100, 3f, mass, rawMinerals, 1600f, BuildLocationRule.Tile, tieR0, noise);
            BuildingTemplates.CreateFoundationTileDef(buildingDef);
            buildingDef.Floodable = false;
            buildingDef.Overheatable = false;
            buildingDef.Entombable = false;
            buildingDef.UseStructureTemperature = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.AudioSize = "small";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
            buildingDef.isKAnimTile = true;
            buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_solid");
            buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_solid_place");
            buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
            buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_info");
            buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_place_info");
            buildingDef.DragBuild = true;
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
            simCellOccupier.doReplaceElement = true;
            simCellOccupier.strengthMultiplier = 1.5f;
            simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_2;
            simCellOccupier.notifyOnMelt = true;
            go.AddOrGet<TileTemperature>();
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            go.AddOrGet<KAnimGridTileVisualizer>();
        }
    }
}
