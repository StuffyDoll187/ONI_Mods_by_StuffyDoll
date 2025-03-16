using HarmonyLib;
using Klei;
using PeterHan.PLib.Options;
using System.Reflection;
using static Unlimited_Autosaves.Autosaves;

namespace Unlimited_Autosaves
{
    public class Mod : KMod.UserMod2
    {
        
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            new POptions().RegisterOptions(this, typeof(Config));

            if (Config.Instance.unlimitedAutosaves)
                SetUnlimitedAutosaves();            
            else
                Autosave_Transpiler.Patch(harmony);
            

        }

                              
    }
}
