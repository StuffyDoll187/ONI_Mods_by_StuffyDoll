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
        internal bool beingDeserialized;
        internal bool serializedInfected;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (CustomGameSettings.Instance.GetCurrentQualitySetting(CustomSettings.Zombies).id == "Disabled")
                return;
            
            if (!init)
            {
                init = true;
                if (!beingDeserialized)
                    MinionModifiers.sicknesses.Infect(new SicknessExposureInfo(Db.Get().Sicknesses.ZombieSickness.Id, "Zombie Challenge"));
                else
                {
                    if (serializedInfected)
                        MinionModifiers.sicknesses.Infect(new SicknessExposureInfo(Db.Get().Sicknesses.ZombieSickness.Id, "Zombie Challenge"));
                }
            }            
        }
    }

    [HarmonyPatch(typeof(BaseMinionConfig), nameof(BaseMinionConfig.BasePrefabInit))]
    public class MinionConfig_BaseOnSpawn_Patch
    {
        public static void Postfix(GameObject go)
        {            
            //the Game Setting has not updated for the current save at this point so it is instead checked at the beginning of each method  :\
            go.AddComponent<ZombieInitialInfection>();            
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
