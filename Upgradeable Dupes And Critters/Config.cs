using Newtonsoft.Json;
using PeterHan.PLib.Options;
//using static Upgradeable_Dupes_And_Critters.STRINGS.MODCONFIG;
using PeterHan.PLib.Core;
using static STRINGS.UI.UISIDESCREENS;

namespace Upgradeable_Dupes_And_Critters
{
    //[UseSharedConfigLocation]
    [ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {
        /*[Option("Wattage", "How many watts you can use before exploding.")]
        [Limit(1, 50000)]
        [JsonProperty]
        public float Watts { get; set; }*/

        [Option("STRINGS.MODCONFIG.MAX_ATTRIBUTE_LEVEL_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]        
        public int Max_Attribute_Level_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.MORALE_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Morale_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.PILOTING_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Piloting_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.CONSTRUCTION_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Construction_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.EXCAVATION_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Excavation_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.MACHINERY_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Machinery_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.ATHLETICS_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Athletics_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.SCIENCE_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Science_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.CUISINE_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Cuisine_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.MEDICINE_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Medicine_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.STRENGTH_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Strength_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.CREATIVITY_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Creativity_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.AGRICULTURE_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Agriculture_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.HUSBANDRY_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.TOOLTIPS.TTIP1", "STRINGS.MODCONFIG.CATEGORIES.DUPATT")]
        [JsonProperty]
        public int Husbandry_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.ENABLE_CRITTER_UPGRADE_PROPAGATION_TO_OFFSPRING.TITLE", "STRINGS.MODCONFIG.ENABLE_CRITTER_UPGRADE_PROPAGATION_TO_OFFSPRING.TOOLTIP", "STRINGS.MODCONFIG.CATEGORIES.CRITTERS")]
        [JsonProperty]
        public bool Enable_Critter_Upgrade_Propagation_To_Offspring { get; set; }

        [Option("STRINGS.MODCONFIG.OXYGEN_CONSUMPTION_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.OXYGEN_CONSUMPTION_PER_UPGRADE.TOOLTIP", "STRINGS.MODCONFIG.CATEGORIES.DUPVIT")]
        [JsonProperty]
        public int Oxygen_Consumption_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.CALORIE_CONSUMPTION_PER_UPGRADE.TITLE", "STRINGS.MODCONFIG.CALORIE_CONSUMPTION_PER_UPGRADE.TOOLTIP", "STRINGS.MODCONFIG.CATEGORIES.DUPVIT")]
        [JsonProperty]
        public int Calorie_Consumption_Per_Upgrade { get; set; }
        [Option("STRINGS.MODCONFIG.ENABLE_MASTERED_SKILLS_ATTRIBUTE_BONUS_FOR_UNUSED_SKILL_POINTS.TITLE", "STRINGS.MODCONFIG.ENABLE_MASTERED_SKILLS_ATTRIBUTE_BONUS_FOR_UNUSED_SKILL_POINTS.TOOLTIP", "STRINGS.MODCONFIG.CATEGORIES.DUPSKILL")]
        [JsonProperty]
        public bool Enable_Mastered_Skills_Attribute_Bonus_For_Unused_Skill_Points { get; set; }
        [Option("STRINGS.MODCONFIG.MAX_MASTERY_BONUS.TITLE", "STRINGS.MODCONFIG.MAX_MASTERY_BONUS.TOOLTIP", "STRINGS.MODCONFIG.CATEGORIES.DUPSKILL")]
        [JsonProperty]
        public int Max_Mastery_Bonus { get; set; }




        public Config()
        {
            //Watts = 10000f; // defaults to 10000, e.g. if the config doesn't exist
            //Max_Attribute_Level_Per_Upgrade = 10;
            Max_Attribute_Level_Per_Upgrade = 10;
            Morale_Per_Upgrade = 3;
            Piloting_Per_Upgrade = 0;
            Construction_Per_Upgrade = 0;
            Excavation_Per_Upgrade = 0;
            Machinery_Per_Upgrade = 0;
            Athletics_Per_Upgrade = 0;
            Science_Per_Upgrade = 5;
            Cuisine_Per_Upgrade = 0;
            Medicine_Per_Upgrade = 0;
            Strength_Per_Upgrade = 0;
            Creativity_Per_Upgrade = 0;
            Agriculture_Per_Upgrade = 0;
            Husbandry_Per_Upgrade = 0;
            Enable_Critter_Upgrade_Propagation_To_Offspring = true;
            Oxygen_Consumption_Per_Upgrade = 100;
            Calorie_Consumption_Per_Upgrade = 1000;
            Enable_Mastered_Skills_Attribute_Bonus_For_Unused_Skill_Points = true;
            Max_Mastery_Bonus = 10;
            
        }
    }
}
