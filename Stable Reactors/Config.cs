using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace Stable_Reactors
{
    [ConfigFile(IndentOutput: true, SharedConfigLocation: false)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {
        [Option("Meltdown Temperature Threshold", "Kelvin", "")]
        [JsonProperty]
        public float MeltdownTempThresh { get; set; }
        [Option("Temperature History Entries To Keep", "5 Entries per Second", "")]
        [JsonProperty]
        public int TempHistoryEntriesToKeep { get; set; }

        public Config()
        {
            MeltdownTempThresh = 3000;
            TempHistoryEntriesToKeep = 10;
        }
    }
}
