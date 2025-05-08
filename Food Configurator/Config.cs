using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Configurator
{
    internal class Config
    {
        public static string configFolderPath;
        public static string configFilePath;
        //public static Config Instance;

        
        //public static Dictionary<string, FoodOptions> customFoodInfo;
        public static List<FoodOptions> customFoodOptions;

        public static void LoadConfig()
        {
            configFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            configFilePath = configFolderPath + @"\config.json";
            if (!Directory.Exists(configFolderPath)) Directory.CreateDirectory(configFolderPath);
            if (!File.Exists(configFilePath))
            {
                GenerateConfig();
                return;
            }
            customFoodOptions = JsonConvert.DeserializeObject<List<FoodOptions>>(File.ReadAllText(configFilePath));
        }
        public static void GenerateConfig()
        {
            customFoodOptions = new List<FoodOptions>();
            foreach (var foodInfo in EdiblesManager.s_allFoodMap.Values)
            {
                string name = foodInfo.Name;
                int start = name.IndexOf('>');
                start += 1;
                int end = name.LastIndexOf('<');
                if (start != -1 && end != -1)
                    name = name.Substring(start, end - start);
                customFoodOptions.Add(new FoodOptions(name, foodInfo.Id,(int)foodInfo.CaloriesPerUnit / 1000, foodInfo.Quality, foodInfo.CanRot, (int)(foodInfo.SpoilTime / 600), (int)(foodInfo.RotTemperature - 273.15f), (int)(foodInfo.PreserveTemperature - 273.15f), foodInfo.Effects));
            }
            customFoodOptions.Sort((x, y) => { return String.Compare(x.name, y.name); });
            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(customFoodOptions, Formatting.Indented));
            
        }
        

        public static void CheckForAdditionalFoods()
        {
            if (customFoodOptions.Count < EdiblesManager.s_allFoodMap.Count)
            {
                bool addedNewEntry = false;
                foreach (var foodInfo in EdiblesManager.s_allFoodMap.Values)
                {
                    bool found = false;
                    for (int i = 0; i < customFoodOptions.Count; i++)
                    {
                        
                        if (customFoodOptions[i].id == foodInfo.Id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        string name = foodInfo.Name;
                        int start = name.IndexOf('>');
                        start += 1;
                        int end = name.LastIndexOf('<');
                        if (start != -1 && end != -1)
                            name = name.Substring(start, end - start);
                        customFoodOptions.Add(new FoodOptions(name, foodInfo.Id, (int)foodInfo.CaloriesPerUnit / 1000, foodInfo.Quality, foodInfo.CanRot, (int)(foodInfo.SpoilTime / 600), (int)(foodInfo.RotTemperature - 273.15f), (int)(foodInfo.PreserveTemperature - 273.15f),foodInfo.Effects));
                        addedNewEntry = true;
                    }
                }
                if (addedNewEntry)
                {
                    customFoodOptions.Sort((x, y) => { return String.Compare(x.name, y.name); });
                    File.WriteAllText(configFilePath, JsonConvert.SerializeObject(customFoodOptions, Formatting.Indented));
                }
            }
        }
        public static void ApplyConfig()
        {
            foreach (var id in EdiblesManager.s_allFoodMap.Keys)
            {
                var foodOptions = customFoodOptions.Find(match => match.id == id);
                EdiblesManager.s_allFoodMap[id].CaloriesPerUnit = foodOptions.kCal * 1000;
                EdiblesManager.s_allFoodMap[id].Quality = foodOptions.quality;
                EdiblesManager.s_allFoodMap[id].CanRot = foodOptions.canRot;
                EdiblesManager.s_allFoodMap[id].SpoilTime = foodOptions.cyclesUntilRot * 600;
                EdiblesManager.s_allFoodMap[id].StaleTime = foodOptions.cyclesUntilRot * 300;
                EdiblesManager.s_allFoodMap[id].RotTemperature = foodOptions.fridgeTempC + 273.15f;
                EdiblesManager.s_allFoodMap[id].PreserveTemperature = foodOptions.freezeTempC + 273.15f;
                EdiblesManager.s_allFoodMap[id].Effects = foodOptions.effects;
            }
        }
    }
}
