using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Upgradeable_Dupes_And_Critters
{
    internal class UnusedSkillPointsMasteryBonus : KMonoBehaviour, ISim4000ms
    {
        [MyCmpGet]
        MinionResume resume;
        [MyCmpGet]
        Effects effectscmp;
        //[SerializeField]
        //[Serialize]
        public int unusedSkillPoints;        
        //[SerializeField]
        //[Serialize]
        public int masteredSkillBranches;
        
        public List<Effect> currentMasteryEffects = new List<Effect>();




        public Effect DiggingMasteryEffect;
        public Effect BuildingMasteryEffect;
        public Effect FarmingMasteryEffect;
        public Effect RanchingMasteryEffect;
        public Effect CookingMasteryEffect;
        public Effect DecoratingMasteryEffect;
        public Effect ResearchingMasteryEffect;
        public Effect PilotingMasteryEffect;
        public Effect SuitWearingMasteryEffect;
        public Effect HaulingMasteryEffect;
        public Effect OperatingMasteryEffect;
        public Effect MedicineMasteryEffect;
        public Effect TidyingMasteryEffect;

        public List<string> masterySkills = new List<string>
        {
            "Mining4",
            "Building3",
            "Farming3",
            "Ranching2",
            "RocketPiloting2",
            "Cooking2",
            "Arting3",
            //"Suits1",
            "Engineering1",
            "Pyrotechnics",
            "Medicine3"
        };
        public List<string> researchMasterySkills = new List<string>
        {
            "AtomicResearch",
            "SpaceResearch"
        };

        protected override void OnSpawn()
        {
            //MinionResume resume = this.GetComponent<MinionResume>();
            this.unusedSkillPoints = this.resume.AvailableSkillpoints;
            this.masteredSkillBranches = this.GetNumberOfMasteredBranches(resume);
            if (this.masteredSkillBranches == 0)
                return;
            //Effects effectscomponent = this.GetComponent<Effects>();
            
            List<Effect> effects = this.GetMasteryEffects(resume);
            this.ApplyMasteryEffects(effects);
        }

        private int GetNumberOfMasteredBranches(MinionResume resume)
        {
            int num = 0;
            foreach (string id in this.masterySkills)
            {
                if (resume.HasMasteredSkill(id))
                    num++;
            }
            bool flag1 = true;
            for (int i = 0; i < researchMasterySkills.Count; i++)
            {
                bool flag2 = resume.HasMasteredSkill(researchMasterySkills[i]);
                flag1 = flag1 && flag2;
            }
            if (flag1)
                num++;

            //if (resume.HasMasteredSkill(researchMasterySkills[0]) && resume.HasMasteredSkill(researchMasterySkills[1]))
              //  num++;
            return num;
        }
        private List<Effect> GetMasteryEffects(MinionResume resume)
        {
            
            List<Effect> effects = new List<Effect>();                       
            int bonus = Math.DivRem(this.unusedSkillPoints, this.masteredSkillBranches, out int remainder);
            if (bonus >= Config.Instance.Max_Mastery_Bonus)
                remainder = 0;
            bonus = Math.Min(bonus, Config.Instance.Max_Mastery_Bonus);
            if (resume.HasMasteredSkill("AtomicResearch") && resume.HasMasteredSkill("SpaceResearch"))
            {
                this.ResearchingMasteryEffect = new Effect("ResearchingMastery", STRINGS.MASTERYEFFECTS.RESEARCHER.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.ResearchingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Learning.Id, bonus, STRINGS.MASTERYEFFECTS.RESEARCHER.DESC));
                else
                {
                    this.ResearchingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Learning.Id, bonus + 1, STRINGS.MASTERYEFFECTS.RESEARCHER.DESC));
                    remainder--;
                }
                effects.Add(this.ResearchingMasteryEffect);
            }
            if (resume.HasMasteredSkill("Building3"))
            {
                this.BuildingMasteryEffect = new Effect("BuildingMastery", STRINGS.MASTERYEFFECTS.BUILDER.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.BuildingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Construction.Id, bonus, STRINGS.MASTERYEFFECTS.BUILDER.DESC));
                else
                {
                    this.BuildingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Construction.Id, bonus + 1, STRINGS.MASTERYEFFECTS.BUILDER.DESC));
                    remainder--;
                }
                effects.Add(this.BuildingMasteryEffect);
            }
            if (resume.HasMasteredSkill("Engineering1"))
            {
                this.OperatingMasteryEffect = new Effect("OperatingMastery", STRINGS.MASTERYEFFECTS.OPERATOR.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.OperatingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Machinery.Id, bonus, STRINGS.MASTERYEFFECTS.OPERATOR.DESC));
                else
                {
                    this.OperatingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Machinery.Id, bonus + 1, STRINGS.MASTERYEFFECTS.OPERATOR.DESC));
                    remainder--;
                }
                effects.Add(this.OperatingMasteryEffect);
            }            
            if (resume.HasMasteredSkill("Mining4"))
            {
                this.DiggingMasteryEffect = new Effect("DiggingMastery", STRINGS.MASTERYEFFECTS.DIGGER.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)                                
                    this.DiggingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Digging.Id, bonus, STRINGS.MASTERYEFFECTS.DIGGER.DESC));
                else
                {
                    this.DiggingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Digging.Id, bonus + 1, STRINGS.MASTERYEFFECTS.DIGGER.DESC));
                    remainder--;
                }

                effects.Add(this.DiggingMasteryEffect);
            }            
            if (resume.HasMasteredSkill("Farming3"))
            {
                this.FarmingMasteryEffect = new Effect("FarmingMastery", STRINGS.MASTERYEFFECTS.FARMER.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.FarmingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Botanist.Id, bonus, STRINGS.MASTERYEFFECTS.FARMER.DESC));
                else
                {
                    this.FarmingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Botanist.Id, bonus + 1, STRINGS.MASTERYEFFECTS.FARMER.DESC));
                    remainder--;
                }
                effects.Add(this.FarmingMasteryEffect);
            }
            if (resume.HasMasteredSkill("Ranching2"))
            {
                this.RanchingMasteryEffect = new Effect("RanchingMastery", STRINGS.MASTERYEFFECTS.RANCHER.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.RanchingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Ranching.Id, bonus, STRINGS.MASTERYEFFECTS.RANCHER.DESC));
                else
                {
                    this.RanchingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Ranching.Id, bonus + 1, STRINGS.MASTERYEFFECTS.RANCHER.DESC));
                    remainder--;
                }
                effects.Add(this.RanchingMasteryEffect);
            }
            if (resume.HasMasteredSkill("Cooking2"))
            {
                this.CookingMasteryEffect = new Effect("CookingMastery", STRINGS.MASTERYEFFECTS.COOK.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.CookingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Cooking.Id, bonus, STRINGS.MASTERYEFFECTS.COOK.DESC));
                else
                {
                    this.CookingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Cooking.Id, bonus + 1, STRINGS.MASTERYEFFECTS.COOK.DESC));
                    remainder--;
                }
                effects.Add(this.CookingMasteryEffect);
            }
            if (resume.HasMasteredSkill("RocketPiloting2"))
            {
                this.PilotingMasteryEffect = new Effect("PilotingMastery", STRINGS.MASTERYEFFECTS.PILOT.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.PilotingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.SpaceNavigation.Id, bonus, STRINGS.MASTERYEFFECTS.PILOT.DESC));
                else
                {
                    this.PilotingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.SpaceNavigation.Id, bonus + 1, STRINGS.MASTERYEFFECTS.PILOT.DESC));
                    remainder--;
                }
                effects.Add(this.PilotingMasteryEffect);
            }                                                
            if (resume.HasMasteredSkill("Pyrotechnics"))
            {
                this.TidyingMasteryEffect = new Effect("TidyingMastery", STRINGS.MASTERYEFFECTS.TIDIER.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.TidyingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Strength.Id, bonus, STRINGS.MASTERYEFFECTS.TIDIER.DESC));
                else
                {
                    this.TidyingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Strength.Id, bonus + 1, STRINGS.MASTERYEFFECTS.TIDIER.DESC));
                    remainder--;
                }
                effects.Add(this.TidyingMasteryEffect);
            }
            if (resume.HasMasteredSkill("Medicine3"))
            {
                this.MedicineMasteryEffect = new Effect("MedicineMastery", STRINGS.MASTERYEFFECTS.DOCTOR.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.MedicineMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Caring.Id, bonus, STRINGS.MASTERYEFFECTS.DOCTOR.DESC));
                else
                {
                    this.MedicineMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Caring.Id, bonus, STRINGS.MASTERYEFFECTS.DOCTOR.DESC));
                    remainder--;
                }
                effects.Add(this.MedicineMasteryEffect);
            }
            if (resume.HasMasteredSkill("Arting3"))
            {
                this.DecoratingMasteryEffect = new Effect("DecoratingMastery", STRINGS.MASTERYEFFECTS.DECORATOR.NAME, STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                if (remainder <= 0)
                    this.DecoratingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Art.Id, bonus, STRINGS.MASTERYEFFECTS.DECORATOR.DESC));
                else
                {
                    this.DecoratingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Art.Id, bonus + 1, STRINGS.MASTERYEFFECTS.DECORATOR.DESC));
                    remainder--;
                }
                effects.Add(this.DecoratingMasteryEffect);
            }
            /*if (resume.HasMasteredSkill("Hauling2"))
            {
                this.HaulingMasteryEffect = new Effect("HaulingMastery", "Master Supplier", STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                this.HaulingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Strength.Id, bonus, "Hauling Mastery"));
                effects.Add(this.HaulingMasteryEffect);
            }*/
            /*if (resume.HasMasteredSkill("Suits1"))
            {
                this.SuitWearingMasteryEffect = new Effect("SuitWearingMastery", "Master Suit Wearer", STRINGS.MASTERYEFFECTS.DESC, 0.0f, true, false, false);
                this.SuitWearingMasteryEffect.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, bonus, "SuitWearing Mastery"));
                effects.Add(this.SuitWearingMasteryEffect);
            }*/
            //Effect AthleticsRemainder = new Effect("AthleticsRemainder", "AthleticsRemainder", "", 0.0f, false, false, false);
            //AthleticsRemainder.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, remainder));
            //effects.Add(AthleticsRemainder);

            return effects;
        }

        private void ApplyMasteryEffects(List<Effect> effects)
        {           
            foreach (Effect effect in this.currentMasteryEffects)            
                this.effectscmp.Remove(effect);            
            foreach (Effect effect in effects)             
                this.effectscmp.Add(effect, false);                            
            this.currentMasteryEffects = effects;
        }
       
        public void Sim4000ms(float dt)
        {
            //resume.ForceAddSkillPoint();
            if (this.unusedSkillPoints == resume.AvailableSkillpoints)
                return;
            this.unusedSkillPoints = resume.AvailableSkillpoints;
            this.masteredSkillBranches = this.GetNumberOfMasteredBranches(resume);
            if (this.masteredSkillBranches == 0)
                return;
            List<Effect> effects = this.GetMasteryEffects(resume);
            this.ApplyMasteryEffects(effects);
        }

    }
}
