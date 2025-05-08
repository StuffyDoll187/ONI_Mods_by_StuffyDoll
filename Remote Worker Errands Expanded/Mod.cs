using HarmonyLib;
using PeterHan.PLib.Options;

namespace Remote_Worker_Errands_Expanded
{
    public class Mod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            //MorbRoverMakerRemote.Db_Initialize_Patch.Harmony = harmony;
            //new POptions().RegisterOptions(this, typeof(Config));            
        }




    }
}
