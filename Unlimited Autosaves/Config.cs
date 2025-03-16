using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace Unlimited_Autosaves
{
    [ConfigFile(IndentOutput: true, SharedConfigLocation: false)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {

        [Option("Unlimited Autosaves", "")]
        [JsonProperty]
        public bool unlimitedAutosaves { get; set; }

        [Option("Autosaves", "Number of autosaves to keep if unlimited is unchecked")]
        [Limit(1, 100)]
        [JsonProperty]
        public int numAutosaves { get; set; }

        
        public Config()
        {
            unlimitedAutosaves = true;
            numAutosaves = 10;
            
        }
    }
}
