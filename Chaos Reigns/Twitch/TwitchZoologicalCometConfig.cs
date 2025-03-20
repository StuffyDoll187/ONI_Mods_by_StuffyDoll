using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Chaos_Reigns
{
    internal class TwitchZoologicalCometConfig : IEntityConfig, IHasDlcRestrictions
    {
        
        
            public static string ID = "TwitchZoologicalComet";

        string[] IEntityConfig.GetDlcIds() => null;
        string[] IHasDlcRestrictions.GetRequiredDlcIds() => DlcManager.EXPANSION1;
        string[] IHasDlcRestrictions.GetForbiddenDlcIds() => null;

        public GameObject CreatePrefab()
            {
                GameObject entity = EntityTemplates.CreateEntity(ID, "Zoological Comet");
                entity.AddOrGet<SaveLoadRoot>();
                entity.AddOrGet<LoopingSounds>();
                TwitchZoologicalComet zoologicalComet = entity.AddOrGet<TwitchZoologicalComet>();
                zoologicalComet.massRange = new Vector2(100f, 200f);
                zoologicalComet.EXHAUST_ELEMENT = SimHashes.Void;
                zoologicalComet.temperatureRange = new Vector2(318.15f, 318.15f);
                zoologicalComet.entityDamage = 0;
                zoologicalComet.explosionOreCount = new Vector2I(0, 0);
                zoologicalComet.totalTileDamage = 0.0f;
                zoologicalComet.splashRadius = 1;
                zoologicalComet.impactSound = "";
                zoologicalComet.flyingSoundID = 1;
                zoologicalComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
                zoologicalComet.addTiles = 0;
                zoologicalComet.affectedByDifficulty = false;
                zoologicalComet.lootOnDestroyedByMissile = new string[]
                {
                    
                };
                zoologicalComet.destroyOnExplode = false;            
                zoologicalComet.craterPrefabs = TwitchZoologicalComet.critters.ToArray();//new string[] { "StaterpillarLiquid" };            
                PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
                primaryElement.SetElement(SimHashes.Creature);
                primaryElement.Temperature = (float)(((double)zoologicalComet.temperatureRange.x + (double)zoologicalComet.temperatureRange.y) / 2.0);
                KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
                kbatchedAnimController.AnimFiles = new KAnimFile[1]
                {
                //Assets.GetAnim((HashedString) "meteor_gassymoo_kanim")
                //Assets.GetAnim((HashedString) "caterpillar_kanim")
                Assets.GetAnim((HashedString) "shower_question_mark_kanim")
                };
                kbatchedAnimController.isMovable = true;
                kbatchedAnimController.initialAnim = "ui";
                kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
                kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
                entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
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
