using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace Engies_Tuneup_Inactivity_Refund
{
    [ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {
        [Option("STRINGS.CONFIG.POWERTINKERDURATION.TITLE", "", "")]
        [JsonProperty]
        public int powerTinkerDuration { get; set; }
        [Option("STRINGS.CONFIG.ADDREFUNDTRACKER.TITLE", "", "")]
        [JsonProperty]
        public bool addRefundTracker { get; set; }
        public Config()
        {
            powerTinkerDuration = 1800;
            addRefundTracker = true;
        }
    }
}
