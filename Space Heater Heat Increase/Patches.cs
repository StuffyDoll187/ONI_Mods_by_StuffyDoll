using HarmonyLib;
using KMod;

namespace Space_Heater_Heat_Increase
{
    public class Patches : UserMod2
    {
        [HarmonyPatch(typeof(SpaceHeaterConfig))]
        [HarmonyPatch("CreateBuildingDef")]
        public class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                Debug.Log("I execute before Db.Initialize!");
            }

            public static void Postfix(ref BuildingDef __result)
            {
                __result.ExhaustKilowattsWhenActive = 20;
                Debug.Log("Set Space Heater Exhaust to " + __result.ExhaustKilowattsWhenActive);
            }
        }
    }
}
