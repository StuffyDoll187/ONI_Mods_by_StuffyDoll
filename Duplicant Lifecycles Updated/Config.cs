using System;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace DuplicantLifecycles
{
    [ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
    [RestartRequired]
    public class Config : SingletonOptions<Config>
    {
        
        [Option("MaxDuplicantAge", "Must be greater than Elderly\nInvalid Relative Values will result in defaults being used", "Age Eras")]
        [JsonProperty]
        public int MaxDuplicantAge { get; set; }// = 200f;
        [Option("YouthfulAgeCutoff", "Must be non-negative\nInvalid Relative Values will result in defaults being used", "Age Eras")]
        [JsonProperty]
        public int YouthfulAgeCutoff { get; set; }// = 30f;
        [Option("MiddleAgedAgeCutoff", "Must be greater than Youthful\nInvalid Relative Values will result in defaults being used", "Age Eras")]
        [JsonProperty]
        public int MiddleAgedAgeCutoff { get; set; }// = 140f;
        [Option("ElderlyAgeCutoff", "Must be greater than Middle Aged\nInvalid Relative Values will result in defaults being used", "Age Eras")]
        [JsonProperty]
        public int ElderlyAgeCutoff { get; set; }// = 180f;        
        [Option("UsePercentageOfTotalStats", "Enable: Multipliers Use Duplicant's Current Stat Values\nDisable: Multipliers Use CustomStatBaseValue for all stats", "Logic Customization")]
        [JsonProperty]
        public bool UsePercentageOfTotalStats { get; set; }// = true;
        [Option("CustomStatBaseValue", "If UsePercentageOfTotalStats is disabled, then multipliers will use this as the base\ne.g. If this is set to 10 and MiddleAgedMultiplier is set to 0.5 then bonus is +15 to all stats", "Logic Customization")]
        [JsonProperty]
        public int CustomStatBaseValue { get; set; }// = 4f;               
        [Option("TimeToDieProbabilityDecrease", "Once Duplicants pass Elderly Cutoff Age, there is a chance to die every cycle start.\nThis reduces the chance.\nHigher Values = Lower Chance", "Logic Customization")]
        [Limit(0,1)]
        [JsonProperty]
        public float TimeToDieProbabilityDecrease { get; set; }// = 0;
        [Option("EnabledDeathFromAge", "Allow Duplicants to die due to age", "Logic Customization")]
        [JsonProperty]
        public bool EnabledDeathFromAge { get; set; }// = true;
        [Option("EnableImmortalTrait", "Immortal may be gained from the Neural Vacillator", "Logic Customization")]
        [JsonProperty]
        public bool EnableImmortalTrait { get; set; }// = true;
        [Option("ApplyAgingToBionics", "Include Bionic Duplicants for aging effects, including death", "Logic Customization")]
        [JsonProperty]
        public bool ApplyAgingToBionics { get; set; }// = false;
        [Option("YouthAttributeMultiplier", "e.g.  1.25 = +125%", "Multipliers")]
        [Limit(-10, 10)]
        [JsonProperty]
        public float YouthAttributeMultiplier { get; set; }// = 1f;        
        [Option("MiddleAgedAttributeMultiplier", "e.g.  0.75 = +75%", "Multipliers")]
        [Limit(-10, 10)]
        [JsonProperty]
        public float MiddleAgedAttributeMultiplier { get; set; }// = 0.35f;        
        [Option("ElderlyAttributeMultiplier", "e.g.  -0.5 = -50%", "Multipliers")]
        [Limit(-10, 10)]
        [JsonProperty]
        public float ElderlyAttributeMultiplier { get; set; }// = -0.35f;
       
        [Option("DyingAttributeMultiplier", "e.g.  -1.5 = -150%", "Multipliers")]
        [Limit(-10, 10)]
        [JsonProperty]
        public float DyingAttributeMultiplier { get; set; }// = -1f;
        
        [Option("ImmortalAttributeMultiplier", "e.g.  0.25 = +25%", "Multipliers")]
        [Limit(-10, 10)]
        [JsonProperty]
        public float ImmortalAttributeMultiplier { get; set; }// = 0.1f;
        

        public Config()
        {
            MaxDuplicantAge = 200;
            YouthfulAgeCutoff = 30;
            MiddleAgedAgeCutoff = 140;
            ElderlyAgeCutoff = 180;
            UsePercentageOfTotalStats = true;
            CustomStatBaseValue = 4;                                              
            TimeToDieProbabilityDecrease = 0;
            EnabledDeathFromAge = true;
            EnableImmortalTrait = true;
            ApplyAgingToBionics = false;            
            YouthAttributeMultiplier = 1f;            
            MiddleAgedAttributeMultiplier = 0.35f;            
            ElderlyAttributeMultiplier = -0.35f;            
            DyingAttributeMultiplier = -1f;            
            ImmortalAttributeMultiplier = 0.1f;
        }
    }
}