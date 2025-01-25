using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Klei.AI;
using KSerialization;

namespace Engies_Tuneup_Inactivity_Refund
{
    internal class EngiesTuneupRefundTracker : KMonoBehaviour, ISim4000ms
    {        
        [MyCmpReq]
        private readonly Effects effects;
        [MyCmpReq]
        private readonly Operational operational;        
        [Serialize]
        private float loanedTime = 0f;
        [Serialize]
        private float inactiveStartTimeTracker = 0f;
        [Serialize]
        public float powerTinkerStartTime = 0f;
        

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (inactiveStartTimeTracker == 0f)
                inactiveStartTimeTracker = GameClock.Instance.GetTime();
            //Debug.Log("Current Time  " + GameClock.Instance.GetTime());
            //Debug.Log("Loaned Time   " + loanedTime);
            //Debug.Log("Owed Time    " + owedTime);
            //Debug.Log("PowerTinkerStartTime  " +  powerTinkerStartTime);
            //Debug.Log("InactiveStartTime    " + operational.inactiveStartTime);
            //Debug.Log("Tracker " + inactiveStartTimeTracker);            
        }
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();            
            operational.Subscribe((int)GameHashes.ActiveChanged, OnActiveChanged);            
        }
        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            operational.Unsubscribe((int)GameHashes.ActiveChanged, OnActiveChanged);            
        }               
        private void OnActiveChanged(Object data) 
        {
            //Debug.Log("\n\n             OnActiveChanged");
            if (!operational.IsActive)
            {
                inactiveStartTimeTracker = operational.inactiveStartTime;
                return;
            }                            
            if (!effects.HasEffect("PowerTinker")) 
            {  
                return; 
            }                       
            // switched to being active while having buff

            var owedTime = operational.activeStartTime - inactiveStartTimeTracker;
            //Debug.Log(operational.activeStartTime +"   " + inactiveStartTimeTracker + "   " + loanedTime);
            //Debug.Log("Owed Time " + owedTime);
            //Debug.Log("powerTinkerStartTime " + powerTinkerStartTime);
            // don't count time before buff was applied
            if (powerTinkerStartTime > inactiveStartTimeTracker)
                owedTime -= powerTinkerStartTime - inactiveStartTimeTracker;
            //Debug.Log(owedTime);
            //Debug.Log("Loaned Time " + loanedTime);
            owedTime -= loanedTime;
            //Debug.Log("Final Owed Time " + owedTime);
            effects.Get("PowerTinker").timeRemaining += owedTime;            
            loanedTime = 0f;
        }

        public void Sim4000ms(float dt)
        {
            //Debug.Log("\n\n             Sim4000");
            //Debug.Log("Inactive Start  " + operational.inactiveStartTime);
            //Debug.Log("Tracker      " + inactiveStartTimeTracker);
            //Debug.Log("Active Start  " + operational.activeStartTime);
            //Debug.Log("Loaned Time  " + loanedTime);
            if (!effects.HasEffect("PowerTinker") || operational.IsActive)
                return;
            //has buff and is inactive
            var delta = GameClock.Instance.GetTime() - operational.inactiveStartTime;
            if (delta < dt)
                return;
            //sitting inactive for more than 4 seconds, loan some time to the buff and keep track            
            effects.Get("PowerTinker").timeRemaining += dt;
            loanedTime += dt;
            
        }        
    }
}
