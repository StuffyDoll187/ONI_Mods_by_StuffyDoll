using HarmonyLib;
using STRINGS;
using UnityEngine;
using System;
using KMod;
using Klei.AI;
using TUNING;
using System.Runtime.CompilerServices;
using static ResearchTypes;
using System.Collections.Generic;
using System.Reflection;
using static BionicUpgradesMonitor;
using PeterHan.PLib.UI;

namespace Upgradeable_Dupes_And_Critters
{
    public class Patches
    {
        

        


        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public class Db_Initialize_Patch
        {

            public static void Postfix()
            {
                DBInit.CreateTraits();
                DBInit.CreateAssignableSlots();
                Db.Get().Techs.Get("NuclearResearch").unlockedItemIDs.Add("DuplicantUpgrader");
                Db.Get().Techs.Get("Bioengineering").unlockedItemIDs.Add("CritterUpgrader");
                Db.Get().Techs.Get("MedicineIV").unlockedItemIDs.Add("InjectionChamber");
                Db.Get().Techs.Get("MedicineIV").unlockedItemIDs.Add("NuclearApothecary");
                Db.Get().Techs.Get("FineDining").unlockedItemIDs.Add("UpgradedEggCracker");
                
            }
        }




        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        internal class MoreTempshiftPlatesGeneratedBuildingsLoadGeneratedBuildings
        {
            private static void Prefix()
            {
                /*Strings.Add("STRINGS.BUILDINGS.PREFABS.DUPLICANTUPGRADER.NAME", UI.FormatAsLink("Duplicant Upgrader", ""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.DUPLICANTUPGRADER.EFFECT", "Massively enhances Duplicants at the cost of increased Oxygen and Calorie consumption.\n\nRequires the GlowStick trait.");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.DUPLICANTUPGRADER.DESC", "Usable up to " + DuplicantUpgradeTracker.MaxUpgradeLevel.ToString() + " times per Duplicant.\n\nGlowStick is consumed on use.");

                Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERUPGRADER.NAME", UI.FormatAsLink("Critter Upgrader", ""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERUPGRADER.EFFECT", "Use Radioactive Serum to supercharge a Critter's Metabolism and Shearable Resource Growth Rate.\n\nDNA Modification propagates to offspring.");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERUPGRADER.DESC", "Modified Critters also drop increased resources on death\n\nRequires Tamed Critter");

                Strings.Add("STRINGS.BUILDINGS.PREFABS.INJECTIONCHAMBER.NAME", UI.FormatAsLink("Injection Chamber", ""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.INJECTIONCHAMBER.EFFECT", "A place to inject Duplicants with Radioactive Serum\n\nGives the GlowStick trait.");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.INJECTIONCHAMBER.DESC", "Requires a Doctor with Advanced Medical Care.");

                Strings.Add("STRINGS.BUILDINGS.PREFABS.NUCLEARAPOTHECARY.NAME", UI.FormatAsLink("Nuclear Apothecary", ""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.NUCLEARAPOTHECARY.EFFECT", "Compound Radioactive Serum for use in modifying Duplicant and Critter DNA.");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.NUCLEARAPOTHECARY.DESC", "Consumes Radbolts.");

                Strings.Add("STRINGS.BUILDINGS.PREFABS.UPGRADEDEGGCRACKER.NAME", UI.FormatAsLink("Upgraded Egg Cracker", ""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.UPGRADEDEGGCRACKER.EFFECT", "Able to crack upgraded eggs for extra Raw Egg and Egg Shells.\n\n" + STRINGS.BUILDINGS.PREFABS.EGGCRACKER.EFFECT);
                Strings.Add("STRINGS.BUILDINGS.PREFABS.UPGRADEDEGGCRACKER.DESC", STRINGS.BUILDINGS.PREFABS.EGGCRACKER.DESC);*/

                ModUtil.AddBuildingToPlanScreen("Equipment", "DuplicantUpgrader", "industrialstation", "ResetSkillsStation");
                TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("DuplicantUpgrader", "industrialstation");
                ModUtil.AddBuildingToPlanScreen("Equipment", "CritterUpgrader", "industrialstation", "ResetSkillsStation");
                TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("CritterUpgrader", "industrialstation");
                ModUtil.AddBuildingToPlanScreen("Medical", "InjectionChamber", "medical", "AdvancedDoctorStation");
                TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("InjectionChamber", "industrialstation");
                ModUtil.AddBuildingToPlanScreen("Medical", "NuclearApothecary", "medical", "Apothecary");
                TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("NuclearApothecary", "industrialstation");
                ModUtil.AddBuildingToPlanScreen("Food", "UpgradedEggCracker", "ranching", "EggCracker");
                TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("UpgradedEggCracker", "ranching");

            }
        }

        [HarmonyPatch(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToBasicCreature), new Type[]
        {
            typeof(bool ) ,
            typeof(GameObject),
            typeof(FactionManager.FactionID),
            typeof(string) ,
            typeof(string) ,
            typeof(NavType) ,
            typeof(int) ,
            typeof(float ),
            typeof(string) ,
            typeof(int),
            typeof(bool ) ,
            typeof(bool) ,
            typeof(float ),
            typeof(float ),
            typeof(float ),
            typeof(float )
        })]
        class EntityTemplates_ExtendEntityToBasicCreature
        {
            static void Postfix(ref GameObject __result,
              bool isWarmBlooded,
              GameObject template,
              FactionManager.FactionID faction,
              string initialTraitID,
              string NavGridName,
              NavType navType,
              int max_probing_radius,
              float moveSpeed,
              string onDeathDropID,
              int onDeathDropCount,
              bool drownVulnerable,
              bool entombVulnerable,
              float warningLowTemperature,
              float warningHighTemperature,
              float lethalLowTemperature,
              float lethalHighTemperature)
            {
                __result.AddComponent<CritterUpgradeTracker>();
            }
        }

        [HarmonyPatch(typeof(EggConfig), nameof(EggConfig.CreateEgg), new Type[]
        {
            typeof(string),
            typeof(string),
            typeof(string) ,
            typeof(Tag) ,
            typeof(string) ,
            typeof(float) ,
            typeof(int ),
            typeof(float) ,
            typeof(string[]),
            typeof(string[])
        })]
        class EggConfig_CreateEgg
        {
            static void Postfix(ref GameObject __result, string id,string name,string desc,Tag creature_id,string anim,float mass,int egg_sort_order,float base_incubation_rate,string[] requiredDlcIds, string[] forbiddenDlcIds)
            {
                __result.AddComponent<CritterUpgradeTracker>();
                EggConfig_CreateEgg.CreateEggRecipes(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, requiredDlcIds, forbiddenDlcIds);
            }

            static void CreateEggRecipes(string id,string name,string desc,Tag creature_id,string anim,float mass,int egg_sort_order,float base_incubation_rate,string[] requiredDlcIds, string[] forbiddenDlcIds)
            {
                //string str1 = string.Format((string)STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RESULT_DESCRIPTION, (object)name);
                string str1 = string.Format((string) Strings.Get("STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RESULT_DESCRIPTION"), (object)name);
                ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement((Tag) id, 1f)
                };
                ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[2]
                {
                    new ComplexRecipe.RecipeElement((Tag) "RawEgg", 0.5f * mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
                    new ComplexRecipe.RecipeElement((Tag) "EggShell", 0.5f * mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
                };
                    string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID(id, (Tag)"RawEgg");
                    string str2 = ComplexRecipeManager.MakeRecipeID("UpgradedEggCracker", (IList<ComplexRecipe.RecipeElement>)recipeElementArray1, (IList<ComplexRecipe.RecipeElement>)recipeElementArray2);
                ComplexRecipe complexRecipe = new ComplexRecipe(str2, recipeElementArray1, recipeElementArray2, requiredDlcIds, forbiddenDlcIds)
                {
                    //description = string.Format((string)STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object)name, (object)str1),
                    description = string.Format((string) Strings.Get("STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION"), (object)name, (object)str1),
                    fabricators = new List<Tag>() { (Tag)"UpgradedEggCracker" },
                    time = 5f
                };
                ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str2);
            }

        }

        /*[HarmonyPatch(typeof(EggCrackerConfig))]
        [HarmonyPatch(nameof(EggCrackerConfig.InstanceureBuildingTemplate))]
        public class EggCrackerConfig_ConfigureBuildingTemplate
        {

            public static void Postfix(GameObject go)
            {
                go.AddComponent<UpgradedEggCracker>();
            }
        }*/

        [HarmonyPatch(typeof(IncubationMonitor))]
        [HarmonyPatch("SpawnBaby")]
        internal class IncubationMonitor_SpawnBaby
        {
            private static bool Prefix(IncubationMonitor.Instance smi)
            {
                if (!Config.Instance.Enable_Critter_Upgrade_Propagation_To_Offspring)
                    return true;
                Vector3 position = new Vector3();
                position = smi.transform.GetPosition();
                /*with
            {
                z = Grid.GetLayerZ(Grid.SceneLayer.Creatures)
            };*/
                position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
                GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(smi.def.spawnedCreature), position);
                gameObject.SetActive(true);
                gameObject.GetSMI<AnimInterruptMonitor.Instance>().Play("hatching_pst");
                KSelectable component = smi.gameObject.GetComponent<KSelectable>();
                if ((UnityEngine.Object)SelectTool.Instance != (UnityEngine.Object)null && (UnityEngine.Object)SelectTool.Instance.selected != (UnityEngine.Object)null && (UnityEngine.Object)SelectTool.Instance.selected == (UnityEngine.Object)component)
                    SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>());
                Db.Get().Amounts.Wildness.Copy(gameObject, smi.gameObject);
                CritterUpgradeTracker eggcmp = smi.gameObject.GetComponent<CritterUpgradeTracker>();
                //Debug.Log(eggcmp.Upgrades);
                //Debug.Log(smi.gameObject.name + "  " + gameObject.name);
                CritterUpgradeTracker cmp = gameObject.GetComponent<CritterUpgradeTracker>();
                //Debug.Log(cmp.Upgrades);
                cmp.Upgrades = eggcmp.Upgrades;
                if ((UnityEngine.Object)smi.incubator != (UnityEngine.Object)null)
                    smi.incubator.StoreBaby(gameObject);
                IncubationMonitor_SpawnBaby.SpawnShell(smi);
                SaveLoader.Instance.saveManager.Unregister(smi.GetComponent<SaveLoadRoot>());
                return false;
            }

            private static GameObject SpawnShell(IncubationMonitor.Instance smi)
            {
                Vector3 position = smi.transform.GetPosition();
                GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EggShell"), position);
                PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
                PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
                CritterUpgradeTracker cmp3 = smi.gameObject.GetComponent<CritterUpgradeTracker>();
                component.Mass = component2.Mass * 0.5f * (cmp3.Upgrades + 1);
                gameObject.SetActive(value: true);
                return gameObject;
            }
        }




        /*private static GameObject SpawnShell(Instance smi)
        {
            Vector3 position = smi.transform.GetPosition();
            GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EggShell"), position);
            PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
            PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
            component.Mass = component2.Mass * 0.5f;
            gameObject.SetActive(value: true);
            return gameObject;
        }

        private static GameObject SpawnEggInnards(Instance smi)
        {
            Vector3 position = smi.transform.GetPosition();
            GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RawEgg"), position);
            PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
            PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
            component.Mass = component2.Mass * 0.5f;
            gameObject.SetActive(value: true);
            return gameObject;
        }

        private static void SpawnGenericEgg(Instance smi)
        {
            SpawnShell(smi);
            GameObject gameObject = SpawnEggInnards(smi);
            KSelectable component = smi.gameObject.GetComponent<KSelectable>();
            if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
            {
                SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>());
            }
        }*/




        [HarmonyPatch(typeof(IncubationMonitor))]
        [HarmonyPatch("SpawnShell")]
        internal class IncubationMonitor_SpawnShell
        {
            private static GameObject Postfix(GameObject __result, IncubationMonitor.Instance smi)
            {
                CritterUpgradeTracker tracker = smi.gameObject.GetComponent<CritterUpgradeTracker>();
                PrimaryElement PE = __result.GetComponent<PrimaryElement>();
                PE.Mass *= (tracker.Upgrades + 1);
                return __result;
            }


        }

        [HarmonyPatch(typeof(IncubationMonitor))]
        [HarmonyPatch("SpawnEggInnards")]
        internal class IncubationMonitor_SpawnEggInnards
        {
            private static GameObject Postfix(GameObject __result, IncubationMonitor.Instance smi)
            {
                CritterUpgradeTracker tracker = smi.gameObject.GetComponent<CritterUpgradeTracker>();
                PrimaryElement PE = __result.GetComponent<PrimaryElement>(); 
                PE.Mass *= (tracker.Upgrades + 1);
                return __result;
            }

            
        }

        [HarmonyPatch(typeof(Butcherable))]
        [HarmonyPatch("CreateDrops")]
        internal class Butcherable_CreateDrops
        {
            private static GameObject[] Postfix(GameObject[] __result, Butcherable __instance)
            {
                //if (!Config.Instance.Enable_Critter_Upgrade_Propagation_To_Offspring)
                  //  return __result;
                
                CritterUpgradeTracker cmp = __instance.gameObject.GetComponent<CritterUpgradeTracker>();
                if (cmp.Upgrades > 0)
                {
                    for (int i = 0; i < cmp.Upgrades; ++i)
                    {
                        //GameObject[] array = new GameObject[__instance.drops.Length];
                        for (int j = 0; j < __instance.drops.Length; j++)
                        {
                            GameObject gameObject = Scenario.SpawnPrefab(Butcherable_CreateDrops.GetDropSpawnLocation(__instance), 0, 0, __instance.drops[j]);
                            gameObject.SetActive(value: true);
                            Edible component = gameObject.GetComponent<Edible>();
                            if ((bool)component)
                            {
                                ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
                            }
                        }
                    }
                }
                return __result;
            }

            private static int GetDropSpawnLocation(Butcherable instance)
            {
                int num = Grid.PosToCell(instance.gameObject);
                int num2 = Grid.CellAbove(num);
                if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
                {
                    return num2;
                }
                return num;
            }
        }

        [HarmonyPatch(typeof(HappinessMonitor))]
        [HarmonyPatch("InitializeStates")]
        internal class HappinessMonitor_InitializeStates
        {
            private static void Postfix(HappinessMonitor __instance, ref Effect ___glumTameEffect, ref Effect ___miserableTameEffect, ref HappinessMonitor.HappyState ___happy)
            {                     
                //___glumTameEffect = new Effect("Glum", (string)STRINGS.CREATURES.MODIFIERS.GLUM.NAME, (string)STRINGS.CREATURES.MODIFIERS.GLUM.TOOLTIP, 0.0f, true, false, true);
                //___glumTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -0.8f, (string)STRINGS.CREATURES.MODIFIERS.GLUM.NAME, is_multiplier: true));
                ___glumTameEffect = new Effect("Glum", (string)Strings.Get("STRINGS.CREATURES.MODIFIERS.GLUM.NAME"), (string)Strings.Get("STRINGS.CREATURES.MODIFIERS.GLUM.TOOLTIP"), 0.0f, true, false, true);
                ___glumTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -0.8f, (string)Strings.Get("STRINGS.CREATURES.MODIFIERS.GLUM.NAME"), is_multiplier: true));

                //___glumTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, (string)STRINGS.CREATURES.MODIFIERS.GLUM.NAME, is_multiplier: true));


                //___miserableTameEffect = new Effect("Miserable", (string)STRINGS.CREATURES.MODIFIERS.MISERABLE.NAME, (string)STRINGS.CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0.0f, true, false, true);
                //___miserableTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -0.8f, (string)STRINGS.CREATURES.MODIFIERS.MISERABLE.NAME, is_multiplier: true));
                ___miserableTameEffect = new Effect("Miserable", (string)Strings.Get("STRINGS.CREATURES.MODIFIERS.MISERABLE.NAME"), (string)Strings.Get("STRINGS.CREATURES.MODIFIERS.MISERABLE.TOOLTIP"), 0.0f, true, false, true);
                ___miserableTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -0.8f, (string)Strings.Get("STRINGS.CREATURES.MODIFIERS.MISERABLE.NAME"), is_multiplier: true));

                //___miserableTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, (string)STRINGS.CREATURES.MODIFIERS.GLUM.NAME, is_multiplier: true));
            }            
        }

        [HarmonyPatch(typeof(BaseMinionConfig), nameof(BaseMinionConfig.BaseMinion))]
        //[HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
        internal class MinionConfig_CreatePrefab_Patch
        {
            private static GameObject Postfix(GameObject __result)
            {
                __result.AddComponent<DuplicantUpgradeTracker>();
                if (Config.Instance.Enable_Mastered_Skills_Attribute_Bonus_For_Unused_Skill_Points)
                __result.AddComponent<UnusedSkillPointsMasteryBonus>();

                return __result;
            }
        }

        [HarmonyPatch(typeof(AttributeLevel))]
        [HarmonyPatch("AddExperience")]
        public class AttributeLevel_AddExperience
        {

            public static bool Postfix(bool __result, AttributeLevel __instance, AttributeLevels levels, float experience)
            {
                if (__instance.level < DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL)
                    return __result;
                GameObject go = levels.gameObject;
                DuplicantUpgradeTracker tracker = go.GetComponent<DuplicantUpgradeTracker>();
                if (__instance.level >= DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL + (tracker.Upgrades * Config.Instance.Max_Attribute_Level_Per_Upgrade))
                    return false;
                __instance.experience += experience;
                __instance.experience = Mathf.Max(0.0f, __instance.experience);
                /*if ((double)__instance.experience < (double)__instance.GetExperienceForNextLevel())
                    return false;*/
                if ((double)__instance.experience < (double)(TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600 / TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL))
                    return false;
                __instance.LevelUp(levels);
                return true;
            }
        }

        [HarmonyPatch(typeof(Edible))]
        [HarmonyPatch("GetFeedingTime")]
        public class Edible_GetFeedingTime
        {

            public static float Postfix(float __result, WorkerBase worker)
            {
                DuplicantUpgradeTracker tracker = worker.GetComponent<DuplicantUpgradeTracker>();
                if (tracker.Upgrades > 0 && Config.Instance.Calorie_Consumption_Per_Upgrade != 0)
                    return __result / ((tracker.Upgrades + 1) * (Config.Instance.Calorie_Consumption_Per_Upgrade / 1000f));
                return __result;
            }
        }


        
        [HarmonyPatch(typeof(BionicMassOxygenAbsorbChore), nameof(BionicMassOxygenAbsorbChore.AbsorbUpdate))]
        public class BionicMassOxygenAbsorbChore_AbsorbUpdate_Patch
        {
            private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
            {
                BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = (BionicMassOxygenAbsorbChore.AbsorbUpdateData)data;
                absorbUpdateData.smi.OnSimConsume(mass_cb_info, absorbUpdateData.dt);
            }
            public static bool Prefix(BionicMassOxygenAbsorbChore.Instance smi, float dt)
            {
                //Debug.Log(smi.oxygenTankMonitor.storage.capacityKg + " " + smi.oxygenTankMonitor.SpaceAvailableInTank);
                float mass = Mathf.Min(dt * /*BionicMassOxygenAbsorbChore.ABSORB_RATE*/ smi.oxygenTankMonitor.storage.capacityKg / BionicMassOxygenAbsorbChore.ABSORB_RATE_IDEAL_CHORE_DURATION, smi.oxygenTankMonitor.SpaceAvailableInTank);
                BionicMassOxygenAbsorbChore.AbsorbUpdateData callback_data = new BionicMassOxygenAbsorbChore.AbsorbUpdateData(smi, dt);
                int elementCell;
                SimHashes breathableElement = BionicMassOxygenAbsorbChore.GetNearBreathableElement(elementCell = Grid.PosToCell(smi.sm.dupe.Get(smi)), BionicMassOxygenAbsorbChore.ABSORB_RANGE, out elementCell);
                HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(OnSimConsumeCallback), (object)callback_data, nameof(BionicMassOxygenAbsorbChore));
                SimMessages.ConsumeMass(elementCell, breathableElement, mass, (byte)3, handle.index);

                return false;
            }
        }

        [HarmonyPatch(typeof(FindAndConsumeOxygenSourceChore), nameof(FindAndConsumeOxygenSourceChore.GetConsumeDuration))]
        public class FindAndConsumeOxygenSourceChore_GetConsumeDuration_Patch
        {
            public static void Postfix(ref float __result, FindAndConsumeOxygenSourceChore.Instance smi)
            {
                //Debug.Log("Original " +__result); 
                //return Mathf.Max(24f * (smi.sm.actualunits.Get(smi) / BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG), 4.333f);
                __result = Mathf.Max(24f * (smi.sm.actualunits.Get(smi) / smi.oxygenTankMonitor.storage.capacityKg), Mathf.Min(4.333f, 4.333f / (smi.oxygenTankMonitor.storage.capacityKg / BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG)));
                //Debug.Log("New " + __result);
            }
        }





        

    }
}
