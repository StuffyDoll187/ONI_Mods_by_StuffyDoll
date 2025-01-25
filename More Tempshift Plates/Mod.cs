using HarmonyLib;
using PeterHan.PLib.Options;
using STRINGS;

namespace More_Tempshift_Plates
{
    public class MoreTempshiftPlatesMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {           
            base.OnLoad(harmony);
            new POptions().RegisterOptions(this, typeof(Config));
            if (Config.Instance.Enable_Vanilla_Plate_Modification)
            {
                ThermalBlockPatches.ThermalBlockConfig_DoPostConfigureComplete_Patch.Patch(harmony);

                if (Config.Instance.Enable_Vanilla_Plate_Build_Location_Rule_Change)
                    ThermalBlockPatches.ThermalBlockConfig_CreateBuildingDef_Patch.Patch(harmony);
            }
        }
        
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        internal class MoreTempshiftPlatesGeneratedBuildingsLoadGeneratedBuildings
        {
            private static void Prefix()
            {

                Strings.Add("STRINGS.BUILDINGS.PREFABS.TEMPSHIFTPLATE2X2.NAME", UI.FormatAsLink("Tempshift Plate 2x2",""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.TEMPSHIFTPLATE2X2.DESC", "");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.TEMPSHIFTPLATE2X2.EFFECT", "Rotatable, 2x2 Area of Effect");

                Strings.Add("STRINGS.BUILDINGS.PREFABS.VERYDENSETEMPSHIFTPLATE.NAME", UI.FormatAsLink("Very Dense Tempshift Plate",""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.VERYDENSETEMPSHIFTPLATE.DESC", "No Building Mass Penalty");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.VERYDENSETEMPSHIFTPLATE.EFFECT", "5x Mass, 1/5 Thermal Conductivity, No Range");
               
                Strings.Add("STRINGS.BUILDINGS.PREFABS.MORPHTEMPSHIFTPLATE.NAME", UI.FormatAsLink("Morphable Tempshift Plate", ""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.MORPHTEMPSHIFTPLATE.DESC", "");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.MORPHTEMPSHIFTPLATE.EFFECT", "Configurable Area of Effect");
                
                Strings.Add("STRINGS.SLIDERS.MULTISLIDEREXTENTS.NAME", "Range of Effect Sliders");
                Strings.Add("STRINGS.SLIDERS.MINXCONTROLLER.NAME", "Left");
                Strings.Add("STRINGS.SLIDERS.MAXXCONTROLLER.NAME", "Right");
                Strings.Add("STRINGS.SLIDERS.MINYCONTROLLER.NAME", "Down");
                Strings.Add("STRINGS.SLIDERS.MAXYCONTROLLER.NAME", "Up");

                Strings.Add("STRINGS.BUILDINGS.PREFABS.DENSETILE.NAME", UI.FormatAsLink("Dense Tile", ""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.DENSETILE.DESC", STRINGS.BUILDINGS.PREFABS.TILE.DESC);
                Strings.Add("STRINGS.BUILDINGS.PREFABS.DENSETILE.EFFECT", STRINGS.BUILDINGS.PREFABS.TILE.EFFECT);

                Strings.Add("STRINGS.BUILDINGS.PREFABS.DENSEMETALTILE.NAME", UI.FormatAsLink("Dense Metal Tile", ""));
                Strings.Add("STRINGS.BUILDINGS.PREFABS.DENSEMETALTILE.DESC", STRINGS.BUILDINGS.PREFABS.METALTILE.DESC);
                Strings.Add("STRINGS.BUILDINGS.PREFABS.DENSEMETALTILE.EFFECT", STRINGS.BUILDINGS.PREFABS.METALTILE.EFFECT);

                if (Config.Instance.Enable_Tempshift_Plate_2x2)
                {                
                    ModUtil.AddBuildingToPlanScreen("Utilities", "TempshiftPlate2x2", "temperature", "ThermalBlock");
                    TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("TempshiftPlate2x2", "temperature");
                }
                if (Config.Instance.Enable_Very_Dense_Tempshift_Plate)
                {
                    ModUtil.AddBuildingToPlanScreen("Utilities", "VeryDenseTempshiftPlate", "temperature", "TempshiftPlate2x2");
                    TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("VeryDenseTempshiftPlate", "temperature");
                }
                if (Config.Instance.Enable_Morph_Tempshift_Plate)
                {
                    ModUtil.AddBuildingToPlanScreen("Utilities", "MorphTempshiftPlate", "temperature", "VeryDenseTempshiftPlate");
                    TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("MorphTempshiftPlate", "temperature");
                }
                if (Config.Instance.Enable_Dense_Tile)
                {
                    ModUtil.AddBuildingToPlanScreen("Base", "DenseTile", "tiles", "Tile");
                    TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("DenseTile", "tiles");
                }
                if (Config.Instance.Enable_Dense_Metal_Tile)
                {
                    ModUtil.AddBuildingToPlanScreen("Base", "DenseMetalTile", "tiles", "MetalTile");
                    TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add("DenseMetalTile", "tiles");
                }

            }
        }
        [HarmonyPatch(typeof(Db), "Initialize")]
        internal class MoreTempshiftPlatesDbInitialize
        {
            private static void Postfix()
            {
                if (Config.Instance.Enable_Tempshift_Plate_2x2)
                    Db.Get().Techs.Get("RefinedObjects").unlockedItemIDs.Add("TempshiftPlate2x2");
                if (Config.Instance.Enable_Very_Dense_Tempshift_Plate)
                    Db.Get().Techs.Get("RefinedObjects").unlockedItemIDs.Add("VeryDenseTempshiftPlate");
                if (Config.Instance.Enable_Morph_Tempshift_Plate)
                    Db.Get().Techs.Get("RefinedObjects").unlockedItemIDs.Add("MorphTempshiftPlate");
                if (Config.Instance.Enable_Dense_Metal_Tile)
                    Db.Get().Techs.Get("Smelting").unlockedItemIDs.Add("DenseMetalTile");
            }
        }
    }
}
