﻿using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace Upgradeable_Dupes_And_Critters
{
    
        public class CritterUpgraderConfig : IBuildingConfig
        {
            public const string ID = "CritterUpgrader";
            public const string CODEX_ENTRY_ID = "STORYTRAITCRITTERMANIPULATOR";
            public const string INITIAL_LORE_UNLOCK_ID = "story_trait_critter_manipulator_initial";
            public const string PARKING_LORE_UNLOCK_ID = "story_trait_critter_manipulator_parking";
            public const string COMPLETED_LORE_UNLOCK_ID = "story_trait_critter_manipulator_complete";
            private const int HEIGHT = 4;

            public override BuildingDef CreateBuildingDef()
            {
                float[] tieR5_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
                string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
                EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
                EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
                EffectorValues noise = tieR5_2;
                BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CritterUpgrader", 3, 4, "gravitas_critter_manipulator_kanim", 250, 120f, tieR5_1, refinedMetals, 3200f, BuildLocationRule.OnFloor, tieR2, noise);
                buildingDef.ExhaustKilowattsWhenActive = 0.0f;
                buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
                buildingDef.Floodable = false;
                buildingDef.Entombable = true;
                buildingDef.Overheatable = false;
                buildingDef.AudioCategory = "Metal";
                buildingDef.AudioSize = "medium";
                buildingDef.ForegroundLayer = Grid.SceneLayer.Ground;
                //buildingDef.ShowInBuildMenu = false;
                return buildingDef;
            }

            public override void DoPostConfigureComplete(GameObject go)
            {
                Storage storage = go.AddOrGet<Storage>();
                storage.capacityKg = 2f;
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
                manualDeliveryKg.MinimumMass = 1f;
                manualDeliveryKg.FillToCapacity = true;
                manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
                go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
                PrimaryElement component = go.GetComponent<PrimaryElement>();
                component.SetElement(SimHashes.Steel);
                component.Temperature = 294.15f;
                //BuildingTemplates.ExtendBuildingToGravitas(go);
                //go.AddComponent<Storage>();
                Activatable activatable = go.AddComponent<Activatable>();
                activatable.synchronizeAnims = false;
                activatable.overrideAnims = new KAnimFile[1]
                {
                    Assets.GetAnim((HashedString) "anim_use_remote_kanim")
                };
                activatable.SetWorkTime(30f);
                GravitasCreatureManipulator.Def def1 = go.AddOrGetDef<GravitasCreatureManipulator.Def>();
                def1.pickupOffset = new CellOffset(-1, 0);
                def1.dropOffset = new CellOffset(1, 0);
                def1.numSpeciesToUnlockMorphMode = 1;
                def1.workingDuration = 15f;
                def1.cooldownDuration = 60f;
                MakeBaseSolid.Def def2 = go.AddOrGetDef<MakeBaseSolid.Def>();
                def2.solidOffsets = new CellOffset[4];
                for (int y = 0; y < 4; ++y)
                    def2.solidOffsets[y] = new CellOffset(0, y);
                go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn)(game_object => game_object.GetComponent<Activatable>().SetOffsets(OffsetGroups.LeftOrRight));
            }

            /*public static Option<string> GetBodyContentForSpeciesTag(Tag species)
            {
                Option<string> nameForSpeciesTag = GravitasCreatureManipulatorConfig.GetNameForSpeciesTag(species);
                Option<string> descriptionForSpeciesTag = GravitasCreatureManipulatorConfig.GetDescriptionForSpeciesTag(species);
                return nameForSpeciesTag.HasValue && descriptionForSpeciesTag.HasValue ? (Option<string>)GravitasCreatureManipulatorConfig.GetBodyContent(nameForSpeciesTag.Value, descriptionForSpeciesTag.Value) : (Option<string>)Option.None;
            }

            public static string GetBodyContentForUnknownSpecies()
            {
                return GravitasCreatureManipulatorConfig.GetBodyContent((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.UNKNOWN_TITLE, (string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.UNKNOWN);
            }

            public static string GetBodyContent(string name, string desc)
            {
                return "<size=125%><b>" + name + "</b></size><line-height=150%>\n</line-height>" + desc;
            }

            public static Option<string> GetNameForSpeciesTag(Tag species)
            {
                if (species == GameTags.Creatures.Species.HatchSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.HATCHSPECIES);
                if (species == GameTags.Creatures.Species.LightBugSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.LIGHTBUGSPECIES);
                if (species == GameTags.Creatures.Species.OilFloaterSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.OILFLOATERSPECIES);
                if (species == GameTags.Creatures.Species.DreckoSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.DRECKOSPECIES);
                if (species == GameTags.Creatures.Species.GlomSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.GLOMSPECIES);
                if (species == GameTags.Creatures.Species.PuftSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.PUFTSPECIES);
                if (species == GameTags.Creatures.Species.PacuSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.PACUSPECIES);
                if (species == GameTags.Creatures.Species.MooSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.MOOSPECIES);
                if (species == GameTags.Creatures.Species.MoleSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.MOLESPECIES);
                if (species == GameTags.Creatures.Species.SquirrelSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.SQUIRRELSPECIES);
                if (species == GameTags.Creatures.Species.CrabSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.CRABSPECIES);
                if (species == GameTags.Creatures.Species.DivergentSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.DIVERGENTSPECIES);
                if (species == GameTags.Creatures.Species.StaterpillarSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.STATERPILLARSPECIES);
                if (species == GameTags.Creatures.Species.BeetaSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.BEETASPECIES);
                if (species == GameTags.Creatures.Species.BellySpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.BELLYSPECIES);
                if (species == GameTags.Creatures.Species.SealSpecies)
                    return Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.SEALSPECIES);
                return species == GameTags.Creatures.Species.DeerSpecies ? Option.Some<string>((string)STRINGS.CREATURES.FAMILY_PLURAL.DEERSPECIES) : (Option<string>)Option.None;
            }

            public static Option<string> GetDescriptionForSpeciesTag(Tag species)
            {
                if (species == GameTags.Creatures.Species.HatchSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.HATCH);
                if (species == GameTags.Creatures.Species.LightBugSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.LIGHTBUG);
                if (species == GameTags.Creatures.Species.OilFloaterSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.OILFLOATER);
                if (species == GameTags.Creatures.Species.DreckoSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.DRECKO);
                if (species == GameTags.Creatures.Species.GlomSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.GLOM);
                if (species == GameTags.Creatures.Species.PuftSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.PUFT);
                if (species == GameTags.Creatures.Species.PacuSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.PACU);
                if (species == GameTags.Creatures.Species.MooSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.MOO);
                if (species == GameTags.Creatures.Species.MoleSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.MOLE);
                if (species == GameTags.Creatures.Species.SquirrelSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.SQUIRREL);
                if (species == GameTags.Creatures.Species.CrabSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.CRAB);
                if (species == GameTags.Creatures.Species.DivergentSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.DIVERGENTSPECIES);
                if (species == GameTags.Creatures.Species.StaterpillarSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.STATERPILLAR);
                if (species == GameTags.Creatures.Species.BeetaSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.BEETA);
                if (species == GameTags.Creatures.Species.BellySpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.ICEBELLY);
                if (species == GameTags.Creatures.Species.SealSpecies)
                    return Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.SEAL);
                return species == GameTags.Creatures.Species.DeerSpecies ? Option.Some<string>((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.WOODDEER) : (Option<string>)Option.None;
            }

            public static class CRITTER_LORE_UNLOCK_ID
            {
                public static string For(Tag species)
                {
                    return "story_trait_critter_manipulator_" + species.ToString().ToLower();
                }
            }*/
        }



    }


