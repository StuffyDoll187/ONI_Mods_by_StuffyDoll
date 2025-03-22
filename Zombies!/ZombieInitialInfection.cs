using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KSerialization;
using Klei.AI;

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
                init = true;                
                MinionModifiers.sicknesses.Infect(new SicknessExposureInfo(Db.Get().Sicknesses.ZombieSickness.Id, "Zombie Challenge"));               
            }
        }
    }
        
    [HarmonyPatch(typeof(BaseMinionConfig), nameof(BaseMinionConfig.BasePrefabInit))]
    public class MinionConfig_BasePrefabInit_Patch
    {
        public static void Postfix(GameObject go)
        {
            if (CustomGameSettings.Instance.GetCurrentQualitySetting(CustomSettings.Zombies).id == "Enabled")
            {
                go.AddComponent<ZombieInitialInfection>();
            }
        }
    }

    [HarmonyPatch(typeof(SaveGame), "OnSpawn")]
    public class SaveGame_OnSpawn_Patch
    {
        public static void Postfix()
        {
            bool flag = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomSettings.Zombies).id == "Enabled";
            Debug.Log("[Zombies!] Enabled = " + flag);
            if (flag)
                Db.Get().Sicknesses.ZombieSickness.cureSpeedBase.BaseValue = 0;
            else
                Db.Get().Sicknesses.ZombieSickness.cureSpeedBase.BaseValue = 1;
        }
    }

}
