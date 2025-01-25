using Newtonsoft.Json;
using PeterHan.PLib.Options;
using TUNING;

namespace Atmo_Suit_Capacity_Config
{
    [ConfigFile(IndentOutput: true, SharedConfigLocation: false)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {
        [Option("Atmo Suit Capacity in kg", "", "")]
        [JsonProperty]
        public float capacityInKg { get; set; }

        public Config()
        {
            capacityInKg = (float) (DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * 600.0 * 1.25);

        }
    }
}
