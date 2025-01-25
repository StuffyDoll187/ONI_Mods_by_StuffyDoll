using HarmonyLib;
using Klei.AI;

namespace Fartomic_Bomb
{
    public class FartomicBombMod : KMod.UserMod2
    {        
        public override void OnLoad(Harmony harmony)
        {            
            base.OnLoad(harmony);
            Config.LoadConfig();
            bool SpacedOut = DlcManager.IsContentSubscribed("EXPANSION1_ID");            
            FartomicBomb.InitEmitElementsList(SpacedOut);
            CreateTraits();
                                       
        }

        public void CreateTraits()
        {
            TUNING.TRAITS.TRAIT_CREATORS.Add(TraitUtil.CreateComponentTrait<FartomicBomb>("FartomicBomb", "Fartomic Bomb", "This Duplicant emits " + Config.config.EmitMassKg.ToString() + "Kg of a random gas with an average frequency of " + Config.config.FrequencyAverageInCycles + " cycles"));
        }
    }
}
