using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgradeable_Dupes_And_Critters
{
    internal class DBInit
    {
        public static void CreateTraits()
        {



            for (int i = 1; i <= DuplicantUpgradeTracker.MaxUpgradeLevel; i++)
            {
                string name = "DuplicantUpgrade" + i.ToString();
                Trait trait = Db.Get().CreateTrait(name, "Duplicant Upgrade " + i.ToString(), "Enhancements", null, true, null, true, false);
                //TUNING.TRAITS.TRAIT_CREATORS.Add(TraitUtil.CreateNamedTrait(name, "Duplicant Upgrade", "testing"));
                //Trait trait = Db.Get().traits.Get("DuplicantUpgrade");

                //trait.Add(new AttributeModifier(Db.Get().Attributes.AirConsumptionRate.Id, 0.1f * i, name));
                if (Config.Instance.Oxygen_Consumption_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier(Db.Get().Attributes.AirConsumptionRate.Id, (float)Config.Instance.Oxygen_Consumption_Per_Upgrade / 1000 * i, name));
                //trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (-1666.66663f * i), name));
                if (Config.Instance.Calorie_Consumption_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, Config.Instance.Calorie_Consumption_Per_Upgrade * -1.66666f * i, name));
                if (Config.Instance.Morale_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, Config.Instance.Morale_Per_Upgrade * i, name));
                if (Config.Instance.Strength_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Strength", Config.Instance.Strength_Per_Upgrade * i, name));
                if (Config.Instance.Medicine_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Caring", Config.Instance.Medicine_Per_Upgrade * i, name));
                if (Config.Instance.Construction_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Construction", Config.Instance.Construction_Per_Upgrade * i, name));
                if (Config.Instance.Excavation_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Digging", Config.Instance.Excavation_Per_Upgrade * i, name));
                if (Config.Instance.Machinery_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Machinery", Config.Instance.Machinery_Per_Upgrade * i, name));
                if (Config.Instance.Science_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Learning", Config.Instance.Science_Per_Upgrade * i, name));
                if (Config.Instance.Cuisine_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Cooking", Config.Instance.Cuisine_Per_Upgrade * i, name));
                if (Config.Instance.Agriculture_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Botanist", Config.Instance.Agriculture_Per_Upgrade * i, name));
                if (Config.Instance.Creativity_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Art", Config.Instance.Creativity_Per_Upgrade * i, name));
                if (Config.Instance.Husbandry_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Ranching", Config.Instance.Husbandry_Per_Upgrade * i, name));
                if (Config.Instance.Athletics_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("Athletics", Config.Instance.Athletics_Per_Upgrade * i, name));
                if (Config.Instance.Piloting_Per_Upgrade != 0)
                    trait.Add(new AttributeModifier("SpaceNavigation", Config.Instance.Piloting_Per_Upgrade * i, name));

                //trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 4000000f * i, name));
                if (Config.Instance.Calorie_Consumption_Per_Upgrade != 0)
                trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, Config.Instance.Calorie_Consumption_Per_Upgrade * 4000f * i, name));

                trait.Add(new AttributeModifier(Db.Get().Amounts.BionicOxygenTank.maxAttribute.Id, BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG * i, name));

            }
            for (int i = 1; i <= CritterUpgradeTracker.MaxUpgradeLevel; i++)
            {
                string name = "CritterUpgrade" + i.ToString();
                Trait trait = Db.Get().CreateTrait(name, "Critter Upgrade " + i.ToString(), "Metabolic Upgrade", null, true, null, true, false);
                trait.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, 100f * i, name));
                //trait.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 0.03f * i, "flat"));
                trait.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 1f * i, name, is_multiplier: true));

                if (!Config.Instance.Enable_Critter_Upgrade_Propagation_To_Offspring)
                {
                    trait.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 0.00334f * i, name));                                       
                }
            }
            /*for (int i = 1; i <= CritterUpgradeTracker.MaxUpgradeLevel; i++)
            {            
                string name = "ReproductiveRate" + i.ToString();
                Effect effect = new Effect(name, name, name, 0.0f, true, false, false);
                effect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 10f * i, (string)STRINGS.CREATURES.MODIFIERS.HAPPY_TAME.NAME, true));
            }*/


        }
        public static void CreateAssignableSlots()
        {
            Db.Get().AssignableSlots.Add(new OwnableSlot(nameof(InjectionChamber), nameof(InjectionChamber)));
            Db.Get().AssignableSlots.Add(new OwnableSlot(nameof(DuplicantUpgrader), nameof(DuplicantUpgrader)));
        }
    }
}