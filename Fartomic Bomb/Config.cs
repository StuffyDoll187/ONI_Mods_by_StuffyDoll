using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fartomic_Bomb
{
    internal class Config
    {
        public static string configFolderPath;
        public static string configFilePath;
        public static Config config;
        public int RerollsToReduceOddsOfHotGases;
        public bool AllowMetallicGases;        
        public float EmitMassKg;
        public float NotificationToEmissionDelay;
        public float FrequencyAverageInCycles;        
        public float FrequencyMin;
        public float FrequencyMax;
        public float FrequencyStdDeviation;
        public bool RedAlertOnEmission;
        public bool OverrideOutputTemperature;
        public float TemperatureOverrideInCelsius;
        
        //public bool ResetTimeRemainingForAllDuplicantsNextLoad;

        public static void LoadConfig()
        {
            //configFolderPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\config\FartomicBomb");
            configFolderPath = Path.Combine(KMod.Manager.GetDirectory(), "config", "FartomicBomb");
            configFilePath = configFolderPath + @"\config.json";
            if (!Directory.Exists(configFolderPath)) Directory.CreateDirectory(configFolderPath);
            if (!File.Exists(configFilePath))
            {
                GenerateConfig();
                return;
            }
            //JsonSerializerSettings settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error };
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));            
        }
        public static void GenerateConfig()
        {
            //Debug.Log("GenerateConfig Enter Method");
            config = new Config();

            config.RerollsToReduceOddsOfHotGases = 2;
            config.AllowMetallicGases = false;            
            config.EmitMassKg = 1000f;
            config.NotificationToEmissionDelay = 12f;
            config.FrequencyAverageInCycles = 50f;
            config.FrequencyMin = 25f;
            config.FrequencyMax = 100f;
            config.FrequencyStdDeviation = 15f;
            config.RedAlertOnEmission = true;
            config.OverrideOutputTemperature = false;
            config.TemperatureOverrideInCelsius = 37;
            
            //config.CalculateNewTimeRemainingForAllDuplicantsNextLoad = false;

            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
            //Debug.Log(configFilePath);
            //Debug.Log(configFolderPath);
        }

        /*public class BooleanEntry
        {
            public string Name;
            public bool Boolean;
        }

        public class ConfigEntry
        {
            public string Name;
            public bool Bool;
        }*/
        /*public List<ConfigEntry> configlist = new List<ConfigEntry>()
        {
            //new ConfigEntry { Name = "entry 1", Bool = true},
            new ConfigEntry { Name = "Allow Unsafe Gases", Bool = false}

        };*/
        
        /*public static void Configtest()
        {
            string output = JsonConvert.SerializeObject(config);
            Debug.Log(output);
            
        }
        public static string GetPath()
        {
            string configPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            configPath = Path.Combine(configPath, @"..\..\config\testing");
            Debug.Log(configPath);
            //DirectoryInfo directoryInfo = new DirectoryInfo(configPath);

            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
            string filePath = Path.Combine(configPath + @"\config.json");
            string output = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(filePath, output);
            //File.WriteAllLines(filePath, output);

            return filePath;
        }*/

    }
}
