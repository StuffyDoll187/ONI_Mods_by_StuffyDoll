using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies
{
    public class Patches
    {
        [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public class Db_Init_Patch
        {
            public static void Postfix()
            {
                Db.Get().Sicknesses.ZombieSickness.cureSpeedBase.BaseValue = 0;
            }
        }

    }
}
