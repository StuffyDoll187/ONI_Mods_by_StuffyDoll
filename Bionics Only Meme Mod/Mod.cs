using HarmonyLib;
using PeterHan.PLib.Options;

namespace Bionics_Only_Meme_Mod
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
