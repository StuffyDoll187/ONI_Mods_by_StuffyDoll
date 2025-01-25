using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaos_Reigns
{
    internal class RainMaker : KMonoBehaviour, ISim200ms
    {
        [MyCmpReq]
        private readonly WorldContainer WorldContainer;
        private string WorldName;
        private static Dictionary<string, RainValues> AsteroidRainPairs = new Dictionary<string, RainValues> 
        {
            { "StartWorld", new RainValues(Config.Instance.RainElementStartWorld, Config.Instance.RainMassStartWorld / 1000f, Config.Instance.RainTempStartWorld + 273.15f) },
            { "NiobiumMoonlet" , new RainValues(SimHashes.Magma, Config.Instance.MagmaRainMass / 1000f, 2000f + 273.15f) },
            { "WaterMoonlet" , new RainValues(SimHashes.Water, Config.Instance.WaterRainMass / 1000f, 25f + 273.15f)},
            { "MarshyMoonlet" , new RainValues(SimHashes.DirtyWater, Config.Instance.MarshyRainMass / 1000f, 35f + 273.15f)},
            { "MooMoonlet" , new RainValues(SimHashes.Chlorine, Config.Instance.MooRainMass / 1000f, -40f + 273.15f)}            
        };
        private bool runSim = false;

        protected override void OnSpawn()
        {            
            base.OnSpawn();                        
            WorldName = this.WorldContainer.worldName?.Replace("expansion1::worlds/", "");
            if (WorldContainer.IsStartWorld)
                WorldName = "StartWorld";    
            if (WorldName != null)
                runSim = (AsteroidRainPairs.Keys.Contains(WorldName));
            
        }
        public void Sim200ms(float dt)
        {
            if (!runSim)
                return;
            if (!WorldContainer.isActiveAndEnabled)
                return;  
            if (WorldContainer.IsStartWorld && Config.Instance.RainElementChoiceStartWorld == Config.RainElementOptions.None)
                return;
            int x = (int) UnityEngine.Random.Range(WorldContainer.minimumBounds.x, WorldContainer.maximumBounds.x);
            float mass = AsteroidRainPairs[WorldName].Mass;
            float temp = AsteroidRainPairs[WorldName].Temp;
            if (mass > 0f && temp > 0)
            FallingWater.instance.AddParticle(Grid.XYToCell(x, (int)WorldContainer.maximumBounds.y), AsteroidRainPairs[WorldName].ElementIdx, mass, temp, 0, 0);            
        }

        private struct RainValues
        {
            public RainValues(SimHashes simHash, float mass, float temp)
            {
                ElementIdx = ElementLoader.GetElementIndex(simHash);
                Mass = mass;
                Temp = temp;
            }
            public ushort ElementIdx;
            public float Mass;
            public float Temp;
        }
    }
}
