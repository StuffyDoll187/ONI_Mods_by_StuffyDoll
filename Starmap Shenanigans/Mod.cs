using HarmonyLib;
using PeterHan.PLib.Options;

namespace Starmap_Shenanigans
{
    public class Mod : KMod.UserMod2
    {
        
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            MorphHarvestablePOIPatches.Db_Initialize_Patch.Harmony = harmony;
            //new POptions().RegisterOptions(this, typeof(Config));            
        }


    }
}
