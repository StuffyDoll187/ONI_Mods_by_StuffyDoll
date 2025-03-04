using HarmonyLib;


namespace Interstellar_Remote_Workers
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
