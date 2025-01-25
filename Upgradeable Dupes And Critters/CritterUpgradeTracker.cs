using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KSerialization;
using Klei.AI;


namespace Upgradeable_Dupes_And_Critters
{
    public class CritterUpgradeTracker : KMonoBehaviour
    {
        [SerializeField]
        [Serialize]
        public int Upgrades;

        public static int MaxUpgradeLevel = 9;

        public int GetUpgradeLevel() => Upgrades;

        //public Effect CritterUpgradeReproductionRate = new Effect("Happy", STRINGS.CREATURES.MODIFIERS.HAPPY_TAME.NAME, STRINGS.CREATURES.MODIFIERS.HAPPY_TAME.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: false);

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            gameObject.Subscribe((int)GameHashes.BabyToAdult, adult =>
            {
                this.CopyBabyToAdult((GameObject)adult);
                this.SpawnOnGrowUpDrops();
            });            

            if (Config.Instance.Enable_Critter_Upgrade_Propagation_To_Offspring)               
                gameObject.Subscribe((int)GameHashes.LayEgg, egg => this.CopyToEgg((GameObject) egg));            
            
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            gameObject.Unsubscribe((int)GameHashes.LayEgg);
            gameObject.Unsubscribe((int)(GameHashes.BabyToAdult));
        }

        protected override void OnSpawn()
        {            

            //Debug.Log("I am a " + this.gameObject.name +" with " + this.Upgrades + " upgrades");                
        }


        public void CopyToEgg(GameObject egg)
        {           
            if (!egg.TryGetComponent<CritterUpgradeTracker>(out CritterUpgradeTracker eggcomponent))
            {
                Debug.Log("Failed to Get Critter Upgrade Tracker on " + egg.name + "\nAdding manually");
                egg.AddComponent<CritterUpgradeTracker>();                
            }
                //CritterUpgradeTracker eggcomponent = egg.AddOrGet<CritterUpgradeTracker>();               
                eggcomponent.Upgrades = this.Upgrades;       
            
            

        }
                
        public void CopyBabyToAdult(GameObject adult)
        {            
            if (!adult.TryGetComponent<CritterUpgradeTracker>(out CritterUpgradeTracker adultcomponent))
            {
                Debug.Log("Failed to Get Critter Upgrade Tracker on " + adult.name + "\nAdding manually");
                adult.AddComponent<CritterUpgradeTracker>();
                
            }
            //CritterUpgradeTracker adultcmp = adult.AddOrGet<CritterUpgradeTracker>();
            adultcomponent.Upgrades = this.Upgrades;
            if (this.Upgrades > 0)
            {
                Traits traits = adult.GetComponent<Traits>();
                traits.Add(Db.Get().traits.Get("CritterUpgrade" + Upgrades.ToString()));
            }
        }
             
        public void SpawnOnGrowUpDrops()
        {
            
            BabyMonitor.GenericInstance monitor = this.GetSMI<BabyMonitor.GenericInstance>();            
            if ((monitor.def.onGrowDropID != null) && this.Upgrades > 0)
            {
                Vector3 position = this.transform.GetPosition();
                for (int i = 0; i < this.Upgrades; i++)
                Util.KInstantiate(Assets.GetPrefab((Tag) monitor.def.onGrowDropID), position).SetActive(true);
            }
        }
    }
}
