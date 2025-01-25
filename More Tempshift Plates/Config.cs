using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace More_Tempshift_Plates
{
    //[UseSharedConfigLocation]
    [ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
    [RestartRequired]
    internal class Config : SingletonOptions<Config>
    {
        /*[Option("Wattage", "How many watts you can use before exploding.")]
        [Limit(1, 50000)]
        [JsonProperty]
        public float Watts { get; set; }*/

        
        [Option("Enable Modification of Vanilla Plates", "Unchecked = No Changes to Vanilla Plates", "Vanilla Plates")]
        [JsonProperty]
        public bool Enable_Vanilla_Plate_Modification { get; set; }

        [Option("Enable Heat Exchange Mechanic", "Enable Modification must be checked\nApplies only to Vanilla Plates", "Vanilla Plates")]
        [JsonProperty]
        public bool Enable_Vanilla_Plate_Heat_Exchange_Component { get; set; }

        [Option("Change Build Location Rule to Anywhere", "Enable Modification must be checked", "Vanilla Plates")]
        [JsonProperty]
        public bool Enable_Vanilla_Plate_Build_Location_Rule_Change { get; set; }

        [Option("Enable BugFix for rare bug with Extents", "Enable Modification must be checked", "Vanilla Plates")]
        [JsonProperty]
        public bool Enable_Vanilla_Plate_Extents_BugFix { get; set; }

        [Option("Enable Heat Exchange Mechanic", "Applies to All Plates in This Category", "Other Plates")]
        [JsonProperty]
        public bool Enable_Heat_Exchange_Component { get; set; }

        [Option("Default Toggle Setting", "Checked = Default On", "Heat Exchange Mechanic")]
        [JsonProperty]
        public bool Default_Toggle_Setting { get; set; }

        [Option("Restrict To Other Plates", "Plates will only exchange heat with each other", "Heat Exchange Mechanic")]
        [JsonProperty]
        public bool Restrict_To_Other_Plates { get; set; }

        [Option("Enable Dense Tile", "", "Zeus' Dense Tiles")]
        [JsonProperty]
        public bool Enable_Dense_Tile { get; set; }

        [Option("Dense Tile Mass", "kilograms", "Zeus' Dense Tiles")]      
        [JsonProperty]
        public int Dense_Tile_Mass { get; set; }

        [Option("Enable Dense Metal Tile", "", "Zeus' Dense Tiles")]
        [JsonProperty]
        public bool Enable_Dense_Metal_Tile { get; set; }

        [Option("Dense Metal Tile Mass", "kilograms", "Zeus' Dense Tiles")]        
        [JsonProperty]
        public int Dense_Metal_Tile_Mass { get; set; }

        [Option("Enable Morphable Tempshift Plate", "", "Other Plates")]
        [JsonProperty]
        public bool Enable_Morph_Tempshift_Plate { get; set; }

        [Option("Enable Tempshift Plate 2x2", "", "Other Plates")]
        [JsonProperty]
        public bool Enable_Tempshift_Plate_2x2 { get; set; }

        [Option("Enable Very Dense Tempshift Plate", "", "Other Plates")]
        [JsonProperty]
        public bool Enable_Very_Dense_Tempshift_Plate { get; set; }

        [Option("Very Dense Tempshift Plate Mass", "kilograms", "Other Plates")]       
        [JsonProperty]
        public int Very_Dense_Tempshift_Plate_Mass { get; set; }


        public Config()
        {
            //Watts = 10000f; // defaults to 10000, e.g. if the config doesn't exist            
            Enable_Vanilla_Plate_Modification = true;
            Enable_Vanilla_Plate_Heat_Exchange_Component = true;
            Enable_Vanilla_Plate_Build_Location_Rule_Change = true;
            Enable_Vanilla_Plate_Extents_BugFix = true;
            Enable_Heat_Exchange_Component = true;
            Default_Toggle_Setting = true;
            Restrict_To_Other_Plates = false;
            Enable_Dense_Tile = false;
            Dense_Tile_Mass = 2000;
            Enable_Dense_Metal_Tile = false;
            Dense_Metal_Tile_Mass = 1000;
            Enable_Morph_Tempshift_Plate = true;
            Enable_Tempshift_Plate_2x2 = true;
            Enable_Very_Dense_Tempshift_Plate = true;
            Very_Dense_Tempshift_Plate_Mass = 4000;
        }
    }
}
