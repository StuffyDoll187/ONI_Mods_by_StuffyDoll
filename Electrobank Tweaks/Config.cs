using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace Electrobank_Tweaks
{
    [ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {
        [Option("Power Bank Capacity","kiloJoules\nApplies to all types", "")]
        [JsonProperty]
        public int PowerBankCapacity { get; set; }
        [Option("Dura Power Bank Recharge Rate", "Watts","")]
        [JsonProperty]
        public int DuraBankSelfChargingRate { get; set; }
        [Option("Infinite Dura Power Bank Lifetime", "", "")]
        [JsonProperty]
        public bool EternalDuraBank {  get; set; }

        public Config()
        {
            PowerBankCapacity = 120;
            DuraBankSelfChargingRate = 60;
            EternalDuraBank = false;
        }
    }
}
