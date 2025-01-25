using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace Geyser_Control
{

    [ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {
        [Option("STRINGS.CONFIG.BREAKVANILLALIMITS.TITLE", "STRINGS.CONFIG.BREAKVANILLALIMITS.TOOLTIP", "")]        
        [JsonProperty]
        public bool breakVanilla { get; set; }
        [Option("STRINGS.CONFIG.BREAKFACTOR.TITLE", "STRINGS.CONFIG.BREAKFACTOR.TOOLTIP", "")]
        [JsonProperty]
        public int breakFactor { get; set; }
        [Option("STRINGS.CONFIG.RESETBUTTON.TITLE", "STRINGS.CONFIG.RESETBUTTON.TOOLTIP", "")]
        [JsonProperty]
        public bool resetButton { get; set; }
        [Option("STRINGS.CONFIG.RANDOMIZESLIDERSBUTTON.TITLE", "STRINGS.CONFIG.RANDOMIZESLIDERSBUTTON.TOOLTIP", "")]
        [JsonProperty]
        public bool randomizeSlidersButton { get; set; }

        [Option("STRINGS.CONFIG.DORMANCYBUTTON.TITLE", "STRINGS.CONFIG.DORMANCYBUTTON.TOOLTIP", "")]
        [JsonProperty]
        public bool dormancyButton { get; set; }
        [Option("STRINGS.CONFIG.ERUPTIONBUTTON.TITLE", "STRINGS.CONFIG.ERUPTIONBUTTON.TOOLTIP", "")]
        [JsonProperty]
        public bool eruptionButton { get; set; }

        /*[Option("STRINGS.CONFIG.MAXPRESSURECONTROLS.TITLE", "STRINGS.CONFIG.MAXPRESSURECONTROLS.TOOLTIP", "")]
        [JsonProperty]
        public bool maxPressureControls { get; set; }*/
        [Option("STRINGS.CONFIG.UNCAPPRESSURECHECKBOX.TITLE", "STRINGS.CONFIG.UNCAPPRESSURECHECKBOX.TOOLTIP", "")]
        [JsonProperty]
        public bool uncapPressureCheckbox { get; set; }
        [Option("STRINGS.CONFIG.ALLOWINSTANTANALYSIS.TITLE", "STRINGS.CONFIG.ALLOWINSTANTANALYSIS.TOOLTIP", "")]
        [JsonProperty]
        public bool allowInstantAnalysis { get; set; }
        [Option("STRINGS.CONFIG.DISABLECOOLDOWNS.TITLE", "STRINGS.CONFIG.DISABLECOOLDOWNS.TOOLTIP", "")]
        [JsonProperty]
        public bool disableCooldowns { get; set; }

        public Config()
        {
            breakVanilla = false;
            breakFactor = 3;
            resetButton = true;
            randomizeSlidersButton = true;
            dormancyButton = false;
            eruptionButton = false;
            //maxPressureControls = false;
            uncapPressureCheckbox = false;
            allowInstantAnalysis = false;
            disableCooldowns = false;
            
        }
    }
}
