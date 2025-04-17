using HarmonyLib;
using PeterHan.PLib.Options;

namespace Geyser_Control
{
    public class GeyserControlMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            new POptions().RegisterOptions(this, typeof(Config));
            if (Config.Instance.allowInstantAnalysis) 
                Patches.Studyable_OnSidescreenButtonPressed_Patch.Patch(harmony);
        }




    }
}
