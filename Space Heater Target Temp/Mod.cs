using HarmonyLib;
using KMod;

namespace SpaceHeaterTargetTemp
{
    public class SpaceHeaterTargetTempMod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("base.OnLoad(harmony)");
        }
    }
}