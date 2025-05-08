using HarmonyLib;
using PeterHan.PLib.Options;

namespace Food_Configurator
{
    public class Mod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);            
            //new POptions().RegisterOptions(this, typeof(Config));            
        }




    }
}
