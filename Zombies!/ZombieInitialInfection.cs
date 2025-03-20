using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KSerialization;

namespace Zombies
{
    public class ZombieInitialInfection : KMonoBehaviour
    {
        [MyCmpGet]
        private MinionModifiers MinionModifiers;
        [Serialize]
        private bool init;
        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (!init)
            {
                MinionModifiers.sicknesses.Infect(new SicknessExposureInfo(Db.Get().Sicknesses.ZombieSickness.Id, "Zombie Challenge"));
                init = true;
            }
            
        }
    }
    [HarmonyPatch(typeof(BaseMinionConfig), nameof(BaseMinionConfig.BaseMinion))] 
    public class MinionConfig_CreatePrefab_Patch
    {
        public static void Postfix(GameObject __result)
        {
            __result.AddComponent<ZombieInitialInfection>();                        
        }
    }
}
