using HarmonyLib;
using Klei.AI;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Bionic_Overclock
{
    
    public class Overclocker : KMonoBehaviour, IMultiSliderControl
    {
        [MyCmpGet]
        private MinionResume MinionResume;
        [MySmiGet]
        private BionicBatteryMonitor.Instance batteryMonitor;
        [MySmiGet]
        private BionicOilMonitor.Instance oilMonitor;
        
        private BionicBatteryMonitor.WattageModifier overclockWattageModifier = new BionicBatteryMonitor.WattageModifier("Overclock_Wattage", "", 0, 0);

        [HarmonyPatch(typeof(BionicMinionConfig), nameof(BionicMinionConfig.CreatePrefab))]        
        internal class BionicMinionConfig_CreatePrefab_Patch
        {
            private static void Postfix(GameObject __result)
            {
                __result.AddComponent<Overclocker>();
                
            }
        }
        string IMultiSliderControl.SidescreenTitleKey => "";
        public ISliderControl[] sliderControls;
        ISliderControl[] IMultiSliderControl.sliderControls => sliderControls;
        bool IMultiSliderControl.SidescreenEnabled() => MinionResume.HasPerk("Overclocking");
        public Overclocker()
        {
            sliderControls = new ISliderControl[]
            {
                new OverclockerController(this)
            };
        }

        public class OverclockerController : ISliderControl
        {
            public Overclocker target;
            public OverclockerController(Overclocker t) => target = t;

            public string SliderTitleKey => "STRINGS.SLIDERS.BIONICS.OVERCLOCK.TITLE";
            [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
            internal class MoreTempshiftPlatesGeneratedBuildingsLoadGeneratedBuildings
            {
                public static void Prefix()
                {
                    Strings.Add("STRINGS.SLIDERS.BIONICS.OVERCLOCK.TITLE", "Overclock");
                }
            }
            public string SliderUnits => "%";
            public int SliderDecimalPlaces(int index) => 0;
            public float GetSliderMin(int index) => -100;
            public float GetSliderMax(int index) => 100;
            public float GetSliderValue(int index) => target.overclockFactor * 100;
            public void SetSliderValue(float value, int index)
            {
                target.overclockFactor = value / 100;                
            }
            public string GetSliderTooltipKey(int index) => "";
            public string GetSliderTooltip(int index) => "Percent change to Booster Attribute Potency and Bionic Wattage/Lubrication Requirements";
        }

        

        protected override void OnSpawn()
        {
            base.OnSpawn();            
            InitOverclockModifiers();
            if (gameObject.HasTag(GameTags.Dead))
                return;
            RefreshOverclockedModifiers();
            batteryMonitor.AddOrUpdateModifier(overclockWattageModifier);

        }
        [Serialize]
        private float overclockFactorReal;
        public float overclockFactor
        {
            get { return MinionResume.HasPerk("Overclocking") ? overclockFactorReal : 0; }
            set
            {              
                overclockFactorReal = value;
                RefreshOverclockedModifiers();
            }
        }
        public Dictionary<string, AttributeModifier> overclockedModifiers = new Dictionary<string, AttributeModifier>();

        public static AttributeModifier[] attributeModifiersTemplate = new AttributeModifier[] {
            new AttributeModifier(Db.Get().Attributes.Construction.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Digging.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Machinery.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Athletics.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Learning.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Cooking.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Caring.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Strength.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Art.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Botanist.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.Ranching.Id, 0, "Overclock"),
            new AttributeModifier(Db.Get().Attributes.SpaceNavigation.Id, 0, "Overclock"),

            new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, 0, "Overclock")      //base rate (-1f/30f)
        };

        
        public void InitOverclockModifiers()
        {                        
            if (overclockedModifiers.Count == 0)
            {
                foreach (AttributeModifier modifier in attributeModifiersTemplate)
                {
                    overclockedModifiers.Add(modifier.AttributeId, modifier.Clone());                    
                }
            }
        }
        public void RefreshOverclockedModifiers()
        {            
            Klei.AI.Attributes attributes = MinionResume.GetAttributes();
            foreach (KeyValuePair<string, float> pair in boosterAttributeTotals)
            {
                if (overclockedModifiers.TryGetValue(pair.Key, out AttributeModifier _))
                {
                    overclockedModifiers[pair.Key].SetValue(pair.Value * overclockFactor);
                    overclockedModifiers[pair.Key].Description = overclockedModifiers[pair.Key].Value >= 0 ? "Overclock" : "Underclock";
                    attributes.Remove(overclockedModifiers[pair.Key]);                    
                    if (pair.Value != 0)
                        attributes.Add(overclockedModifiers[pair.Key]);
                }
            }
            if (overclockedModifiers.TryGetValue(Db.Get().Amounts.BionicOil.deltaAttribute.Id, out AttributeModifier _))
            {
                overclockedModifiers[Db.Get().Amounts.BionicOil.deltaAttribute.Id].SetValue(overclockFactor * -1f / 30f);   //base rate (-1f/30f)
                overclockedModifiers[Db.Get().Amounts.BionicOil.deltaAttribute.Id].Description = overclockedModifiers[Db.Get().Amounts.BionicOil.deltaAttribute.Id].Value < 0 ? "Overclock" : "Underclock";
                if (oilMonitor != null && oilMonitor.IsOnline)
                {
                    attributes.Remove(overclockedModifiers[Db.Get().Amounts.BionicOil.deltaAttribute.Id]);
                    if (overclockedModifiers[Db.Get().Amounts.BionicOil.deltaAttribute.Id].Value != 0)
                        attributes.Add(overclockedModifiers[Db.Get().Amounts.BionicOil.deltaAttribute.Id]);
                }

            }            
            if (batteryMonitor != null)//batteryMonitor is null when this is called from OnSpawn but then isn't null immediately after in the next line of code? ok....
            {
                overclockWattageModifier.value = overclockFactor * (batteryMonitor.GetBaseWattage() + BionicBatteryMonitor.GetDifficultyModifier().value);
                overclockWattageModifier.name = (overclockFactor >= 0) ? "Overclock: +" + overclockWattageModifier.value + " W" : "Underclock: -" + -overclockWattageModifier.value + " W";
                batteryMonitor.AddOrUpdateModifier(overclockWattageModifier);
            }              
        }


        /// <summary>
        /// Intercept calls to Apply/Remove booster attribute modifiers to keep track of the total bonus for each attribute.
        /// </summary>
        public Dictionary<string, float> boosterAttributeTotals = new Dictionary<string, float>();

        [HarmonyPatch(typeof(BionicUpgrade_SkilledWorker.Instance), nameof(BionicUpgrade_SkilledWorker.Instance.ApplyModifiers))]
        public class BionicUpgrade_SkilledWorker_ApplyModifiers
        {
            public static void Postfix(BionicUpgrade_SkilledWorker.Instance __instance)
            {
                if (!__instance.gameObject.TryGetComponent(out Overclocker overclocker))
                    return;
                foreach (AttributeModifier modifier in ((BionicUpgrade_SkilledWorker.Def)__instance.def).modifiers)
                {
                    if (overclocker.boosterAttributeTotals.TryGetValue(modifier.AttributeId, out float _))
                    {
                        overclocker.boosterAttributeTotals[modifier.AttributeId] += modifier.Value;
                    }
                    else
                    {
                        overclocker.boosterAttributeTotals.Add(modifier.AttributeId, modifier.Value);
                    }
                }
                overclocker.RefreshOverclockedModifiers();
            }

        }
        [HarmonyPatch(typeof(BionicUpgrade_SkilledWorker.Instance), nameof(BionicUpgrade_SkilledWorker.Instance.RemoveModifiers))]
        public class BionicUpgrade_SkilledWorker_RemoveModifiers
        {
            public static void Postfix(BionicUpgrade_SkilledWorker.Instance __instance)
            {
                if (!__instance.gameObject.TryGetComponent(out Overclocker overclocker))
                    return;
                foreach (AttributeModifier modifier in ((BionicUpgrade_SkilledWorker.Def)__instance.def).modifiers)
                {
                    Debug.Assert(overclocker.boosterAttributeTotals.TryGetValue(modifier.AttributeId, out float _), "Trying to remove " + modifier.AttributeId + " modifier which was never applied");
                    overclocker.boosterAttributeTotals[modifier.AttributeId] -= modifier.Value;
                }
                overclocker.RefreshOverclockedModifiers();
            }

        }







        [HarmonyPatch(typeof(BionicOilMonitor.Instance), nameof(BionicOilMonitor.Instance.SetBaseDeltaModifierActiveState))]
        public class BionicOilSMI_SetOilModifier_Patch
        {
            public static void Postfix(BionicOilMonitor.Instance __instance, bool isActive)
            {                
                if (__instance.gameObject.TryGetComponent(out Overclocker overclocker))
                {
                    if (!overclocker.MinionResume.HasPerk("Overclocking"))
                        return;
                    overclocker.InitOverclockModifiers();
                    Klei.AI.Attributes attributes = overclocker.MinionResume.GetAttributes();
                    if (isActive)                    
                        attributes.Add(overclocker.overclockedModifiers[Db.Get().Amounts.BionicOil.deltaAttribute.Id]);
                    else
                        attributes.Remove(overclocker.overclockedModifiers[Db.Get().Amounts.BionicOil.deltaAttribute.Id]);

                }
            }
        }


    }
}
