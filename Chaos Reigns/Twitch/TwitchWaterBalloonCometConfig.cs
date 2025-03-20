

using STRINGS;
using UnityEngine;

namespace Chaos_Reigns
{
    public class TwitchWaterBalloonCometConfig : IEntityConfig, IHasDlcRestrictions
    {
        public static string ID = "TwitchWaterBalloonComet";

        string[] IEntityConfig.GetDlcIds() => null;
        string[] IHasDlcRestrictions.GetRequiredDlcIds() => DlcManager.EXPANSION1;
        string[] IHasDlcRestrictions.GetForbiddenDlcIds() => null;

        public GameObject CreatePrefab()
        {
            GameObject entity = EntityTemplates.CreateEntity(ID, "Water Balloon");
            entity.AddOrGet<SaveLoadRoot>();
            entity.AddOrGet<LoopingSounds>();
            Comet comet = entity.AddOrGet<TwitchWaterBalloonComet>();
            comet.destroyOnExplode = false;
            comet.EXHAUST_ELEMENT = SimHashes.Void;            
            comet.massRange = new Vector2(500f, 1000f);
            comet.temperatureRange = new Vector2(318.15f, 318.15f);
            comet.explosionOreCount = new Vector2I(0, 0);
            comet.entityDamage = 0;
            comet.totalTileDamage = 0f;
            comet.splashRadius = 2;
            //comet.impactSound = "Meteor_Medium_Impact";
            comet.flyingSoundID = 1;
            comet.explosionEffectHash = SpawnFXHashes.OxygenEmissionBubbles;
            comet.spawnVelocity = new Vector2(12f, 5f);


            PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
            primaryElement.SetElement(SimHashes.Water);
            primaryElement.Temperature = (float)(((double)comet.temperatureRange.x + (double)comet.temperatureRange.y) / 2.0);
            KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
            kbatchedAnimController.AnimFiles = new KAnimFile[1]
            {
      Assets.GetAnim((HashedString) "water_balloon_kanim")
            };
            kbatchedAnimController.isMovable = true;
            kbatchedAnimController.initialAnim = "NewAnimation";
            kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
            kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
            kbatchedAnimController.animScale = 0.01f;
            entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
            entity.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            entity.AddTag(GameTags.Comet);
            return entity;
        }

        public void OnPrefabInit(GameObject go)
        {
        }

        public void OnSpawn(GameObject go)
        {
        }
    }
}