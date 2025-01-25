/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tuning
{
    internal class Config
    {
        public static string configFolderPath;
        public static string configFilePath;
        public static Config config;
        public int MAX_GAINED_ATTRIBUTE_LEVEL;
        public int TARGET_MAX_LEVEL_CYCLE;
        public float EXPERIENCE_LEVEL_POWER;
        public float PASSIVE_EXPERIENCE_PORTION;


        public static void LoadConfig()
        {          
            configFolderPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\config\Tuning");
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

            config.MAX_GAINED_ATTRIBUTE_LEVEL = 20;
            config.TARGET_MAX_LEVEL_CYCLE = 400;
            config.EXPERIENCE_LEVEL_POWER = 1.7f;
            config.PASSIVE_EXPERIENCE_PORTION = 0.5f;

            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
            //Debug.Log(configFilePath);
            //Debug.Log(configFolderPath);
        }

        

    }
}
*/