using HarmonyLib;
using PeterHan.PLib.Options;

namespace Upgradeable_Dupes_And_Critters
{
    public class Upgradeable_Dupes_And_Critters_Mod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            new POptions().RegisterOptions(this, typeof(Config));  
            
        }




    }
}
