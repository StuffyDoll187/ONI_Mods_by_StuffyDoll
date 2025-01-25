using HarmonyLib;
using System;
using UnityEngine;
using Klei.AI;
using TUNING;

namespace Neural_Vacillator_Assignment_Precondition
{
    public class Neural_Vacillator_Precondition
    {
        [HarmonyPatch(typeof(GeneShuffler), "OnPrefabInit")]
        public class GeneShuffler_OnPrefabInit_Patch
        {
            public static void Postfix(GeneShuffler __instance)
            {
                __instance.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(CanHaveMoreGeneShufflerTraits));
            }
        }       

        public static bool CanHaveMoreGeneShufflerTraits(MinionAssignablesProxy proxy)
        {
            GameObject go = proxy.GetTargetGameObject();
            Traits component = go.GetComponent<Traits>();
            bool flag = true;            
            foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.GENESHUFFLERTRAITS)
            {
                bool flag2 = true;
                if (!component.HasTrait(traitVal.id))
                    flag2 = false;
                flag = flag && flag2;                    
            }            
            return !flag;
        }
    }
}
