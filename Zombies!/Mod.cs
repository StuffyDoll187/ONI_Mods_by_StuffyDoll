using HarmonyLib;
using PeterHan.PLib.Options;

namespace Zombies
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
