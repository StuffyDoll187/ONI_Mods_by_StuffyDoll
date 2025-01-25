using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace Chaos_Reigns
{
    [ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {
        
        [Option("Use YAML or CGM", "Check this if you wish to add these custom showers manually via YAML editing or Cluster Generation Manager by SGT_Imalas\n\nIDs for YAML editing:\nSpongeSlugMeteorShowers\nZoologicalMeteorShowers\nWaterBalloonMeteorShowers\n\nUnchecked: These meteor showers will be added to Niobium Moonlet, Marshy Moonlet, and Water Moonlet respectively on New Game Start", "")]
        [JsonProperty]
        public bool UseYAMLorCGM { get; set; }
        [Option("Add these showers to an existing save on load", "Only required if the save was not created with the showers\n\nIgnores games created with 'Use YAML or CGM' checked", "")]
        [JsonProperty]
        public bool AddToExistingSaveOnLoad { get; set; }

        [Option("Cluster Travel Duration", "Seconds\n\nTravel Time to target asteroid from spawn on Cluster Map Edge", "")]
        [JsonProperty]
        public int TravelDuration { get; set; }

        
        [Option("Enable Molten Slug Meteors", "", "Meteors Slugs")]
        [JsonProperty]
        public bool EnableMoltenSlugs { get; set; }
        
        [Option("Slug Time To Live", "Seconds", "Meteors Slugs")]
        [JsonProperty]
        public int SlugTTL { get; set; }               
        [Option("Slug Shower Period", "Average Cycles between showers", "Meteors Slugs")]
        [JsonProperty]
        public float SlugPeriod { get; set; }
        [Option("Slug Shower Duration", "Seconds", "Meteors Slugs")]
        [JsonProperty]
        public int SlugShowerDuration { get; set; }
        [Option("Slug Average Meteors per shower", "", "Meteors Slugs")]
        [JsonProperty]
        public int SlugAvgMeteors { get; set; }
        [Option("Slug Content Element1", "", "Meteors Slugs")]
        [JsonProperty]
        public LiquidOptions SlugContentElementChoice1 { get; set; }
        [Option("Slug Content Mass1", "Kilograms", "Meteors Slugs")]
        [JsonProperty]
        public int SlugContentMass1 { get; set; }
        [Option("Slug Content Temperature1", "Celsius", "Meteors Slugs")]
        [JsonProperty]
        public int SlugContentTemp1 { get; set; }
        [Option("Slug Content Element2", "", "Meteors Slugs")]
        [JsonProperty]
        public LiquidOptions SlugContentElementChoice2 { get; set; }
        [Option("Slug Content Mass2", "Kilograms", "Meteors Slugs")]
        [JsonProperty]
        public int SlugContentMass2 { get; set; }
        [Option("Slug Content Temperature2", "Celsius", "Meteors Slugs")]
        [JsonProperty]
        public int SlugContentTemp2 { get; set; }
        [Option("Slug Content Element3", "", "Meteors Slugs")]
        [JsonProperty]
        public LiquidOptions SlugContentElementChoice3 { get; set; }
        [Option("Slug Content Mass3", "Kilograms", "Meteors Slugs")]
        [JsonProperty]
        public int SlugContentMass3 { get; set; }
        [Option("Slug Content Temperature3", "Celsius", "Meteors Slugs")]
        [JsonProperty]
        public int SlugContentTemp3 { get; set; }
        
        [Option("Enable Zoological Meteors", "", "Meteors Zoological")]
        [JsonProperty]
        public bool EnableZoological { get; set; }
        
        [Option("Zoological Shower Period", "Average Cycles between showers", "Meteors Zoological")]
        [JsonProperty]
        public float ZoologicalPeriod { get; set; }
        [Option("Zoological Shower Duration", "Seconds", "Meteors Zoological")]
        [JsonProperty]
        public int ZoologicalDuration { get; set; }
        [Option("Zoological Average Meteors per Shower", "", "Meteors Zoological")]
        [JsonProperty]
        public int ZoologicalAvgMeteors { get; set; }
        [Option("Zoological Meteors Include Morphs", "", "Meteors Zoological")]
        [JsonProperty]
        public bool ZoologicalIncludeMorphs { get; set; }

        
        [Option("Enable Water Balloon Meteors", "", "Meteors Water Balloons")]
        [JsonProperty]
        public bool EnableWaterBalloons { get; set; }
        
        [Option("Water Balloon Shower Period", "Average Cycles between showers", "Meteors Water Balloons")]
        [JsonProperty]
        public float WaterBalloonPeriod { get; set; }
        [Option("Water Balloon Shower Duration", "Seconds", "Meteors Water Balloons")]
        [JsonProperty]
        public int WaterBalloonDuration { get; set; }
        [Option("Water Balloon Shower Average Balloons per Shower", "", "Meteors Water Balloons")]
        [JsonProperty]
        public int WaterBalloonAvgMeteors { get; set; }
        [Option("Water Balloon Content Mass", "Kilograms", "Meteors Water Balloons")]
        [JsonProperty]
        public int WaterBalloonContentMass { get; set; }


        [Option("Chance for Fish", "What is a Water Balloon without a chance for a Tropical Fish?!", "Meteors Water Balloons")]
        [JsonProperty]
        [Limit(0,100)]
        public int ChanceForFish {  get; set; }
        
        [Option("Enable Rain", "StartWorld: Dealer's Choice\nSuperConductive: Magma\nMarshy: Polluted Water\nWater: Water\nMoo: Chlorine", "Rain")]
        [JsonProperty]
        public bool EnableRain { get; set; }
        [Option("StartWorld Rain Element", "", "Rain")]
        [JsonProperty]
        public RainElementOptions RainElementChoiceStartWorld { get; set; }
        [Option("StartWorld Rain Drop Mass", "Grams\n\n0 to disable","Rain")]
        [JsonProperty]
        public int RainMassStartWorld { get; set; }
        [Option("StartWorld Rain Drop Temperature", "Celsius", "Rain")]
        [JsonProperty]
        public int RainTempStartWorld { get; set; }


        [Option("Niobium Moonlet Magma Rain Drop Mass", "Grams\n\n0 to disable", "Rain")]
        [JsonProperty]
        public int MagmaRainMass { get; set; }
        [Option("Water Moonlet Water Rain Drop Mass", "Grams\n\n0 to disable", "Rain")]
        [JsonProperty]
        public int WaterRainMass { get; set; }
        [Option("Marshy Moonlet Polluted Rain Drop Mass", "Grams\n\n0 to disable", "Rain")]
        [JsonProperty]
        public int MarshyRainMass { get; set; }
        [Option("Moo Moonlet Chlorine Rain Drop Mass", "Grams\n\n0 to disable", "Rain")]
        [JsonProperty]
        public int MooRainMass { get; set; }






        [Option("Enable Twitch Event: Magma Rain", "", "Twitch Integration Events")]
        [JsonProperty]
        public bool EnableTwitchMagmaRain { get; set; }
        [Option("Twitch Magma Rain Duration", "Seconds", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchMagmaRainDuration { get; set; }
        [Option("Twitch Magma Rain Drop Mass", "Kilograms\n\n5 drops per second", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchMagmaRainMass { get; set; }
        [Option("Enable Twitch Event: Nuclear Waste Rain", "", "Twitch Integration Events")]
        [JsonProperty]
        public bool EnableTwitchNuclearWasteRain { get; set; }
        [Option("Twitch Nuclear Waste Rain Duration", "Seconds", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchNuclearWasteRainDuration { get; set; }
        [Option("Twitch Nuclear Waste Rain Drop Mass", "Kilograms\n\n5 drops per second", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchNuclearWasteRainMass { get; set; }




        [Option("Enable Twitch Event: Water Balloons", "", "Twitch Integration Events")]
        [JsonProperty]
        public bool EnableTwitchWaterBalloons { get; set; }
        [Option("Twitch Water Balloon Duration", "Seconds", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchWaterBalloonDuration { get; set; }
        
        [Option("Twitch Water Balloon Content Mass", "Kilograms", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchWaterBalloonContentMass { get; set; }
        [Option("Twitch Chance for Fish", "What is a Water Balloon without a chance for a Tropical Fish?!", "Twitch Integration Events")]
        [JsonProperty]
        [Limit(0, 100)]
        public int TwitchChanceForFish { get; set; }








        [Option("Enable Twitch Event: Zoological Meteors", "", "Twitch Integration Events")]
        [JsonProperty]
        public bool EnableTwitchZoological { get; set; }
        [Option("Twitch Zoological Shower Duration", "Seconds", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchZoologicalDuration { get; set; }
        
        [Option("Twitch Zoological Meteors Include Morphs", "", "Twitch Integration Events")]
        [JsonProperty]
        public bool TwitchZoologicalIncludeMorphs { get; set; }





        [Option("Enable Twitch Event: Molten Slug Meteors", "", "Twitch Integration Events")]
        [JsonProperty]
        public bool EnableTwitchMoltenSlugs { get; set; }


        [Option("Twitch Slug Time To Live", "Seconds", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchSlugTTL { get; set; }
        
        [Option("Twitch Slug Shower Duration", "Seconds", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchSlugShowerDuration { get; set; }
        
        [Option("Twitch Slug Content Element1", "", "Twitch Integration Events")]
        [JsonProperty]
        public LiquidOptions TwitchSlugContentElementChoice1 { get; set; }
        [Option("Twitch Slug Content Mass1", "Kilograms", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchSlugContentMass1 { get; set; }
        [Option("Twitch Slug Content Temperature1", "Celsius", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchSlugContentTemp1 { get; set; }
        [Option("Twitch Slug Content Element2", "", "Twitch Integration Events")]
        [JsonProperty]
        public LiquidOptions TwitchSlugContentElementChoice2 { get; set; }
        [Option("Twitch Slug Content Mass2", "Kilograms", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchSlugContentMass2 { get; set; }
        [Option("Twitch Slug Content Temperature2", "Celsius", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchSlugContentTemp2 { get; set; }
        [Option("Twitch Slug Content Element3", "", "Twitch Integration Events")]
        [JsonProperty]
        public LiquidOptions TwitchSlugContentElementChoice3 { get; set; }
        [Option("Twitch Slug Content Mass3", "Kilograms", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchSlugContentMass3 { get; set; }
        [Option("Twitch Slug Content Temperature3", "Celsius", "Twitch Integration Events")]
        [JsonProperty]
        public int TwitchSlugContentTemp3 { get; set; }



        
        public Config() 
        {
            
            UseYAMLorCGM = false;
            AddToExistingSaveOnLoad = true;
            TravelDuration = 6000;
            
            EnableMoltenSlugs = true;
            
            SlugTTL = 30;
            SlugPeriod = 20;
            SlugShowerDuration = 120;
            SlugAvgMeteors = 10;
            SlugContentElementChoice1 = LiquidOptions.Magma;
            SlugContentMass1 = 1840 * 5;
            SlugContentTemp1 = 2000;
            SlugContentElementChoice2 = LiquidOptions.Cobalt;
            SlugContentMass2 = 1000 * 2;
            SlugContentTemp2 = 2000;
            SlugContentElementChoice3 = LiquidOptions.Aluminum;
            SlugContentMass3 = 1000 * 2;
            SlugContentTemp3 = 2000;

            EnableZoological = true;
            
            ZoologicalPeriod = 20;
            ZoologicalDuration = 300;
            ZoologicalAvgMeteors = 20;
            ZoologicalIncludeMorphs = false;

            EnableWaterBalloons = true;
            
            WaterBalloonPeriod = 20;
            WaterBalloonDuration = 300;
            WaterBalloonAvgMeteors = 100;
            WaterBalloonContentMass = 1000;
            ChanceForFish = 10;

            EnableRain = true;
            
            RainElementChoiceStartWorld = RainElementOptions.Water;
            RainMassStartWorld = 250;
            RainTempStartWorld = 22;
            

            MagmaRainMass = 600;
            WaterRainMass = 2000;
            MarshyRainMass = 250;
            MooRainMass = 50;


            EnableTwitchMoltenSlugs = true;
            TwitchSlugTTL = 600;
            TwitchSlugShowerDuration = 10;
            TwitchSlugContentElementChoice1 = LiquidOptions.Magma;
            TwitchSlugContentMass1 = 1840 * 5;
            TwitchSlugContentTemp1 = 2000;
            TwitchSlugContentElementChoice2 = LiquidOptions.Cobalt;
            TwitchSlugContentMass2 = 1000 * 2;
            TwitchSlugContentTemp2 = 2000;
            TwitchSlugContentElementChoice3 = LiquidOptions.Aluminum;
            TwitchSlugContentMass3 = 1000 * 2;
            TwitchSlugContentTemp3 = 2000;

            EnableTwitchZoological = true;
            TwitchZoologicalDuration = 30;
            TwitchZoologicalIncludeMorphs = true;


            EnableTwitchWaterBalloons = true;
            TwitchWaterBalloonDuration = 120;
            TwitchWaterBalloonContentMass = 1000;
            TwitchChanceForFish = 30;


            EnableTwitchMagmaRain = true;
            EnableTwitchNuclearWasteRain = true;
            TwitchMagmaRainMass = 60;
            TwitchMagmaRainDuration = 120;
            TwitchNuclearWasteRainMass = 60;
            TwitchNuclearWasteRainDuration = 120;


            

            
        }
        public SimHashes SlugContentElement1 { get; set; }
        public SimHashes SlugContentElement2 { get; set; }
        public SimHashes SlugContentElement3 { get; set; }
        public SimHashes RainElementStartWorld { get; set; }
        public SimHashes RainElementNiobiumMoonlet { get; set; }
        public SimHashes RainElementWaterMoonlet { get; set; }
        public SimHashes RainElementMarshyMoonlet { get; set; }
        public SimHashes TwitchRainElement { get; set; }
        public SimHashes TwitchSlugContentElement1 { get; set; }
        public SimHashes TwitchSlugContentElement2 { get; set; }
        public SimHashes TwitchSlugContentElement3 { get; set; }

        public void InitSlugElement()
        {
            switch (SlugContentElementChoice1)
            {
                //case LiquidOptions.Random: SlugContentElement = MoltenSimHashes[UnityEngine.Random.Range(0, MoltenSimHashes.Length)]; break;
                case LiquidOptions.Magma: SlugContentElement1 = SimHashes.Magma; break;
                case LiquidOptions.Aluminum: SlugContentElement1=SimHashes.MoltenAluminum; break;
                case LiquidOptions.Cobalt: SlugContentElement1 = SimHashes.MoltenCobalt; break;
                case LiquidOptions.Copper: SlugContentElement1 = SimHashes.MoltenCopper; break;
                case LiquidOptions.Gold: SlugContentElement1 = SimHashes.MoltenGold; break;
                case LiquidOptions.Iron: SlugContentElement1 = SimHashes.MoltenIron; break;
                case LiquidOptions.Lead: SlugContentElement1 = SimHashes.MoltenLead; break;
                case LiquidOptions.Niobium: SlugContentElement1 = SimHashes.MoltenNiobium; break;
                case LiquidOptions.Steel: SlugContentElement1 = SimHashes.MoltenSteel; break;
                case LiquidOptions.Tungsten: SlugContentElement1 = SimHashes.MoltenTungsten; break;
                case LiquidOptions.Uranium: SlugContentElement1=SimHashes.MoltenUranium; break;
                case LiquidOptions.NuclearWaste: SlugContentElement1 = SimHashes.NuclearWaste; break;

                case LiquidOptions.Brackene: SlugContentElement1 = SimHashes.Milk; break;
                case LiquidOptions.Brine: SlugContentElement1 = SimHashes.Brine; break;
                case LiquidOptions.CrudeOil: SlugContentElement1 = SimHashes.CrudeOil; break;
                case LiquidOptions.Ethanol: SlugContentElement1 = SimHashes.Ethanol; break;
                case LiquidOptions.LiquidCarbon: SlugContentElement1 = SimHashes.MoltenCarbon; break;
                case LiquidOptions.LiquidCO2: SlugContentElement1 = SimHashes.LiquidCarbonDioxide; break;
                case LiquidOptions.Chlorine: SlugContentElement1 = SimHashes.Chlorine; break;
                case LiquidOptions.Hydrogen: SlugContentElement1 = SimHashes.LiquidHydrogen; break;
                case LiquidOptions.Methane: SlugContentElement1 = SimHashes.LiquidMethane; break;
                case LiquidOptions.Naptha: SlugContentElement1 = SimHashes.Naphtha; break;
                case LiquidOptions.LiquidO2: SlugContentElement1 = SimHashes.LiquidOxygen; break;
                case LiquidOptions.LiquidPhosphorous: SlugContentElement1 = SimHashes.LiquidPhosphorus; break;
                case LiquidOptions.LiquidResin: SlugContentElement1 = SimHashes.Resin; break;
                case LiquidOptions.LiquidSucrose: SlugContentElement1 = SimHashes.MoltenSucrose; break;
                case LiquidOptions.LiquidSulfur: SlugContentElement1 = SimHashes.LiquidSulfur; break;
                case LiquidOptions.MoltenGlass: SlugContentElement1 = SimHashes.MoltenGlass; break;
                case LiquidOptions.MoltenSalt: SlugContentElement1 = SimHashes.MoltenSalt; break;
                case LiquidOptions.Petroleum: SlugContentElement1 = SimHashes.Petroleum; break;
                case LiquidOptions.DirtyWater: SlugContentElement1 = SimHashes.DirtyWater; break;
                case LiquidOptions.SaltWater: SlugContentElement1 = SimHashes.SaltWater; break;
                case LiquidOptions.SuperCoolant: SlugContentElement1 = SimHashes.SuperCoolant; break;
                case LiquidOptions.ViscoGel: SlugContentElement1 = SimHashes.ViscoGel; break;
                case LiquidOptions.Water: SlugContentElement1 = SimHashes.Water; break;

            }
            switch (SlugContentElementChoice2)
            {
                //case LiquidOptions.Random: SlugContentElement = MoltenSimHashes[UnityEngine.Random.Range(0, MoltenSimHashes.Length)]; break;
                case LiquidOptions.Magma: SlugContentElement2 = SimHashes.Magma; break;
                case LiquidOptions.Aluminum: SlugContentElement2 = SimHashes.MoltenAluminum; break;
                case LiquidOptions.Cobalt: SlugContentElement2 = SimHashes.MoltenCobalt; break;
                case LiquidOptions.Copper: SlugContentElement2 = SimHashes.MoltenCopper; break;
                case LiquidOptions.Gold: SlugContentElement2 = SimHashes.MoltenGold; break;
                case LiquidOptions.Iron: SlugContentElement2 = SimHashes.MoltenIron; break;
                case LiquidOptions.Lead: SlugContentElement2 = SimHashes.MoltenLead; break;
                case LiquidOptions.Niobium: SlugContentElement2 = SimHashes.MoltenNiobium; break;
                case LiquidOptions.Steel: SlugContentElement2 = SimHashes.MoltenSteel; break;
                case LiquidOptions.Tungsten: SlugContentElement2 = SimHashes.MoltenTungsten; break;
                case LiquidOptions.Uranium: SlugContentElement2 = SimHashes.MoltenUranium; break;
                case LiquidOptions.NuclearWaste: SlugContentElement2 = SimHashes.NuclearWaste; break;

                case LiquidOptions.Brackene: SlugContentElement2 = SimHashes.Milk; break;
                case LiquidOptions.Brine: SlugContentElement2 = SimHashes.Brine; break;
                case LiquidOptions.CrudeOil: SlugContentElement2 = SimHashes.CrudeOil; break;
                case LiquidOptions.Ethanol: SlugContentElement2 = SimHashes.Ethanol; break;
                case LiquidOptions.LiquidCarbon: SlugContentElement2 = SimHashes.MoltenCarbon; break;
                case LiquidOptions.LiquidCO2: SlugContentElement2 = SimHashes.LiquidCarbonDioxide; break;
                case LiquidOptions.Chlorine: SlugContentElement2 = SimHashes.Chlorine; break;
                case LiquidOptions.Hydrogen: SlugContentElement2 = SimHashes.LiquidHydrogen; break;
                case LiquidOptions.Methane: SlugContentElement2 = SimHashes.LiquidMethane; break;
                case LiquidOptions.Naptha: SlugContentElement2 = SimHashes.Naphtha; break;
                case LiquidOptions.LiquidO2: SlugContentElement2 = SimHashes.LiquidOxygen; break;
                case LiquidOptions.LiquidPhosphorous: SlugContentElement2 = SimHashes.LiquidPhosphorus; break;
                case LiquidOptions.LiquidResin: SlugContentElement2 = SimHashes.Resin; break;
                case LiquidOptions.LiquidSucrose: SlugContentElement2 = SimHashes.MoltenSucrose; break;
                case LiquidOptions.LiquidSulfur: SlugContentElement2 = SimHashes.LiquidSulfur; break;
                case LiquidOptions.MoltenGlass: SlugContentElement2 = SimHashes.MoltenGlass; break;
                case LiquidOptions.MoltenSalt: SlugContentElement2 = SimHashes.MoltenSalt; break;
                case LiquidOptions.Petroleum: SlugContentElement2 = SimHashes.Petroleum; break;
                case LiquidOptions.DirtyWater: SlugContentElement2 = SimHashes.DirtyWater; break;
                case LiquidOptions.SaltWater: SlugContentElement2 = SimHashes.SaltWater; break;
                case LiquidOptions.SuperCoolant: SlugContentElement2 = SimHashes.SuperCoolant; break;
                case LiquidOptions.ViscoGel: SlugContentElement2 = SimHashes.ViscoGel; break;
                case LiquidOptions.Water: SlugContentElement2 = SimHashes.Water; break;

            }
            switch (SlugContentElementChoice3)
            {
                //case LiquidOptions.Random: SlugContentElement = MoltenSimHashes[UnityEngine.Random.Range(0, MoltenSimHashes.Length)]; break;
                case LiquidOptions.Magma: SlugContentElement3 = SimHashes.Magma; break;
                case LiquidOptions.Aluminum: SlugContentElement3 = SimHashes.MoltenAluminum; break;
                case LiquidOptions.Cobalt: SlugContentElement3 = SimHashes.MoltenCobalt; break;
                case LiquidOptions.Copper: SlugContentElement3 = SimHashes.MoltenCopper; break;
                case LiquidOptions.Gold: SlugContentElement3 = SimHashes.MoltenGold; break;
                case LiquidOptions.Iron: SlugContentElement3 = SimHashes.MoltenIron; break;
                case LiquidOptions.Lead: SlugContentElement3 = SimHashes.MoltenLead; break;
                case LiquidOptions.Niobium: SlugContentElement3 = SimHashes.MoltenNiobium; break;
                case LiquidOptions.Steel: SlugContentElement3 = SimHashes.MoltenSteel; break;
                case LiquidOptions.Tungsten: SlugContentElement3 = SimHashes.MoltenTungsten; break;
                case LiquidOptions.Uranium: SlugContentElement3 = SimHashes.MoltenUranium; break;
                case LiquidOptions.NuclearWaste: SlugContentElement3 = SimHashes.NuclearWaste; break;

                case LiquidOptions.Brackene: SlugContentElement3 = SimHashes.Milk; break;
                case LiquidOptions.Brine: SlugContentElement3 = SimHashes.Brine; break;
                case LiquidOptions.CrudeOil: SlugContentElement3 = SimHashes.CrudeOil; break;
                case LiquidOptions.Ethanol: SlugContentElement3=SimHashes.Ethanol; break;
                case LiquidOptions.LiquidCarbon: SlugContentElement3 = SimHashes.MoltenCarbon; break;
                case LiquidOptions.LiquidCO2: SlugContentElement3 = SimHashes.LiquidCarbonDioxide; break;
                case LiquidOptions.Chlorine: SlugContentElement3 = SimHashes.Chlorine; break;
                case LiquidOptions.Hydrogen: SlugContentElement3=SimHashes.LiquidHydrogen; break;
                case LiquidOptions.Methane: SlugContentElement3=SimHashes.LiquidMethane; break;
                case LiquidOptions.Naptha: SlugContentElement3 = SimHashes.Naphtha; break;
                case LiquidOptions.LiquidO2: SlugContentElement3 = SimHashes.LiquidOxygen; break;
                case LiquidOptions.LiquidPhosphorous: SlugContentElement3 = SimHashes.LiquidPhosphorus; break;
                case LiquidOptions.LiquidResin: SlugContentElement3 = SimHashes.Resin; break;
                case LiquidOptions.LiquidSucrose: SlugContentElement3 = SimHashes.MoltenSucrose; break;
                case LiquidOptions.LiquidSulfur: SlugContentElement3 = SimHashes.LiquidSulfur; break;
                case LiquidOptions.MoltenGlass: SlugContentElement3=SimHashes.MoltenGlass; break;
                case LiquidOptions.MoltenSalt: SlugContentElement3=SimHashes.MoltenSalt; break;
                case LiquidOptions.Petroleum: SlugContentElement3=SimHashes.Petroleum; break;
                case LiquidOptions.DirtyWater: SlugContentElement3 = SimHashes.DirtyWater; break;
                case LiquidOptions.SaltWater: SlugContentElement3 = SimHashes.SaltWater; break;
                case LiquidOptions.SuperCoolant: SlugContentElement3 = SimHashes.SuperCoolant; break;
                case LiquidOptions.ViscoGel: SlugContentElement3 = SimHashes.ViscoGel; break;
                case LiquidOptions.Water: SlugContentElement3 = SimHashes.Water; break;

            }

            switch (TwitchSlugContentElementChoice1)
            {
                //case LiquidOptions.Random: SlugContentElement = MoltenSimHashes[UnityEngine.Random.Range(0, MoltenSimHashes.Length)]; break;
                case LiquidOptions.Magma: TwitchSlugContentElement1 = SimHashes.Magma; break;
                case LiquidOptions.Aluminum: TwitchSlugContentElement1 = SimHashes.MoltenAluminum; break;
                case LiquidOptions.Cobalt: TwitchSlugContentElement1 = SimHashes.MoltenCobalt; break;
                case LiquidOptions.Copper: TwitchSlugContentElement1 = SimHashes.MoltenCopper; break;
                case LiquidOptions.Gold: TwitchSlugContentElement1 = SimHashes.MoltenGold; break;
                case LiquidOptions.Iron: TwitchSlugContentElement1 = SimHashes.MoltenIron; break;
                case LiquidOptions.Lead: TwitchSlugContentElement1 = SimHashes.MoltenLead; break;
                case LiquidOptions.Niobium: TwitchSlugContentElement1 = SimHashes.MoltenNiobium; break;
                case LiquidOptions.Steel: TwitchSlugContentElement1 = SimHashes.MoltenSteel; break;
                case LiquidOptions.Tungsten: TwitchSlugContentElement1 = SimHashes.MoltenTungsten; break;
                case LiquidOptions.Uranium: TwitchSlugContentElement1 =SimHashes.MoltenUranium; break;
                case LiquidOptions.NuclearWaste: TwitchSlugContentElement1 = SimHashes.NuclearWaste; break;                
            }
            switch (TwitchSlugContentElementChoice2)
            {
                //case LiquidOptions.Random: SlugContentElement = MoltenSimHashes[UnityEngine.Random.Range(0, MoltenSimHashes.Length)]; break;
                case LiquidOptions.Magma: TwitchSlugContentElement2 = SimHashes.Magma; break;
                case LiquidOptions.Aluminum: TwitchSlugContentElement2 = SimHashes.MoltenAluminum; break;
                case LiquidOptions.Cobalt: TwitchSlugContentElement2 = SimHashes.MoltenCobalt; break;
                case LiquidOptions.Copper: TwitchSlugContentElement2 = SimHashes.MoltenCopper; break;
                case LiquidOptions.Gold: TwitchSlugContentElement2 = SimHashes.MoltenGold; break;
                case LiquidOptions.Iron: TwitchSlugContentElement2 = SimHashes.MoltenIron; break;
                case LiquidOptions.Lead: TwitchSlugContentElement2 = SimHashes.MoltenLead; break;
                case LiquidOptions.Niobium: TwitchSlugContentElement2 = SimHashes.MoltenNiobium; break;
                case LiquidOptions.Steel: TwitchSlugContentElement2 = SimHashes.MoltenSteel; break;
                case LiquidOptions.Tungsten: TwitchSlugContentElement2 = SimHashes.MoltenTungsten; break;
                case LiquidOptions.Uranium: TwitchSlugContentElement2 = SimHashes.MoltenUranium; break;
                case LiquidOptions.NuclearWaste: TwitchSlugContentElement2 = SimHashes.NuclearWaste; break;
            }
            switch (TwitchSlugContentElementChoice3)
            {
                //case LiquidOptions.Random: SlugContentElement = MoltenSimHashes[UnityEngine.Random.Range(0, MoltenSimHashes.Length)]; break;
                case LiquidOptions.Magma: TwitchSlugContentElement3 = SimHashes.Magma; break;
                case LiquidOptions.Aluminum: TwitchSlugContentElement3 = SimHashes.MoltenAluminum; break;
                case LiquidOptions.Cobalt: TwitchSlugContentElement3 = SimHashes.MoltenCobalt; break;
                case LiquidOptions.Copper: TwitchSlugContentElement3 = SimHashes.MoltenCopper; break;
                case LiquidOptions.Gold: TwitchSlugContentElement3 = SimHashes.MoltenGold; break;
                case LiquidOptions.Iron: TwitchSlugContentElement3 = SimHashes.MoltenIron; break;
                case LiquidOptions.Lead: TwitchSlugContentElement3 = SimHashes.MoltenLead; break;
                case LiquidOptions.Niobium: TwitchSlugContentElement3 = SimHashes.MoltenNiobium; break;
                case LiquidOptions.Steel: TwitchSlugContentElement3 = SimHashes.MoltenSteel; break;
                case LiquidOptions.Tungsten: TwitchSlugContentElement3 = SimHashes.MoltenTungsten; break;
                case LiquidOptions.Uranium: TwitchSlugContentElement3 = SimHashes.MoltenUranium; break;
                case LiquidOptions.NuclearWaste: TwitchSlugContentElement3 = SimHashes.NuclearWaste; break;
            }
            
        }
        public enum LiquidOptions
        {
            [Option("None", "")] None,
            //[Option("Random", "")] Random,
            [Option("Magma","")] Magma,                        
            [Option("Molten Aluminum", "")] Aluminum,
            [Option("Molten Cobalt", "")] Cobalt,
            [Option("Molten Copper", "")] Copper,
            [Option("Molten Gold", "")] Gold,
            [Option("Molten Iron","")] Iron,
            [Option("Molten Lead", "")] Lead,
            [Option("Molten Niobium","")] Niobium,
            [Option("Molten Steel", "")] Steel,
            [Option("Molten Tungsten", "")] Tungsten,
            [Option("Molten Uranium","")] Uranium,
            [Option("Nuclear Waste", "")] NuclearWaste,

            [Option("Brackene", "")] Brackene,
            [Option("Brine", "")] Brine,
            [Option("Crude Oil", "")] CrudeOil,
            [Option("Ethanol", "")] Ethanol,
            [Option("Liquid Carbon")] LiquidCarbon,
            [Option("Liquid Carbon Dioxide","")] LiquidCO2,
            [Option("Liquid Chlorine", "")] Chlorine,
            [Option("Liquid Hydrogen", "")] Hydrogen,
            [Option("Liquid Methane", "")] Methane,
            [Option("Liquid Naphtha","")] Naptha,
            [Option("Liquid Oxygen", "")] LiquidO2,
            [Option("Liquid Phosphorus", "")] LiquidPhosphorous,
            [Option("Liquid Resin", "")] LiquidResin,
            [Option("Liquid Sucrose", "")] LiquidSucrose,
            [Option("Liquid Sulfur", "")] LiquidSulfur,
            [Option("Molten Glass", "")] MoltenGlass,
            [Option("Molten Salt", "")] MoltenSalt,
            [Option("Petroleum","")] Petroleum,
            [Option("Polluted Water", "")] DirtyWater,
            [Option("Salt Water", "")] SaltWater,
            [Option("Super Coolant", "")] SuperCoolant,
            [Option("Visco-Gel Fluid")] ViscoGel,
            [Option("Water", "")] Water,                                               
        }

        public enum RainElementOptions
        {
            [Option("None","")] None,
            [Option("Water","")] Water,
            [Option("Polluted Water","")] PollutedWater,
            [Option("Salt Water", "")] SaltWater,
            [Option("Brine", "")] Brine,
            [Option("Chlorine", "")] Chlorine,
            [Option("Magma", "")] Magma,
            [Option("Nuclear Waste")] NuclearWaste
               
        }
        

        public void InitRainElements()
        {
            switch (RainElementChoiceStartWorld)
            {
                case RainElementOptions.Water: RainElementStartWorld = SimHashes.Water; break;
                case RainElementOptions.PollutedWater: RainElementStartWorld = SimHashes.DirtyWater; break;
                case RainElementOptions.SaltWater: RainElementStartWorld = SimHashes.SaltWater; break;
                case RainElementOptions.Brine: RainElementStartWorld = SimHashes.Brine; break;
                case RainElementOptions.Chlorine: RainElementStartWorld = SimHashes.Chlorine; break;
                case RainElementOptions.Magma: RainElementStartWorld = SimHashes.Magma; break;
                case RainElementOptions.NuclearWaste: RainElementStartWorld = SimHashes.NuclearWaste; break;
            }
            
        }


        
    }
    
}
