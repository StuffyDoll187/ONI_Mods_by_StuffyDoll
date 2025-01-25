using HarmonyLib;
using PeterHan.PLib.Options;

namespace Engies_Tuneup_Inactivity_Refund
{
    public class EngiesTuneupInactivityRefundMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            new POptions().RegisterOptions(this, typeof(Config));

            if (Config.Instance.addRefundTracker)
            {
                Patches.Tinkerable_MakePowerTinkerable_Patch.Patch(harmony);
                Patches.Tinkerable_OnCompleteWork_Patch.Patch(harmony);
            }

            
        }

    }
}
