using HarmonyLib;
using PeterHan.PLib.Options;

namespace Smaller_Camera_Zoom_In
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
