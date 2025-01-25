using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using static TUNING.BUILDINGS;

namespace Upgradeable_Dupes_And_Critters
{
    [AddComponentMenu("KMonoBehaviour/Workable/DuplicantUpgrader")]
    public class DuplicantUpgrader : Workable
    {
        [MyCmpReq]
        public Assignable assignable;
        private Notification notification;
        private Chore chore;
        
        /*public static readonly Chore.Precondition FullyUpgraded = new Chore.Precondition()
        {
            id = nameof(FullyUpgraded),
            description = (string)DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
            fn = (Chore.PreconditionFn)((ref Chore.Precondition.Context context, object data) => ((DuplicantUpgrader)data).IsFullyUpgraded(context.consumerState.gameObject))
        };*/
        
        
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.lightEfficiencyBonus = false;
            this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAssign));
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.OnAssign(this.assignable.assignee);
            this.assignable.OnAssign += new Action<IAssignableIdentity>(this.OnAssign);
            //if (this.Upgrades == -1)
            //    this.UpgradesVariableShouldHaveBeenImplementedTheFirstTime();
        }

        /*private void UpgradesVariableShouldHaveBeenImplementedTheFirstTime()
        {

        }*/

        private void OnAssign(IAssignableIdentity obj)
        {
            if (obj != null)
            {
                this.CreateChore();
            }
            else
            {
                if (this.chore == null)
                    return;
                this.chore.Cancel("Unassigned");
                this.chore = (Chore)null;
            }
        }

        private bool CanAssign(MinionAssignablesProxy worker)
        {
            bool flag1 = false;
            GameObject go = worker.GetTargetGameObject();
            Traits traits = go.GetComponent<Traits>();
            flag1 = traits.HasTrait("GlowStick");
            bool flag2 = false;
            MinionIdentity target = worker.target as MinionIdentity;
            if (target != null)
                flag2 = !this.IsFullyUpgraded(target.gameObject);
            return flag1 && flag2;
        }
        public bool IsFullyUpgraded(GameObject minion)
        {
            DuplicantUpgradeTracker component = minion.GetComponent<DuplicantUpgradeTracker>();
            return component.Upgrades >= DuplicantUpgradeTracker.MaxUpgradeLevel;
        }
        private void CreateChore()
        {
            this.chore = (Chore)new WorkChore<DuplicantUpgrader>(Db.Get().ChoreTypes.GeneShuffle, (IStateMachineTarget)this, allow_in_red_alert: false, ignore_schedule_block: true, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.high);
        }

        protected override void OnStartWork(WorkerBase worker)
        {
            base.OnStartWork(worker);
            this.GetComponent<Operational>().SetActive(true);
            //this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, (object)this);
        }

        protected override void OnCompleteWork(WorkerBase worker)
        {
            base.OnCompleteWork(worker);
            this.assignable.Unassign();
            Db db = Db.Get();
            Traits traits = worker.GetComponent<Traits>();
            DuplicantUpgradeTracker tracker = worker.GetComponent<DuplicantUpgradeTracker>();
            tracker.Upgrades += 1;
            traits.Add(db.traits.Get("DuplicantUpgrade" + tracker.Upgrades.ToString()));
            if (tracker.Upgrades > 1)
                traits.Remove(db.traits.Get("DuplicantUpgrade" + (tracker.Upgrades - 1).ToString()));
                       
            traits.Remove(db.traits.Get("GlowStick"));
            GlowStick glowstick = worker.GetComponent<GlowStick>();  
            glowstick.enabled = false;
                        
            RadiationEmitter emitter = worker.GetComponent<RadiationEmitter>();            
            emitter.SetEmitting(false);
            emitter.emitRads = 0;
            
            UpgradeFX.Instance instance = new UpgradeFX.Instance((IStateMachineTarget)this, new Vector3(0.0f, 0.0f, -0.1f));
            instance.StartSM();
            EmoteChore emoteChore1 = new EmoteChore((IStateMachineTarget)worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.ClapCheer);

            var oxygenTank = worker.GetSMI<BionicOxygenTankMonitor.Instance>();
            if (oxygenTank != null)
            {
                oxygenTank.storage.capacityKg = (tracker.Upgrades + 1) * BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG;
            }


        }

        protected override void OnStopWork(WorkerBase worker)
        {
            base.OnStopWork(worker);
            this.GetComponent<Operational>().SetActive(false);
            //this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, (bool)(UnityEngine.Object)this);
            this.chore = (Chore)null;
        }
    }
}