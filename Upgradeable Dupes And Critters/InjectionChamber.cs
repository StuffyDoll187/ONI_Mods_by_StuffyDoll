// Decompiled with JetBrains decompiler
// Type: InjectionChamber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B86E23FE-3B43-4053-84B0-ABB90493789E
// Assembly location: E:\SteamLibrary\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SituationalAnim;
using static UnityEngine.GraphicsBuffer;

namespace Upgradeable_Dupes_And_Critters
{
    [AddComponentMenu("KMonoBehaviour/Workable/InjectionChamber")]
    public class InjectionChamber : Workable
    {
        private static readonly EventSystem.IntraObjectHandler<InjectionChamber> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<InjectionChamber>((Action<InjectionChamber, object>)((component, data) => component.OnStorageChange(data)));
        [MyCmpReq]
        public Storage storage;
        [MyCmpReq]
        public Operational operational;
        [MyCmpReq]
        public Assignable assignable;
        private InjectionChamberDoctor doctor_workable;
        //private string requiredPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;
        private Dictionary<HashedString, Tag> treatments_available = new Dictionary<HashedString, Tag>();
        private InjectionChamber.StatesInstance smi;
        public static readonly Chore.Precondition TreatmentAvailable = new Chore.Precondition()
        {
            id = nameof(TreatmentAvailable),
            description = (string)DUPLICANTS.CHORES.PRECONDITIONS.TREATMENT_AVAILABLE,
            fn = (Chore.PreconditionFn)((ref Chore.Precondition.Context context, object data) => ((InjectionChamber)data).IsTreatmentAvailable(context.consumerState.gameObject))
        };
        public static readonly Chore.Precondition DoctorAvailable = new Chore.Precondition()
        {
            id = nameof(DoctorAvailable),
            description = (string)DUPLICANTS.CHORES.PRECONDITIONS.DOCTOR_AVAILABLE,
            fn = (Chore.PreconditionFn)((ref Chore.Precondition.Context context, object data) => ((InjectionChamber)data).IsDoctorAvailable(context.consumerState.gameObject))
        };
        /*public static readonly Chore.Precondition AdvancedMedicalDoctor = new Chore.Precondition()
        {
            id = nameof(AdvancedMedicalDoctor),
            description = (string)DUPLICANTS.CHORES.PRECONDITIONS.DOCTOR_AVAILABLE,
            fn = (Chore.PreconditionFn)((ref Chore.Precondition.Context context, object data) => ((InjectionChamber)data).IsAdvancedMedicalDoctor(context.consumerState.gameObject))
        };*/

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanManuallyAssignTo));
            this.assignable.AddAutoassignPrecondition(new Func<MinionAssignablesProxy, bool> (this.ReturnFalse));
        } 
        private bool ReturnFalse(MinionAssignablesProxy _) {  return false; }
        protected override void OnSpawn()
        {
            base.OnSpawn();
            Prioritizable.AddRef(this.gameObject);
            this.doctor_workable = this.GetComponent<InjectionChamberDoctor>();
            this.SetWorkTime(float.PositiveInfinity);
            this.smi = new InjectionChamber.StatesInstance(this);
            this.smi.StartSM();
            this.OnStorageChange();
            this.Subscribe<InjectionChamber>(-1697596308, InjectionChamber.OnStorageChangeDelegate);
            this.OnAssign(this.assignable.assignee);
            this.assignable.OnAssign += new Action<IAssignableIdentity>(this.OnAssign);
            
        }

        protected override void OnCleanUp()
        {
            Prioritizable.RemoveRef(this.gameObject);
            if (this.smi != null)
            {
                this.smi.StopSM(nameof(OnCleanUp));
                this.smi = (InjectionChamber.StatesInstance)null;
            }
            base.OnCleanUp();
        }

        private void OnStorageChange(object data = null)
        {
            this.treatments_available.Clear();
            bool flag = false;
            foreach (GameObject go in this.storage.items)
            {
                //Debug.Log(go.name);
                if (go.name == "RadioactiveSerum") flag = true;
                /*MedicinalPill component = go.GetComponent<MedicinalPill>();
                if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                {
                    Tag tag = go.PrefabID();
                    foreach (string curedSickness in component.info.curedSicknesses)
                        this.AddTreatment(curedSickness, tag);
                }*/
            }
            this.smi.sm.hasSupplies.Set(flag, this.smi);
        }
        private void OnAssign(IAssignableIdentity identity) 
        {
            if (identity != null) 
                this.smi.sm.hasAssignee.Set(true, this.smi);
            else
                this.smi.sm.hasAssignee.Set(false, this.smi);
        }
        private void AddTreatment(string id, Tag tag)
        {
            if (this.treatments_available.ContainsKey((HashedString)id))
                return;
            this.treatments_available.Add((HashedString)id, tag);
        }

        protected override void OnStartWork(WorkerBase worker)
        {
            base.OnStartWork(worker);
            this.smi.sm.hasPatient.Set(true, this.smi); 
            
            //Debug.Log("Start Work" + this.smi.sm.hasPatient.Get(this.smi));
        }

        protected override void OnStopWork(WorkerBase worker)
        {
            base.OnStopWork(worker);
            this.smi.sm.hasPatient.Set(false, this.smi);
            //Debug.Log("Stop Work"+ this.smi.sm.hasPatient.Get(this.smi));
        }

        protected override void OnCompleteWork(WorkerBase worker)
        {
            base.OnCompleteWork(worker);
            //this.SetWorkTime(float.PositiveInfinity);
        }
        public override bool InstantlyFinish(WorkerBase worker) => false;

        public void SetHasDoctor(bool has) => this.smi.sm.hasDoctor.Set(has, this.smi);

        public void CompleteDoctoring()
        {
            if (!(bool)(UnityEngine.Object)this.worker)
                return;
            this.CompleteDoctoring(this.worker.gameObject);
        }

        private void CompleteDoctoring(GameObject target)
        {
            Traits traits = target.GetComponent<Traits>();
            traits.Add(Db.Get().traits.Get("GlowStick"));
            if (target.TryGetComponent<GlowStick>(out GlowStick glowstick))
                glowstick.enabled = true;
            if (target.TryGetComponent<RadiationEmitter>(out RadiationEmitter emitter))
            {
                emitter.SetEmitting(true);
                emitter.emitRads = 100f;
                //emitter.enabled = true;
            }
            //target.AddComponent<GlowStick>();
            //RadiationEmitter emitter = target.GetComponent<RadiationEmitter>();
            //emitter.emitRads = 100f;
            //emitter.SetEmitting(true);
            //GlowStick glowStick = target.GetComponent<GlowStick>();
            //glowStick.smi.StartSM();
            //this.SetWorkTime(0f);
            this.assignable.Unassign();
            this.smi.sm.hasAssignee.Set(false, this.smi);
            this.smi.sm.hasPatient.Set(false, this.smi);
            this.storage.ConsumeIgnoringDisease("RadioactiveSerum", 1f);
            /*Sicknesses sicknesses = target.GetSicknesses();
            if (sicknesses == null)
                return;
            bool flag = false;
            foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>)sicknesses)
            {
                Tag tag;
                if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
                {
                    Game.Instance.savedInfo.curedDisease = true;
                    sicknessInstance.Cure();
                    this.storage.ConsumeIgnoringDisease(tag, 1f);
                    flag = true;
                    break;
                }
            }
            if (flag)
                return;
            Debug.LogWarningFormat((UnityEngine.Object)this.gameObject, "Failed to treat any disease for {0}", (object)target);*/
        }

        public bool IsDoctorAvailable(GameObject target)
        {
            return string.IsNullOrEmpty(InjectionChamberConfig.requiredSkillPerk) || MinionResume.AnyOtherMinionHasPerk(InjectionChamberConfig.requiredSkillPerk, target.GetComponent<MinionResume>());
        }
        /*public bool IsAdvancedMedicalDoctor(GameObject target)
        {
            var resume = target.GetComponent<MinionResume>();
            return resume.HasPerk(InjectionChamberConfig.requiredSkillPerk);
        }*/
        public bool IsTreatmentAvailable(GameObject target)
        {
            bool flag1 = this.smi.sm.hasSupplies.Get(this.smi);
            Traits traits = target.GetComponent<Traits>();
            bool flag2 = traits.HasTrait("GlowStick");
            return (!flag2 && flag1);
            /*Sicknesses sicknesses = target.GetSicknesses();
            if (sicknesses != null)
            {
                foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>)sicknesses)
                {
                    if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out Tag _))
                        return true;
                }
            }
            return false;*/
        }

        public bool CanManuallyAssignTo(MinionAssignablesProxy worker) 
        {
            
            GameObject go = worker.GetTargetGameObject();            
            bool flag1 =IsDoctorAvailable(go);            
            Traits traits = go.GetComponent<Traits>();
            bool flag2 = traits.HasTrait("GlowStick");
            //bool flag = false;
            /*MinionIdentity target = worker.target as MinionIdentity;
            if ((UnityEngine.Object)target != (UnityEngine.Object)null)
                flag = this.IsHealthBelowThreshold(target.gameObject);*/
            return flag1 && !flag2;
            
        } 

        public class States :
          GameStateMachine<States, StatesInstance, InjectionChamber>
        {
            public State unoperational;
            public OperationalStates operational;
            public BoolParameter hasSupplies;
            public BoolParameter hasPatient;
            public BoolParameter hasDoctor;
            public BoolParameter hasAssignee;

            public override void InitializeStates(out BaseState default_state)
            {
                this.serializable = SerializeType.Never;
                default_state = this.unoperational;
                this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (smi => smi.master.operational.IsOperational));
                this.operational.EventTransition(GameHashes.OperationalChanged, this.unoperational, (smi => !smi.master.operational.IsOperational)).DefaultState(this.operational.not_ready);
                this.operational.not_ready.ParamTransition<bool>(this.hasSupplies, this.operational.awaiting_assignee, (smi, p) => p);
                this.operational.awaiting_assignee.ParamTransition<bool>(this.hasAssignee, this.operational.ready.idle, ((smi, p) => p));
                this.operational.ready.DefaultState(this.operational.ready.idle).ParamTransition<bool>(this.hasSupplies, this.operational.not_ready, (smi, p) => !p);
                //this.operational.ready.awaitingassignee.ParamTransition<bool>((StateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.Parameter<bool>)this.hasAssignee, (GameStateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.State)this.operational.ready.idle, (StateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.Parameter<bool>.Callback)((smi, p) => p));
                //this.operational.ready.idle.Enter()
                //this.operational.ready.idle.ToggleChore(new Func<InjectionChamber.StatesInstance, Chore>(this.CreatePatientChore), this.operational.ready);

                this.operational.ready.ParamTransition<bool>(this.hasAssignee, this.operational, ((smi, p) => !p));

                this.operational.ready.ToggleRecurringChore(new Func<InjectionChamber.StatesInstance, Chore>(this.CreatePatientChore));
                this.operational.ready.idle.ParamTransition<bool>(this.hasPatient, this.operational.ready.has_patient, (smi, p) => p);
                this.operational.ready.has_patient.ParamTransition<bool>(this.hasPatient, this.operational.awaiting_assignee, (smi, p) => !p).DefaultState(this.operational.ready.has_patient.waiting).ToggleRecurringChore(new Func<InjectionChamber.StatesInstance, Chore>(this.CreateDoctorChore));
                this.operational.ready.has_patient.waiting.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.being_treated, (smi, p) => p);
                this.operational.ready.has_patient.being_treated.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.waiting, (smi, p) => !p).Enter(smi => smi.GetComponent<Operational>().SetActive(true)).Exit((smi => smi.GetComponent<Operational>().SetActive(false)));

                //this.unoperational.Enter((StateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.State.Callback)(smi => Debug.Log("Enter Unoperational")));
                //this.operational.not_ready.Enter((StateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.State.Callback)(smi => Debug.Log("Enter Not Ready")));
                //this.operational.awaiting_assignee.Enter((StateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.State.Callback)(smi => Debug.Log("Awaiting Assignee!!!!!")));
                //this.operational.ready.idle.Enter((StateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.State.Callback)(smi => Debug.Log("Enter Idle")));
                //this.operational.ready.has_patient.Enter((StateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.State.Callback)(smi => Debug.Log("Enter has patient")));
                //this.operational.ready.has_patient.waiting.Enter((StateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.State.Callback)(smi => Debug.Log("Enter has patient waiting")));

            }

            private Chore CreatePatientChore(StatesInstance smi)
            {
                //Debug.Log("Create Patient Chore");
                WorkChore<InjectionChamber> patientChore = new WorkChore<InjectionChamber>(Db.Get().ChoreTypes.GetDoctored, (IStateMachineTarget)smi.master, allow_in_red_alert: false, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.personalNeeds);
                patientChore.AddPrecondition(InjectionChamber.TreatmentAvailable, (object)smi.master);
                //patientChore.AddPrecondition(InjectionChamber.DoctorAvailable, (object)smi.master);
                //this.hasPatient.Set(true, smi);
                return (Chore)patientChore;
            }

            private Chore CreateDoctorChore(InjectionChamber.StatesInstance smi)
            {
                //Debug.Log("Create Doctor Chore");
                InjectionChamberDoctor component = smi.master.GetComponent<InjectionChamberDoctor>();
                return (Chore)new WorkChore<InjectionChamberDoctor>(Db.Get().ChoreTypes.Doctor, (IStateMachineTarget)component, allow_in_red_alert: false, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.high, ignore_building_assignment: true);
            }

            public class OperationalStates : State
            {
                public State not_ready;
                public State awaiting_assignee;
                public ReadyStates ready;
            }

            public class ReadyStates : State
            {
                public State idle;
                //public GameStateMachine<InjectionChamber.States, InjectionChamber.StatesInstance, InjectionChamber, object>.State awaitingassignee;
                public PatientStates has_patient;
            }

            public class PatientStates : State
            {
                public State waiting;
                public State being_treated;
            }
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, InjectionChamber, object>.GameInstance
        {
            public StatesInstance(InjectionChamber master)
              : base(master)
            {
            }
        }
    }
}