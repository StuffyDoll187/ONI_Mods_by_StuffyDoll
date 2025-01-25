using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Upgradeable_Dupes_And_Critters
{
    public class DuplicantUpgradeTracker : KMonoBehaviour
    {
        [SerializeField]
        [Serialize]
        public int Upgrades;
        public static int MaxUpgradeLevel = 9;
        [SerializeField]
        [Serialize]
        public bool FixedPossibleLegacyDupe = false;
        

        public int GetUpgradeLevel() => Upgrades;
        protected override void OnSpawn()
        {
            if (!FixedPossibleLegacyDupe)
                this.SetUpgradesForPossibleLegacyDupe();






            //if (this.TryGetComponent<BionicOxygenTankMonitor.Instance>(out BionicOxygenTankMonitor.Instance oxygenTank))
            var oxygenTank = this.GetSMI<BionicOxygenTankMonitor.Instance>();
            if (oxygenTank != null)
            {                
                oxygenTank.storage.capacityKg = (Upgrades + 1) * BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG;
            }
        }

        private void SetUpgradesForPossibleLegacyDupe()
        {
            Traits traits = this.GetComponent<Traits>();
            Db db = Db.Get();
            for (int i = 1; i <= 9; i++)
            {
                if (traits.HasTrait(db.traits.Get("DuplicantUpgrade" + i.ToString())))
                {
                    //Debug.Log(this.gameObject.name + i + "upgrades");
                    this.Upgrades = i; break;
                }
            }
            this.FixedPossibleLegacyDupe = true;
        }



    }
}
