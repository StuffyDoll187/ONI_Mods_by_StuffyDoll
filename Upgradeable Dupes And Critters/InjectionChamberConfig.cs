using TUNING;
using UnityEngine;

namespace Upgradeable_Dupes_And_Critters
{
    public class InjectionChamberConfig : IBuildingConfig
    {
        public const string ID = "InjectionChamber";
        public static string requiredSkillPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;

        public override BuildingDef CreateBuildingDef()
        {
            float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
            string[] refinedMetals = MATERIALS.REFINED_METALS;
            EffectorValues none1 = NOISE_POLLUTION.NONE;
            EffectorValues none2 = BUILDINGS.DECOR.NONE;
            EffectorValues noise = none1;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("InjectionChamber", 2, 3, "bed_medical_kanim", 100, 10f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, none2, noise);
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 480f;
            buildingDef.ExhaustKilowattsWhenActive = 0.25f;
            buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
            buildingDef.AudioCategory = "Metal";
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            
            go.AddOrGet<LoopingSounds>();
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Clinic);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<Ownable>().slotID = InjectionChamberConfig.ID;
            //go.AddComponent<Ownable>().slotID = Db.Get().AssignableSlots.ResetSkillsStation.Id;
            Storage storage = go.AddOrGet<Storage>();
            storage.showInUI = true;
            Tag supplyTagForStation = MedicineInfo.GetSupplyTagForStation(InjectionChamberConfig.ID);
            ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
            manualDeliveryKg.SetStorage(storage);
            manualDeliveryKg.RequestedItemTag = "RadioactiveSerum";//supplyTagForStation;
            manualDeliveryKg.capacity = 1f;
            manualDeliveryKg.refillMass = 1f;
            manualDeliveryKg.MinimumMass = 1f;
            manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
            manualDeliveryKg.operationalRequirement = Operational.State.Functional;
            InjectionChamber doctorStation = go.AddOrGet<InjectionChamber>();
            doctorStation.overrideAnims = new KAnimFile[1]
            {
                Assets.GetAnim((HashedString) "anim_interacts_medical_bed_kanim")
            };
            doctorStation.workLayer = Grid.SceneLayer.BuildingFront;
            RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
            roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
            roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
            roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
            InjectionChamberDoctor stationDoctorWorkable = go.AddOrGet<InjectionChamberDoctor>();
            stationDoctorWorkable.overrideAnims = new KAnimFile[1]
            {
                Assets.GetAnim((HashedString) "anim_interacts_medical_bed_doctor_kanim")
                //Assets.GetAnim((HashedString) "anim_interacts_injection_chamber_doctor_kanim")
            };
            stationDoctorWorkable.SetWorkTime(60f);
            stationDoctorWorkable.requiredSkillPerk = requiredSkillPerk;
        }
    }
}