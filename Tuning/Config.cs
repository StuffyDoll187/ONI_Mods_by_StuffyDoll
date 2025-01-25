using Newtonsoft.Json;
using PeterHan.PLib.Options;
using PeterHan.PLib.Core;
using static STRINGS.UI.UISIDESCREENS;

namespace Tuning
{
    
    [ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {
        [Option("Maximum Base Attribute Level", "", "Attributes")]
        [JsonProperty]
        public int MAX_GAINED_ATTRIBUTE_LEVEL {  get; set; }
        [Option("Target Max Level Cycle", "", "Attributes")]
        [JsonProperty]
        public int TARGET_MAX_LEVEL_CYCLE { get; set; }
        [Option("Experience Level Power", "", "Attributes")]
        [JsonProperty]
        public float EXPERIENCE_LEVEL_POWER { get; set; }
        [Option("Passive Experience Portion", "", "Skill Experience Gain")]
        [JsonProperty]
        public float PASSIVE_EXPERIENCE_PORTION { get; set; }
        [Option("Quick Learner On New Duplicants", "", "Starting Traits")]
        [JsonProperty]
        public bool Quick_Learner_On_New_Duplicants { get; set; }






        public Config()
        {
            MAX_GAINED_ATTRIBUTE_LEVEL = 20;
            TARGET_MAX_LEVEL_CYCLE = 400;
            EXPERIENCE_LEVEL_POWER = 1.7f;
            PASSIVE_EXPERIENCE_PORTION = 0.5f;
            Quick_Learner_On_New_Duplicants = false;
        }
    }
}
