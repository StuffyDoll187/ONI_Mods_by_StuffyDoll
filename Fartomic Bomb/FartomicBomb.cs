using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace Fartomic_Bomb
{
    
 
[SkipSaveFileSerialization]
    public class FartomicBomb : StateMachineComponent<FartomicBomb.StatesInstance>
    {
        [MyCmpGet]
        private SaveOnDuplicant Save;
        private Notification notification;
        private static List<SimHashes> EmitElements = new List<SimHashes>();
        /*{
            SimHashes.Hydrogen,
            //SimHashes.ContaminatedOxygen,
            //SimHashes.Oxygen,
            SimHashes.Methane,
            SimHashes.SourGas,
            //SimHashes.CarbonDioxide,
            SimHashes.ChlorineGas,
            SimHashes.Fallout,
            SimHashes.EthanolGas,
            //SimHashes.Steam,
            //SimHashes.PhosphorusGas,
            //SimHashes.SulfurGas,
            SimHashes.MercuryGas,
            //SimHashes.SuperCoolantGas,
            //SimHashes.SaltGas,
            SimHashes.LeadGas,
            //SimHashes.RockGas,
            SimHashes.AluminumGas,
            SimHashes.CopperGas,
            SimHashes.IronGas,
            SimHashes.GoldGas,
            SimHashes.CobaltGas,
            //SimHashes.SteelGas,
            //SimHashes.NiobiumGas,
            //SimHashes.CarbonGas,
            SimHashes.TungstenGas
        };*/
        private static List<SimHashes> NormalGases = new List<SimHashes>
        {
            SimHashes.Hydrogen,
            //SimHashes.ContaminatedOxygen,
            //SimHashes.Oxygen,
            SimHashes.Methane,
            SimHashes.SourGas,
            //SimHashes.CarbonDioxide,
            SimHashes.ChlorineGas,
            //SimHashes.Fallout,
            SimHashes.EthanolGas,
        };
        private static List<SimHashes> MetallicGases = new List<SimHashes>
        {
            //SimHashes.MercuryGas,
            //SimHashes.SuperCoolantGas,
            //SimHashes.SaltGas,
            SimHashes.LeadGas,
            //SimHashes.RockGas,
            SimHashes.AluminumGas,
            SimHashes.CopperGas,
            SimHashes.IronGas,
            SimHashes.GoldGas,
            //SimHashes.CobaltGas,
            //SimHashes.SteelGas,
            //SimHashes.NiobiumGas,
            //SimHashes.CarbonGas,
            SimHashes.TungstenGas
        };
        private static List<SimHashes> SpacedOutNormalGases = new List<SimHashes>
        {
            SimHashes.Fallout
        };
        private static List<SimHashes> SpacedOutMetallicGases = new List<SimHashes>
        { 
            SimHashes.CobaltGas 
        };
        /*private static List<SimHashes> FrostyPlanetPackMetallicGases = new List<SimHashes>
        {
            SimHashes.MercuryGas
        }; */      
        internal static void InitEmitElementsList(bool SpacedOut)
        {            
            foreach (var simhash in NormalGases) EmitElements.Add(simhash);

            if (SpacedOut)
                foreach (var simhash in SpacedOutNormalGases) EmitElements.Add(simhash);

            if (Config.config.AllowMetallicGases)
                foreach (var simhash in MetallicGases) EmitElements.Add(simhash);

            if (SpacedOut && Config.config.AllowMetallicGases)
                foreach (var simhash in SpacedOutMetallicGases) EmitElements.Add(simhash);

            
            /*foreach (var simhash in EmitElements)
            {
                Element element = ElementLoader.FindElementByHash(simhash);     //this crashes on load because FindElementByHash is not static?
                EmitElementsStrings.Add(element.name.ToString());
                float lowtemp = element.lowTemp;
                float temp = 310.15f;
                if (lowtemp > 310.15) temp = lowtemp + 10f;
                EmitElementsTemps.Add(temp);
            }*/

            //foreach (var simhash in EmitElements) Debug.Log(simhash);
        }

       
        //internal static string Description = "This Duplicant emits " + Config.config.EmitMassKg.ToString() + "Kg of a random gas with an average frequency of " + Config.config.FrequencyAverageInCycles +" cycles";
        private static readonly HashedString[] WorkLoopAnims = new HashedString[3]
        {
            (HashedString) "working_pre",
            (HashedString) "working_loop",
            (HashedString) "working_pst"
        };

        protected override void OnSpawn()
        { 
            
            base.OnSpawn();
            if (this.Save.TimeRemaining == -1)
            this.Save.TimeRemaining = Mathf.Clamp(Util.GaussianRandom(Config.config.FrequencyAverageInCycles, Config.config.FrequencyStdDeviation), Config.config.FrequencyMin, Config.config.FrequencyMax) * 600;            
            //Debug.Log(this.Save.TimeRemaining);
            this.smi.StartSM();

        }

        private void Emit(object data)
        {
            GameObject gameObject = (GameObject)data;           
            SimHashes simhash = EmitElements[this.Save.RandomElementIdx];
            Element element = ElementLoader.FindElementByHash(simhash);
            float lowtemp = element.lowTemp;            
            float temp = 310.15f;
            if (lowtemp > 310.15) temp = lowtemp + 30f;
            if (Config.config.OverrideOutputTemperature) temp = Config.config.TemperatureOverrideInCelsius + 273.15f ;

            Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
            if ((UnityEngine.Object)equippable != (UnityEngine.Object)null)
            {
                equippable.GetComponent<Storage>().AddGasChunk(simhash, Config.config.EmitMassKg, temp, byte.MaxValue, 0, false);
            }
            else
            {
                Components.Cmps<MinionIdentity> minionIdentities = Components.LiveMinionIdentities;
                Vector2 position1 = (Vector2)gameObject.transform.GetPosition();
                for (int idx = 0; idx < minionIdentities.Count; ++idx)
                {
                    MinionIdentity minionIdentity = minionIdentities[idx];
                    if ((UnityEngine.Object)minionIdentity.gameObject != (UnityEngine.Object)gameObject.gameObject)
                    {
                        Vector2 position2 = (Vector2)minionIdentity.transform.GetPosition();
                        if ((double)Vector2.SqrMagnitude(position1 - position2) <= 2.25)
                        {
                            minionIdentity.Trigger(508119890, (object)Strings.Get("STRINGS.DUPLICANTS.DISEASES.PUTRIDODOUR.CRINGE_EFFECT").String);
                            minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
                        }
                    }
                }
                SimMessages.AddRemoveSubstance(Grid.PosToCell(gameObject.transform.GetPosition()), simhash, CellEventLogger.Instance.ElementConsumerSimUpdate, Config.config.EmitMassKg, temp, byte.MaxValue, 0);
                KBatchedAnimController effect = FXHelpers.CreateEffect("odor_fx_kanim", gameObject.transform.GetPosition(), gameObject.transform, true);
                effect.Play(FartomicBomb.WorkLoopAnims);
                effect.destroyOnAnimComplete = true;
            }
            GameObject go = gameObject;
            bool objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(go);
            Vector3 vector3 = go.GetComponent<Transform>().GetPosition(); 
            /*with
            {
                z = 0.0f         //look into this
            };*/
            float volume = 1f;
            if (objectIsSelectedAndVisible)
            {
                vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
                volume = SoundEvent.GetVolume(objectIsSelectedAndVisible);
            }
            else
                vector3.z = 0.0f;
            KFMOD.PlayOneShot(GlobalAssets.GetSound("Dupe_Flatulence"), vector3, volume);
        }

        private void TryCreateNotification()
        {
            WorldContainer myWorld = this.smi.master.GetMyWorld();
            if (!((UnityEngine.Object)myWorld != (UnityEngine.Object)null))
                return;
            //Debug.Log(this.Save.RandomElementIdx + "  " + EmitElements.Count);
            SimHashes simhash = EmitElements[this.Save.RandomElementIdx];
            Element element = ElementLoader.FindElementByHash(simhash);
            string str = element.name.ToString();
            this.notification = new Notification("Fartomic Bomb Imminent!!\n" + str, NotificationType.Bad, new Func<List<Notification>, object, string>(ImminentTooltip));
            this.notification.tooltipData = (object)this.smi.master.gameObject.GetProperName();         
            this.gameObject.AddOrGet<Notifier>().Add(this.notification);
        }
        private static string ImminentTooltip(List<Notification> notifications, object data)
        {
            string str = "";
            foreach (Notification notification in notifications)
            {
                if (notification.tooltipData != null)
                    str = str + "• " + (string)notification.tooltipData + " \n";
            }
            return str;            
        }

        private void TryClearNotification()
        {
            if (this.notification == null)
                return;
            this.gameObject.AddOrGet<Notifier>().Remove(this.notification);
        }

        public class StatesInstance :
          GameStateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.GameInstance
        {
            
            public StatesInstance(FartomicBomb master)
              : base(master)
            {
                
            }
            
        }

        public class States : GameStateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb>
        {
            public GameStateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State idle;
            public GameStateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State notify;
            public GameStateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State emit;

            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = (StateMachine.BaseState)this.idle;
                this.root.TagTransition(GameTags.Dead, (GameStateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State)null);

                //this.idle.Enter((StateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State.Callback)(smi => Debug.Log(smi.master.Save.TimeRemaining)));

                this.idle.UpdateTransition(this.notify, (Func<StatesInstance, float, bool>)((smi, dt) => this.IsReady(smi, dt)), UpdateRate.SIM_4000ms);

                this.idle.Exit((StateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State.Callback)(smi =>
                {
                    if (smi.master.Save.RandomElementIdx == -1 || smi.master.Save.RandomElementIdx > FartomicBomb.EmitElements.Count)
                        this.GenerateRandomElementIndex(smi);
                }));
                
                this.notify.Enter((StateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State.Callback)(smi => smi.master.TryCreateNotification())).ScheduleGoTo(Mathf.Max(1f, Config.config.NotificationToEmissionDelay), this.emit);

                //this.emit.Enter("Fart", (StateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State.Callback)(smi => smi.master.Emit((object)smi.master.gameObject))).ToggleExpression(Db.Get().Expressions.Relief).ScheduleGoTo(10f, (StateMachine.BaseState)this.idle);

                this.emit.Enter((StateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State.Callback)(smi =>
                {
                    smi.master.Emit((object)smi.master.gameObject);
                    this.ResetTimeRemaining(smi);
                    this.ClearRandomElementIndex(smi);                    
                    if (Config.config.RedAlertOnEmission) 
                    { 
                        smi.master.GetMyWorld().AlertManager.ToggleRedAlert(true);                        
                    }                    
                })).ToggleExpression(Db.Get().Expressions.Relief).ScheduleGoTo(12f , (StateMachine.BaseState)this.idle);                

                this.emit.Exit((StateMachine<FartomicBomb.States, FartomicBomb.StatesInstance, FartomicBomb, object>.State.Callback)(smi =>
                {
                    smi.master.TryClearNotification();
                    if (Config.config.RedAlertOnEmission)
                        smi.master.GetMyWorld().AlertManager.ToggleRedAlert(false);
                }));   
                
              
            }
            private bool IsReady(StatesInstance smi, float dt)
            {                
                smi.master.Save.TimeRemaining -= dt;
                //Debug.Log(smi.master.Save.TimeRemaining);
                return smi.master.Save.TimeRemaining < 0;
            }
            private void ResetTimeRemaining(StatesInstance smi)
            {                
                smi.master.Save.TimeRemaining = Mathf.Clamp(Util.GaussianRandom(Config.config.FrequencyAverageInCycles, Config.config.FrequencyStdDeviation), Config.config.FrequencyMin, Config.config.FrequencyMax) * 600;                               
            }      
            private void GenerateRandomElementIndex(StatesInstance smi)
            {
                int idx = UnityEngine.Random.Range((int)0, (int)EmitElements.Count);
                SimHashes simhash = EmitElements[idx];
                Element element = ElementLoader.FindElementByHash(simhash);                
                float lowtemp = element.lowTemp;               
                int rerolls = Config.config.RerollsToReduceOddsOfHotGases;                
                //if (rerolls > 0 && lowtemp > 310.15) Debug.Log("Rerolled from " + simhash);
                while (lowtemp > 310.15 && rerolls > 0)
                {
                    int idx1 = UnityEngine.Random.Range((int)0, (int)EmitElements.Count);
                    SimHashes simhash1 = EmitElements[idx1];
                    Element element1 = ElementLoader.FindElementByHash(simhash1);
                    lowtemp = element1.lowTemp;
                    //Debug.Log(" to " + simhash1);
                    idx = idx1;
                    rerolls--;                    
                    
                    //Debug.Log("Rerolls remaining " + rerolls);
                }
                
                smi.master.Save.RandomElementIdx = idx;
            }
            private void ClearRandomElementIndex(StatesInstance smi)
            {
                smi.master.Save.RandomElementIdx = -1;
            }
        }
    }
}
