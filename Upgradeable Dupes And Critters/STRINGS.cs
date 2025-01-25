using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STRINGS.NAMEGEN;

namespace Upgradeable_Dupes_And_Critters
{
    internal class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class DUPLICANTUPGRADER
                {
                    public static LocString NAME = UI.FormatAsLink("Duplicant Upgrader", "");
                    public static LocString EFFECT = "Massively enhances Duplicants at the cost of increased Oxygen and Calorie consumption.\n\nRequires the GlowStick trait.";
                    public static LocString DESC = "Usable up to " + DuplicantUpgradeTracker.MaxUpgradeLevel.ToString() + " times per Duplicant.\n\nGlowStick is consumed on use.";
                }
                public class CRITTERUPGRADER
                {
                    public static LocString NAME = UI.FormatAsLink("Critter Upgrader", "");
                    public static LocString EFFECT = "Use Radioactive Serum to supercharge a Critter's Metabolism and Shearable Resource Growth Rate.\n\nDNA Modification propagates to offspring.";
                    public static LocString DESC = "Modified Critters also drop increased resources on death\n\nRequires Tamed Critter";
                }
                public class INJECTIONCHAMBER
                {                    
                    public static LocString NAME = UI.FormatAsLink("Injection Chamber", "");
                    public static LocString EFFECT = "A place to inject Duplicants with Radioactive Serum\n\nGives the GlowStick trait.";
                    public static LocString DESC = "Requires a Doctor with Advanced Medical Care.";
                }
                public class NUCLEARAPOTHECARY
                {
                    public static LocString NAME = UI.FormatAsLink("Nuclear Apothecary", "");
                    public static LocString EFFECT = "Compound Radioactive Serum for use in modifying Duplicant and Critter DNA.\n\nRequires: Glass, Steel, and Salt Water";
                    public static LocString DESC = "Consumes Radbolts.";
                }
                public class UPGRADEDEGGCRACKER
                {
                    public static LocString NAME = UI.FormatAsLink("Upgraded Egg Cracker", "");
                    public static LocString EFFECT = "Able to crack upgraded eggs for extra Raw Egg and Egg Shells.\n\n" + Strings.Get("STRINGS.BUILDINGS.PREFABS.EGGCRACKER.EFFECT").String;
                    //public static LocString EFFECT = "Able to crack upgraded eggs for extra Raw Egg and Egg Shells.\n\n" + STRINGS.BUILDINGS.PREFABS.EGGCRACKER.EFFECT);
                    public static LocString DESC = Strings.Get("STRINGS.BUILDINGS.PREFABS.EGGCRACKER.DESC").String;
                    //public static LocString DESC = STRINGS.BUILDINGS.PREFABS.EGGCRACKER.DESC);
                }                                
            }
        }
        public class MODCONFIG
        {
            public class CATEGORIES
            {
                public static LocString DUPATT = "Duplicant Attributes";
                public static LocString DUPVIT = "Duplicant Vitals";                
                public static LocString DUPSKILL = "Duplicant Unused Skill Points";
                public static LocString CRITTERS = "Critters";
            }
            public class TOOLTIPS
            {
                public static LocString TTIP1 = "Increase Per Upgrade";               
            }
            public class MAX_ATTRIBUTE_LEVEL_PER_UPGRADE
            {
                public static LocString TITLE = "Maximum Base Attribute Level";
                //public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class MORALE_PER_UPGRADE
            {
                public static LocString TITLE = "Morale";
                //public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class PILOTING_PER_UPGRADE
            {
                public static LocString TITLE = "Piloting";
               // public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class CONSTRUCTION_PER_UPGRADE
            {
                public static LocString TITLE = "Construction";
                //public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class EXCAVATION_PER_UPGRADE
            {
                public static LocString TITLE = "Excavation";
                //public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class MACHINERY_PER_UPGRADE
            {
                public static LocString TITLE = "Machinery";
                //public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class ATHLETICS_PER_UPGRADE
            {
                public static LocString TITLE = "Athletics";
               // public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class SCIENCE_PER_UPGRADE
            {
                public static LocString TITLE = "Science";
               // public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class CUISINE_PER_UPGRADE
            {
                public static LocString TITLE = "Cuisine";
              //  public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class MEDICINE_PER_UPGRADE
            {
                public static LocString TITLE = "Medicine";
              //  public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class STRENGTH_PER_UPGRADE
            {
                public static LocString TITLE = "Strength";
              //  public static LocString TOOLTIP = TOOLTIPS.TTIP1;
               
            }
            public class CREATIVITY_PER_UPGRADE
            {
                public static LocString TITLE = "Creativity";
              //  public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class AGRICULTURE_PER_UPGRADE
            {
                public static LocString TITLE = "Agriculture";
               // public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class HUSBANDRY_PER_UPGRADE
            {
                public static LocString TITLE = "Husbandry";
              //  public static LocString TOOLTIP = TOOLTIPS.TTIP1;
                
            }
            public class ENABLE_CRITTER_UPGRADE_PROPAGATION_TO_OFFSPRING
            {
                public static LocString TITLE = "Upgrades Propagate To Offspring";
                public static LocString TOOLTIP = "Upgraded Critters Lay Upgraded Eggs which hatch into Upgraded Critters\nDisabling will increase Reproductive Rate of Upgraded Critters";
                
            }
            public class OXYGEN_CONSUMPTION_PER_UPGRADE
            {
                public static LocString TITLE = "Oxygen Consumption";
                public static LocString TOOLTIP = "Grams per Second per Upgrade";
               
            }
            public class CALORIE_CONSUMPTION_PER_UPGRADE
            {
                public static LocString TITLE = "Calorie Consumption";
                public static LocString TOOLTIP = "kCal per Cycle per Upgrade";
               
            }
            public class ENABLE_MASTERED_SKILLS_ATTRIBUTE_BONUS_FOR_UNUSED_SKILL_POINTS
            {
                public static LocString TITLE = "Enable Unused Skill Point Bonus";
                public static LocString TOOLTIP = "Bonus To Attributes Corresponding To Completed Skill Branches\nScales with unused skill points\nExcludes Suit Wearing and Carrying";
                
            }
            public class MAX_MASTERY_BONUS
            {
                public static LocString TITLE = "Maximum Bonus";
                public static LocString TOOLTIP = "";
               
            }
        }
        public class MASTERYEFFECTS
        {
            public static LocString DESC = "Bonus for Unused Skill Points";

            public class RESEARCHER
            {
                public static LocString NAME = "Master Researcher";
                public static LocString DESC = "Researching Mastery";
            }
            public class BUILDER
            {
                public static LocString NAME = "Master Builder";
                public static LocString DESC = "Building Mastery";
            }
            public class OPERATOR
            {
                public static LocString NAME = "Master Operator";
                public static LocString DESC = "Operating Mastery";
            }
            public class DIGGER
            {
                public static LocString NAME = "Master Digger"; 
                public static LocString DESC = "Digging Mastery";
            }
            public class FARMER
            {
                public static LocString NAME = "Master Farmer"; 
                public static LocString DESC = "Farming Mastery";
            }
            public class RANCHER
            {
                public static LocString NAME = "Master Rancher"; 
                public static LocString DESC = "Ranching Mastery";
            }
            public class COOK
            {
                public static LocString NAME = "Master Cook"; 
                public static LocString DESC = "Cooking Mastery";
            }
            public class PILOT
            {
                public static LocString NAME = "Master Pilot"; 
                public static LocString DESC = "Piloting Mastery";
            }
            public class TIDIER
            {
                public static LocString NAME = "Master Tidier"; 
                public static LocString DESC = "Tidying Mastery";
            }
            public class DOCTOR
            {
                public static LocString NAME = "Master Doctor"; 
                public static LocString DESC = "Medicine Mastery";
            }
            public class DECORATOR
            {
                public static LocString NAME = "Master Decorator"; 
                public static LocString DESC = "Decorating Mastery";   
            }






        }
        
    }
}
