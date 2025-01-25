// Decompiled with JetBrains decompiler
// Type: AdvancedApothecaryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B86E23FE-3B43-4053-84B0-ABB90493789E
// Assembly location: E:\SteamLibrary\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Upgradeable_Dupes_And_Critters
{
    public class NuclearApothecaryConfig : IBuildingConfig
    {
        public const string ID = "NuclearApothecary";
        public const float PARTICLE_CAPACITY = 400f;
        public static Tag INPUT_MATERIAL = SimHashes.SaltWater.CreateTag();
        public ComplexRecipe recipe;

        //public override string[] GetDlcIds() => DlcManager.AVAILABLE_EXPANSION1_ONLY;
        public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;
        public override BuildingDef CreateBuildingDef()
        {
            float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
            string[] refinedMetals = MATERIALS.REFINED_METALS;
            EffectorValues none1 = NOISE_POLLUTION.NONE;
            EffectorValues none2 = BUILDINGS.DECOR.NONE;
            EffectorValues noise = none1;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("NuclearApothecary", 3, 3, "medicine_nuclear_kanim", 250, 240f, tieR5, refinedMetals, 800f, BuildLocationRule.OnFloor, none2, noise);
            buildingDef.ExhaustKilowattsWhenActive = 0.5f;
            buildingDef.SelfHeatKilowattsWhenActive = 2f;
            buildingDef.UseHighEnergyParticleInputPort = true;
            buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 2);
            buildingDef.ViewMode = OverlayModes.Radiation.ID;
            buildingDef.AudioCategory = "Glass";
            buildingDef.AudioSize = "large";            
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Prioritizable.AddRef(go);
            Storage storage = go.AddOrGet<Storage>();
            storage.capacityKg = 30f;
            storage.showInUI = true;
            storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
            {                
                Storage.StoredItemModifier.Insulate
            });
            /*ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
            manualDeliveryKg.SetStorage(storage);
            manualDeliveryKg.RequestedItemTag = NuclearApothecaryConfig.INPUT_MATERIAL;
            manualDeliveryKg.refillMass = 50f;
            manualDeliveryKg.capacity = 200f;
            manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;*/
            HighEnergyParticleStorage energyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
            energyParticleStorage.autoStore = true;
            energyParticleStorage.capacity = 400f;
            energyParticleStorage.showCapacityStatusItem = true;
            go.AddOrGet<HighEnergyParticlePort>().requireOperational = false;
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
            NuclearApothecary fabricator = go.AddOrGet<NuclearApothecary>();
            BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator)fabricator);
            ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
            go.AddOrGet<FabricatorIngredientStatusManager>();
            go.AddOrGet<CopyBuildingSettings>();
            ActiveParticleConsumer.Def def = go.AddOrGetDef<ActiveParticleConsumer.Def>();
            def.activeConsumptionRate = 2f;
            def.minParticlesForOperational = 2f;
            def.meterSymbolName = (string)null;                        
        }
                      
        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGetDef<PoweredController.Def>();
        }
    }
}