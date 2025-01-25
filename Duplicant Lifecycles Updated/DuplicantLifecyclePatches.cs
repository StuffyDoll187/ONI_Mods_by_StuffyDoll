using System;
using System.Collections.Generic;
using Database;
//using Harmony;
using HarmonyLib;
using Klei.AI;
using TUNING;
using Zolibrary.Logging;
using static STRINGS.UI.NEWBUILDCATEGORIES;

namespace DuplicantLifecycles
{
    public class DuplicantLifeCyclePatches
    {
        public static Death oldAgeDeath;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("DuplicantLifeCycles", "1.0.8");                
                LogManager.LogInit();                
            }
        }

        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {                
                Trait aging_trait = Db.Get().CreateTrait(
                    id: (string)DuplicantLifecycleStrings.AgingID,
                    name: (string)DuplicantLifecycleStrings.AgingNAME,
                    description: (string)DuplicantLifecycleStrings.AgingDESC,
                    group_name: null,
                    should_save: true,
                    disabled_chore_groups: null,
                    positive_trait: true,
                    is_valid_starter_trait: false
                    );
                aging_trait.OnAddTrait = (go => go.FindOrAddUnityComponent<AgingTrait>());

                if (!DuplicantLifecycleConfigChecker.EnableImmortalTrait)
                    return;

                Trait immortal_trait = Db.Get().CreateTrait(
                    id: (string)DuplicantLifecycleStrings.ImmortalID,
                    name: (string)DuplicantLifecycleStrings.ImmortalNAME,
                    description: (string)DuplicantLifecycleStrings.ImmortalDESC,
                    group_name: null,
                    should_save: true,
                    disabled_chore_groups: null,
                    positive_trait: true,
                    is_valid_starter_trait: false
                    );
                immortal_trait.OnAddTrait = (go => go.FindOrAddUnityComponent<ImmortalTrait>());

                DUPLICANTSTATS.GENESHUFFLERTRAITS.Add(new DUPLICANTSTATS.TraitVal() { id = DuplicantLifecycleStrings.ImmortalID });

                /*Traverse.Create<DUPLICANTSTATS>().Field("GENESHUFFLERTRAITS").SetValue(
                    new List<DUPLICANTSTATS.TraitVal>() {
                        new DUPLICANTSTATS.TraitVal() { id = "Regeneration" },
                        new DUPLICANTSTATS.TraitVal() { id = "DeeperDiversLungs" },
                        new DUPLICANTSTATS.TraitVal() { id = "SunnyDisposition" },
                        new DUPLICANTSTATS.TraitVal() { id = "RockCrusher" },
                        new DUPLICANTSTATS.TraitVal() { id = DuplicantLifecycleStrings.ImmortalID }
                    });*/
            }
        }


        [HarmonyPatch(typeof(Deaths), MethodType.Constructor, typeof(ResourceSet))]
        public class Deaths_Constructor_Postfix
        {
            public static void Postfix(Deaths __instance)
            {
                DuplicantLifeCyclePatches.oldAgeDeath = new Death(
                        id: "DuplicantLifecycles.Aging.OldAgeDeath",
                        parent: (ResourceSet)Db.Get().Deaths,
                        name: "Old Age",
                        description: DuplicantLifecycleStrings.DeathMessage,
                        pre_anim: "death_suffocation",
                        loop_anim: "dead_on_back"
                    );

                __instance.Add(oldAgeDeath);
            }
        }

        [HarmonyPatch(typeof(Traits), "OnSpawn")]
        public class Traits_OnSpawn_Patch
        {
            private static bool MatchesAgingTraitId(string str)
            {
                return str.Equals("Aging") || str.Equals((string)DuplicantLifecycleStrings.AgingID);
            }
            public static void Prefix(Traits __instance)
            {                                
                if (__instance.TryGetComponent(out MinionIdentity identity))
                {                                                                                        
                    List<string> tempIds = __instance.GetTraitIds();
                    tempIds.RemoveAll(new Predicate<string>(MatchesAgingTraitId));

                    if (identity.model == MinionConfig.MODEL || (identity.model == BionicMinionConfig.MODEL && DuplicantLifecycleConfigChecker.config.ApplyAgingToBionics))
                    {
                        if (!tempIds.Contains(DuplicantLifecycleStrings.AgingID))
                            tempIds.Add(DuplicantLifecycleStrings.AgingID);
#if DEBUG
                    foreach(string s in tempIds)
                        LogManager.LogDebug(__instance.name + ": " + s);
#endif

                        __instance.SetTraitIds(tempIds);
                    }
                }
            }
        }
    }
}