using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSerialization;
using UnityEngine;

namespace Chaos_Reigns
{
    [SerializationConfig(MemberSerialization.OptIn)]
    internal class TwitchMagmaRain : StateMachineComponent<TwitchMagmaRain.StatesInstance>, ISaveLoadable
    {
        [MyCmpReq]
        private readonly WorldContainer WorldContainer;
        [Serialize]
        private float timeRemaining = 0;
        public float TimeRemaining { get => timeRemaining; set => timeRemaining = value; }
        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (WorldContainer.IsStartWorld)
                smi.StartSM();
        }        
        public class States : GameStateMachine<States,StatesInstance,TwitchMagmaRain, object>
        {            
            public State raining;
            public State clearSkies;                                                
            public override void InitializeStates(out BaseState default_state)
            {                
                default_state = clearSkies;                                                
                clearSkies.UpdateTransition(raining, new Func<StatesInstance, float, bool>(ShouldBeRaining));
                raining.UpdateTransition(clearSkies, new Func<StatesInstance, float, bool>(RainMagma));                             
                //raining.Update((smi, dt) => { Debug.Log("Raining "+smi.master.TimeRemaining); });
                //clearSkies.Update((smi, dt) => { Debug.Log("ClearSkies "+smi.master.TimeRemaining); });
            }            
            private bool ShouldBeRaining(StatesInstance smi, float dt) 
            {                
                return smi.master.TimeRemaining > 0;
            }
            private bool RainMagma(StatesInstance smi, float dt)
            {                
                smi.master.TimeRemaining -= dt;
                int x = (int)UnityEngine.Random.Range(smi.master.WorldContainer.minimumBounds.x, smi.master.WorldContainer.maximumBounds.x);
                FallingWater.instance.AddParticle(Grid.XYToCell(x, (int)smi.master.WorldContainer.maximumBounds.y), ElementLoader.GetElementIndex(SimHashes.Magma), Config.Instance.TwitchMagmaRainMass, 2273.15f, 0, 0);
                return smi.master.TimeRemaining <= 0;
            }
        }
        public class StatesInstance : GameStateMachine<States, StatesInstance, TwitchMagmaRain, object>.GameInstance
        {
            public StatesInstance(TwitchMagmaRain master) : base(master) { }                        
        }
    }
}
