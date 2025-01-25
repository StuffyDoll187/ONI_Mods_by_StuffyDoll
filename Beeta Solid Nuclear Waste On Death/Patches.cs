using HarmonyLib;

namespace Beeta_Solid_Nuclear_Waste_On_Death
{
    public class Patches
    {
        [HarmonyPatch(typeof(Bee))]
        [HarmonyPatch("OnDeath")]
        internal class Bee_OnDeath_Patch
        {
            private static bool Prefix(object data, Bee __instance)
            {
                PrimaryElement component1 = __instance.GetComponent<PrimaryElement>();
                Storage component2 = __instance.GetComponent<Storage>();
                byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
                component2.AddOre(SimHashes.SolidNuclearWaste, BeeTuning.WASTE_DROPPED_ON_DEATH, component1.Temperature, index, BeeTuning.GERMS_DROPPED_ON_DEATH);
                component2.DropAll(__instance.transform.position, true, true);

                return false;
            }
        }
        /*private void OnDeath(object data)
        {
            PrimaryElement component1 = this.GetComponent<PrimaryElement>();
            Storage component2 = this.GetComponent<Storage>();
            byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
            component2.AddOre(SimHashes.NuclearWaste, BeeTuning.WASTE_DROPPED_ON_DEATH, component1.Temperature, index, BeeTuning.GERMS_DROPPED_ON_DEATH);
            component2.DropAll(this.transform.position, true, true);
        }*/


    }
}
