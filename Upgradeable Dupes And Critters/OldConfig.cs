/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Upgradeable_Dupes_And_Critters
{
    internal class Config
    {
        public static string configFolderPath;
        public static string configFilePath;
        public static Config config;
        public int Max_Attribute_Level_Per_Upgrade;
        public int Morale_Per_Upgrade;
        public int Piloting_Per_Upgrade;
        public int Construction_Per_Upgrade;
        public int Excavation_Per_Upgrade;
        public int Machinery_Per_Upgrade;
        public int Athletics_Per_Upgrade;
        public int Science_Per_Upgrade;
        public int Cuisine_Per_Upgrade;
        public int Medicine_Per_Upgrade;
        public int Strength_Per_Upgrade;
        public int Creativity_Per_Upgrade;
        public int Agriculture_Per_Upgrade;
        public int Husbandry_Per_Upgrade;
        public bool Enable_Critter_Upgrade_Propagation_To_Offspring;



        public static void LoadConfig()
        {
            // use Kmod.Manager.GetDirectory for mod folder in future
            configFolderPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\config\Upgradeable_Dupes_And_Critters");
            //configFolderPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\config\Upgradeable Dupes And Critters");
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

            config.Max_Attribute_Level_Per_Upgrade = 0;
            config.Morale_Per_Upgrade = 3;
            config.Piloting_Per_Upgrade = 10;
            config.Construction_Per_Upgrade = 10;
            config.Excavation_Per_Upgrade = 10;
            config.Machinery_Per_Upgrade = 10;
            config.Athletics_Per_Upgrade = 10;
            config.Science_Per_Upgrade = 10;
            config.Cuisine_Per_Upgrade = 10;
            config.Medicine_Per_Upgrade = 10;
            config.Strength_Per_Upgrade = 10;
            config.Creativity_Per_Upgrade = 10;
            config.Agriculture_Per_Upgrade = 10;
            config.Husbandry_Per_Upgrade = 10;
            config.Enable_Critter_Upgrade_Propagation_To_Offspring = true;


            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));

        }



    }
}
*/