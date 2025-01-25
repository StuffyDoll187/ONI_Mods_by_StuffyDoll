using HarmonyLib;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TemplateClasses;
using Tuning;
using UnityEngine;
using static STRINGS.BUILDINGS.PREFABS;

namespace TUNING
{
    public class Patches
    {
        /*[HarmonyPatch(typeof(MinionConfig))]
        [HarmonyPatch("AddMinionTraits")]
        internal class MinionConfig_AddMinionTraits_Patch
        {
            private static void Postfix(Modifiers modifiers)
            {
                modifiers.initialTraits.Add("FastLearner");
            }
        }*/

        [HarmonyPatch(typeof(MinionStartingStats))]
        [HarmonyPatch("ApplyTraits")]
        internal class MinionStartingStats_ApplyTraits_Patch
        {
            private static void Postfix(GameObject go)
            {
                if (Config.Instance.Quick_Learner_On_New_Duplicants)
                {
                    Klei.AI.Traits component1 = go.GetComponent<Klei.AI.Traits>();
                    component1.Add(Db.Get().traits.Get("FastLearner"));
                }
                //modifiers.initialTraits.Add("FastLearner");
            }
        }


        /*public void ApplyTraits(GameObject go)
        {
            Klei.AI.Traits component1 = go.GetComponent<Klei.AI.Traits>();
            component1.Clear();
            foreach (Trait trait in this.Traits)
                component1.Add(trait);
            component1.Add(this.stressTrait);
            component1.Add(this.joyTrait);
            go.GetComponent<MinionIdentity>().SetStickerType(this.stickerType);
            MinionIdentity component2 = go.GetComponent<MinionIdentity>();
            component2.SetName(this.Name);
            component2.nameStringKey = this.NameStringKey;
            go.GetComponent<MinionIdentity>().SetGender(this.GenderStringKey);
        }*/


    }
}