using Newtonsoft.Json;
using PeterHan.PLib.Options;


namespace Egg_Incubator_Power_Config
{
    [ConfigFile(IndentOutput: true, SharedConfigLocation: false)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {

        [Option("Requires Power", "", "")]
        [JsonProperty]
        public bool Requires_Power { get; set; }
        [Option("Watts", "", "")]
        [JsonProperty]
        public int Watts { get; set; }





        public Config()
        {
            Requires_Power = true;
            Watts = 240;
        }
    }


   
}
