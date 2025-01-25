﻿using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Chaos_Reigns
{
    internal class TwitchSpongeSlugCometConfig : IEntityConfig
    {
        public static string ID = "TwitchMoltenSpongeSlugComet";

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public GameObject CreatePrefab()
        {
            GameObject entity = EntityTemplates.CreateEntity(ID, "Molten Sponge Slug Comet");
            entity.AddOrGet<SaveLoadRoot>();
            entity.AddOrGet<LoopingSounds>();
            TwitchSpongeSlugComet gassyMooComet = entity.AddOrGet<TwitchSpongeSlugComet>();
            gassyMooComet.massRange = new Vector2(100f, 200f);
            gassyMooComet.EXHAUST_ELEMENT = SimHashes.Hydrogen;
            gassyMooComet.temperatureRange = new Vector2(296.15f, 318.15f);
            gassyMooComet.entityDamage = 0;
            gassyMooComet.explosionOreCount = new Vector2I(0, 0);
            gassyMooComet.totalTileDamage = 0.0f;
            gassyMooComet.splashRadius = 1;
            //gassyMooComet.impactSound = "Meteor_GassyMoo_Impact";
            gassyMooComet.flyingSoundID = 0;
            gassyMooComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
            gassyMooComet.addTiles = 0;
            gassyMooComet.affectedByDifficulty = false;
            gassyMooComet.lootOnDestroyedByMissile = new string[3]
            {
      "Meat",
      "Meat",
      "Meat"
            };
            gassyMooComet.destroyOnExplode = false;
            gassyMooComet.craterPrefabs = new string[1] { "StaterpillarLiquid" };
            PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
            primaryElement.SetElement(SimHashes.Creature);
            primaryElement.Temperature = (float)(((double)gassyMooComet.temperatureRange.x + (double)gassyMooComet.temperatureRange.y) / 2.0);
            KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
            kbatchedAnimController.AnimFiles = new KAnimFile[1]
            {
                //Assets.GetAnim((HashedString) "meteor_gassymoo_kanim")
                Assets.GetAnim((HashedString) "caterpillar_kanim")
            };
            kbatchedAnimController.isMovable = true;
            kbatchedAnimController.initialAnim = "wtr_ui";
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
