using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;

using UnityEngine;
//using static STRINGS.UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE;

namespace Upgradeable_Dupes_And_Critters
{

    //[AddComponentMenu("")]
    public class DuplicantUpgraderConfig : IBuildingConfig
    {
        public const string ID = "DuplicantUpgrader";
        public override BuildingDef CreateBuildingDef()
        {
            float[] mass = new float[2] 
            {
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER7[0],
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0]
            };

            string[] mats = new string[2]
            {
                MATERIALS.REFINED_METAL,
                MATERIALS.GLASS
            };


            EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
            EffectorValues none = BUILDINGS.DECOR.NONE;
            EffectorValues noise = tieR1;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 4, 3, "geneshuffler_kanim", 30, 30f, mass, mats, 1600f, BuildLocationRule.OnFloor, none, noise);
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 2000f;
            buildingDef.ExhaustKilowattsWhenActive = 2f;
            buildingDef.SelfHeatKilowattsWhenActive = 16f;
            buildingDef.ViewMode = OverlayModes.Power.ID;            
            buildingDef.AudioCategory = "Metal";
            buildingDef.AudioSize = "large";
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            /*Storage storage = go.AddOrGet<Storage>();
            storage.capacityKg = 1f;
            storage.showInUI = true;
            storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
            {
                Storage.StoredItemModifier.Insulate
            });
            ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
            manualDeliveryKg.SetStorage(storage);
            manualDeliveryKg.RequestedItemTag = new Tag("RadioactiveSerum");
            manualDeliveryKg.refillMass = 1f;
            manualDeliveryKg.capacity = 1f;
            manualDeliveryKg.FillToCapacity = true;
            manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;*/
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddTag(GameTags.NotRoomAssignable);
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
            go.AddOrGet<Ownable>().slotID = DuplicantUpgraderConfig.ID;
            //go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.ResetSkillsStation.Id;
            DuplicantUpgrader resetSkillsStation = go.AddOrGet<DuplicantUpgrader>();
            resetSkillsStation.workTime = 12f;
            resetSkillsStation.overrideAnims = new KAnimFile[1]
            {
                Assets.GetAnim((HashedString) "anim_interacts_neuralvacillator_kanim")
            };
            resetSkillsStation.workLayer = Grid.SceneLayer.BuildingFront;
        }





    
        public override void DoPostConfigureComplete(GameObject go)
        {
            //GameUtil.KInstantiate(Assets.GetPrefab("GeneShuffler"), go.transform.GetPosition(),Grid.SceneLayer.Building);
        }
    }


}
