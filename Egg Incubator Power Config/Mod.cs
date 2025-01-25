using HarmonyLib;
using PeterHan.PLib.Options;
using static STRINGS.UI.UISIDESCREENS;

namespace Egg_Incubator_Power_Config
{
    public class EggIncubatorPowerMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            new POptions().RegisterOptions(this, typeof(Config));
        }




    }
}
