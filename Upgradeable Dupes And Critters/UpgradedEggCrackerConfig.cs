// Decompiled with JetBrains decompiler
// Type: EggCrackerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B86E23FE-3B43-4053-84B0-ABB90493789E
// Assembly location: E:\SteamLibrary\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Upgradeable_Dupes_And_Critters
{

    [EntityConfigOrder(2)]
    public class UpgradedEggCrackerConfig : IBuildingConfig
    {
        public const string ID = "UpgradedEggCracker";

        public override BuildingDef CreateBuildingDef()
        {
            float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
            string[] rawMetals = MATERIALS.RAW_METALS;
            EffectorValues none = NOISE_POLLUTION.NONE;
            EffectorValues tieR0 = BUILDINGS.DECOR.BONUS.TIER0;
            EffectorValues noise = none;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("UpgradedEggCracker", 2, 2, "egg_cracker_kanim", 30, 10f, tieR1, rawMetals, 1600f, BuildLocationRule.OnFloor, tieR0, noise);
            buildingDef.AudioCategory = "Metal";
            buildingDef.SceneLayer = Grid.SceneLayer.Building;
            buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
            go.AddOrGet<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString)"snapto_egg", false);
            UpgradedEggCracker fabricator = go.AddOrGet<UpgradedEggCracker>();
            fabricator.labelByResult = false;
            fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
            fabricator.duplicantOperated = true;
            go.AddOrGet<FabricatorIngredientStatusManager>();
            go.AddOrGet<CopyBuildingSettings>();
            ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
            BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
            KAnimFile[] kanimFileArray = new KAnimFile[1]
            {
      Assets.GetAnim((HashedString) "anim_interacts_egg_cracker_kanim")
            };
            fabricatorWorkable.overrideAnims = kanimFileArray;
            fabricator.outputOffset = new Vector3(1f, 1f, 0.0f);
            Prioritizable.AddRef(go);
            //go.AddOrGet<EggCracker>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<LogicOperationalController>();
        }

        
    }
}