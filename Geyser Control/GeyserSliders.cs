using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Geyser_Control.STRINGS.SLIDERS;


namespace Geyser_Control
{
    [SerializationConfig(MemberSerialization.OptIn)]
    internal class GeyserSliders : KMonoBehaviour, IMultiSliderControl , ISidescreenButtonControl, ISim1000ms
    {
        [MyCmpReq]
        public readonly Geyser geyser;        
        [MyCmpReq]
        public readonly Studyable studyable;
        [MyCmpReq]
        public readonly KSelectable kSelectable;
        //[MyCmpGet]
        //UncapPressureCheckbox uncapPressure;
        [Serialize]
        public GeyserConfigurator.GeyserInstanceConfiguration newconfig;
        [Serialize]
        public Geyser.GeyserModification modification = new Geyser.GeyserModification() { originID = "GeyserControl"};//pressure
        [Serialize]
        public Geyser.GeyserModification tempModification = new Geyser.GeyserModification() { originID = "GeyserControl" };

        [Serialize]
        public bool slidersSet;
        [Serialize]
        public bool runSim;
        [Serialize]
        public bool enableSidescreen;
        protected ISliderControl[] sliderControls;
        protected override void OnSpawn()
        {
            base.OnSpawn();
            
            if (modification.originID == null)//pressure
                modification.originID = "GeyserControl";


            if (studyable.Studied && slidersSet)
            {
                geyser.smi.StopSM("Resync SM with new time values");
                geyser.configuration.scaledRate = newconfig.scaledRate;
                geyser.configuration.scaledIterationLength = newconfig.scaledIterationLength;
                geyser.configuration.scaledIterationPercent = newconfig.scaledIterationPercent;
                geyser.configuration.scaledYearLength = newconfig.scaledYearLength;
                geyser.configuration.scaledYearPercent = newconfig.scaledYearPercent;
                geyser.ApplyConfigurationEmissionValues(geyser.configuration);
                geyser.smi.StartSM();

                /*if (Config.Instance.maxPressureControls && !Config.Instance.uncapPressureCheckbox)
                {                    
                    if (modification.maxPressureModifier > -1f + sliderControls[5].GetSliderMax(0) / geyser.configuration.geyserType.maxPressure)
                        modification.maxPressureModifier = 0;
                }
                if (!Config.Instance.maxPressureControls && Config.Instance.uncapPressureCheckbox)
                {
                    modification.maxPressureModifier = uncapPressure ? -1 + 1e30f / geyser.configuration.geyserType.maxPressure: 0;//float.MaxValue / 1000000f : 0;                    
                }                    
                if (Config.Instance.uncapPressureCheckbox || Config.Instance.maxPressureControls)*/

                /*if (!TryGetComponent<UncapPressureCheckbox>(out _))
                    modification.maxPressureModifier = 0;

                if (Config.Instance.uncapPressureCheckbox)
                    geyser.AddModification(modification); */               
            }

            if (Config.Instance.tempControl)
                geyser.AddModification(tempModification);

            if (TryGetComponent<UncapPressureCheckbox>(out var cmp) && cmp.uncappedPressure)
                geyser.AddModification(modification);
        }
        public GeyserSliders()
        {         
            if (Config.Instance.tempControl)
            {
                this.sliderControls = new ISliderControl[6]
                {
                    new MassPerCycleController(this),
                    new IterationLengthController(this),
                    new IterationPercentController(this),
                    new YearLengthController(this),
                    new YearPercentController(this),
                    new TempController(this)
                };
                
            }
            else
            {
                this.sliderControls = new ISliderControl[5]
                {
                    new MassPerCycleController(this),
                    new IterationLengthController(this),
                    new IterationPercentController(this),
                    new YearLengthController(this),
                    new YearPercentController(this)
                };
            }
                
            
        }

        string IMultiSliderControl.SidescreenTitleKey
        {
            get => "STRINGS.SLIDERS.GEYSERSLIDERS.NAME";
        }

        ISliderControl[] IMultiSliderControl.sliderControls => this.sliderControls;

        bool IMultiSliderControl.SidescreenEnabled() => enableSidescreen;//studyable.Studied;

        protected class MassPerCycleController : ISliderControl
        {
            public GeyserSliders target;
            public MassPerCycleController(GeyserSliders t) => this.target = t;

            public string SliderTitleKey => "STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.NAME";
            public string SliderUnits => STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.UNITS;
            public int SliderDecimalPlaces(int index) => 3;
            public float GetSliderMin(int index) => Config.Instance.breakVanilla ? target.geyser.configuration.geyserType.minRatePerCycle / Config.Instance.breakFactor : target.geyser.configuration.geyserType.minRatePerCycle;
            public float GetSliderMax(int index) => Config.Instance.breakVanilla ? Config.Instance.breakFactor * target.geyser.configuration.geyserType.maxRatePerCycle : target.geyser.configuration.geyserType.maxRatePerCycle;
            public float GetSliderValue(int index) => target.geyser.configuration.scaledRate;
            public void SetSliderValue(float value, int index)
            {
                target.newconfig.scaledRate = value;
                if (target.geyser.configuration.scaledRate != value)
                {
                    target.geyser.configuration.scaledRate = value;                    
                    target.geyser.ApplyConfigurationEmissionValues(target.geyser.configuration);                    
                }
                target.slidersSet = true;                
            }
            public string GetSliderTooltipKey(int index) => "STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.TOOLTIP";
            public string GetSliderTooltip(int index) => STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.TOOLTIP;                                                                       
        }

        protected class IterationLengthController : ISliderControl
        {
            protected GeyserSliders target;
            public IterationLengthController(GeyserSliders t) => this.target = t;

            public string SliderTitleKey => "STRINGS.SLIDERS.ITERATIONLENGTHCONTROLLER.NAME";
            public string SliderUnits => STRINGS.SLIDERS.ITERATIONLENGTHCONTROLLER.UNITS;
            public int SliderDecimalPlaces(int index) => 4;
            public float GetSliderMin(int index) => Config.Instance.breakVanilla ? 0.5f * target.geyser.configuration.geyserType.minIterationLength : target.geyser.configuration.geyserType.minIterationLength;
            public float GetSliderMax(int index) => Config.Instance.breakVanilla ? 2 * target.geyser.configuration.geyserType.maxIterationLength : target.geyser.configuration.geyserType.maxIterationLength;
            public float GetSliderValue(int index) => target.geyser.configuration.scaledIterationLength;
            public void SetSliderValue(float value, int index)
            {
                target.newconfig.scaledIterationLength = value;
                if (target.geyser.configuration.scaledIterationLength != value)
                {
                    target.geyser.configuration.scaledIterationLength = value;                    
                    target.runSim = true;
                }
                target.slidersSet = true;                                              
            }
            public string GetSliderTooltipKey(int index) => "STRINGS.SLIDERS.ITERATIONLENGTHCONTROLLER.TOOLTIP";
            public string GetSliderTooltip(int index) => STRINGS.SLIDERS.ITERATIONLENGTHCONTROLLER.TOOLTIP;                                                           
        }

        protected class IterationPercentController : ISliderControl
        {
            protected GeyserSliders target;
            public IterationPercentController(GeyserSliders t) => this.target = t;

            public string SliderTitleKey => "STRINGS.SLIDERS.ITERATIONPERCENTCONTROLLER.NAME";
            public string SliderUnits => STRINGS.SLIDERS.ITERATIONPERCENTCONTROLLER.UNITS;
            public int SliderDecimalPlaces(int index) => 5;
            public float GetSliderMin(int index) => Config.Instance.breakVanilla ? 0.5f : 100f * target.geyser.configuration.geyserType.minIterationPercent;
            public float GetSliderMax(int index) => Config.Instance.breakVanilla ? 100f : 100f * target.geyser.configuration.geyserType.maxIterationPercent;
            public float GetSliderValue(int index) => 100f * target.geyser.configuration.scaledIterationPercent;
            public void SetSliderValue(float value, int index)
            {
                value /= 100;
                target.newconfig.scaledIterationPercent = value;
                if (target.geyser.configuration.scaledIterationPercent != value)
                {
                    target.geyser.configuration.scaledIterationPercent = value;                    
                    target.geyser.ApplyConfigurationEmissionValues(target.geyser.configuration);
                    target.runSim = true;
                }
                target.slidersSet = true;                                
            }
            public string GetSliderTooltipKey(int index) => "STRINGS.SLIDERS.ITERATIONPERCENTCONTROLLER.TOOLTIP";
            public string GetSliderTooltip(int index) => STRINGS.SLIDERS.ITERATIONPERCENTCONTROLLER.TOOLTIP;                                                           
        }

        protected class YearLengthController : ISliderControl
        {
            protected GeyserSliders target;
            public YearLengthController(GeyserSliders t) => this.target = t;

            public string SliderTitleKey => "STRINGS.SLIDERS.YEARLENGTHCONTROLLER.NAME";
            public string SliderUnits => STRINGS.SLIDERS.YEARLENGTHCONTROLLER.UNITS;
            public int SliderDecimalPlaces(int index) => 2;
            public float GetSliderMin(int index) => Config.Instance.breakVanilla ? Math.Min(6000f, target.geyser.configuration.geyserType.minYearLength) : target.geyser.configuration.geyserType.minYearLength;
            public float GetSliderMax(int index) => Config.Instance.breakVanilla ? Math.Min( 300000f , 3 * target.geyser.configuration.geyserType.maxYearLength) : target.geyser.configuration.geyserType.maxYearLength;
            public float GetSliderValue(int index) => target.geyser.configuration.scaledYearLength;
            public void SetSliderValue(float value, int index)
            {
                target.newconfig.scaledYearLength = value;
                if (target.geyser.configuration.scaledYearLength != value)
                {
                    target.geyser.configuration.scaledYearLength = value;                    
                    target.runSim = true;
                }
                target.slidersSet = true;                            
            }
            public string GetSliderTooltipKey(int index) => "STRINGS.SLIDERS.YEARLENGTHCONTROLLER.TOOLTIP";
            public string GetSliderTooltip(int index) => STRINGS.SLIDERS.YEARLENGTHCONTROLLER.TOOLTIP;                                                           
        }

        protected class YearPercentController : ISliderControl
        {
            protected GeyserSliders target;
            public YearPercentController(GeyserSliders t) => this.target = t;

            public string SliderTitleKey => "STRINGS.SLIDERS.YEARPERCENTCONTROLLER.NAME";
            public string SliderUnits => STRINGS.SLIDERS.YEARPERCENTCONTROLLER.UNITS;
            public int SliderDecimalPlaces(int index) => 5;
            public float GetSliderMin(int index) => Config.Instance.breakVanilla ? 1f : 100f * target.geyser.configuration.geyserType.minYearPercent;
            public float GetSliderMax(int index) => Config.Instance.breakVanilla ? 100f : 100f * target.geyser.configuration.geyserType.maxYearPercent;
            public float GetSliderValue(int index) => 100f * target.geyser.configuration.scaledYearPercent;
            public void SetSliderValue(float value, int index)
            {
                value /= 100;
                target.newconfig.scaledYearPercent = value;
                if (target.geyser.configuration.scaledYearPercent != value)
                {
                    target.geyser.configuration.scaledYearPercent = value;                    
                    target.runSim = true;
                }                
                target.slidersSet = true;                             
            }
            public string GetSliderTooltipKey(int index) => "STRINGS.SLIDERS.YEARPERCENTCONTROLLER.TOOLTIP";
            public string GetSliderTooltip(int index) => STRINGS.SLIDERS.YEARPERCENTCONTROLLER.TOOLTIP;                                                           
        }
        /*protected class MaxPressureController : ISliderControl
        {
            protected GeyserSliders target;            
            public MaxPressureController(GeyserSliders target)
            {
                this.target = target;                
            }                                   
            public string SliderTitleKey => "STRINGS.SLIDERS.MAXPRESSURECONTROLS.NAME";

            public string SliderUnits => STRINGS.SLIDERS.MAXPRESSURECONTROLS.UNITS;

            public int SliderDecimalPlaces(int index) => 0;

            public float GetSliderMin(int index) => target.geyser.configuration.geyserType.maxPressure;

            public float GetSliderMax(int index) => 10 * target.geyser.configuration.geyserType.maxPressure;

            public float GetSliderValue(int index) => target.geyser.configuration.GetMaxPressure();

            public void SetSliderValue(float value, int index)
            {
                if (target.uncapPressure == null)
                {
                    Debug.Log(target.geyser.configuration.GetMaxPressure());
                    target.geyser.RemoveModification(target.modification);
                    target.modification.maxPressureModifier = -1f + value / target.geyser.configuration.geyserType.maxPressure;
                    target.geyser.AddModification(target.modification);
                    target.slidersSet = true;
                    //Debug.Log("Set Pressure Slider, Uncapped is Null" + "  " + target.geyser.configuration.GetMaxPressure());
                    return;
                }
                if (target.uncapPressure.uncappedPressure == false)
                {
                    target.geyser.RemoveModification(target.modification);
                    target.modification.maxPressureModifier = -1f + value / target.geyser.configuration.geyserType.maxPressure;
                    target.geyser.AddModification(target.modification);
                    target.slidersSet = true;
                    //Debug.Log("Set Pressure Slider, Uncapped is false" + "  " + target.geyser.configuration.GetMaxPressure());
                    return;
                }
                if (target.uncapPressure.uncappedPressure == true)
                {
                    //Debug.Log("Set Pressure Slider, Uncapped is true" + "  " + target.geyser.configuration.GetMaxPressure());
                }
                
            }

            public string GetSliderTooltipKey(int index) => "STRINGS.SLIDERS.MAXPRESSURECONTROLS.TOOLTIP";

            public string GetSliderTooltip(int index) => STRINGS.SLIDERS.MAXPRESSURECONTROLS.TOOLTIP;
           
        }*/

        protected class TempController : ISliderControl
        {
            protected GeyserSliders target;
            public TempController(GeyserSliders t) => this.target = t;
            
            public string SliderTitleKey => "STRINGS.SLIDERS.TEMPCONTROLLER.NAME";
            public string SliderUnits => STRINGS.SLIDERS.TEMPCONTROLLER.UNITS;
            public int SliderDecimalPlaces(int index) => 0;
            public float GetSliderMin(int index) => -target.geyser.configuration.geyserType.temperature;
            //public float GetSliderMax(int index) => 9999f - target.geyser.configuration.geyserType.temperature;


            
            public float GetSliderMax(int index) 
            {
                var headroom = GeoTunerConfig.MAX_GEOTUNED * GeoTunerConfig.geotunerGeyserSettings[target.geyser.configuration.typeId].template.temperatureModifier;
                return 9999f - target.geyser.configuration.geyserType.temperature - headroom;
            } 

            public float GetSliderValue(int index) => (int)target.tempModification.temperatureModifier;
            public void SetSliderValue(float value, int index)
            {
                target.geyser.RemoveModification(target.tempModification);
                target.tempModification.temperatureModifier = value;
                target.geyser.AddModification(target.tempModification);
            }
            public string GetSliderTooltipKey(int index) => "STRINGS.SLIDERS.TEMPCONTROLLER.TOOLTIP";
            public string GetSliderTooltip(int index) => STRINGS.SLIDERS.TEMPCONTROLLER.TOOLTIP;
        }

        public string SidescreenButtonText => enableSidescreen ? STRINGS.BUTTONS.SLIDERBUTTON.HIDE.NAME : STRINGS.BUTTONS.SLIDERBUTTON.SHOW.NAME;        
        public string SidescreenButtonTooltip => studyable.Studied ? STRINGS.BUTTONS.SLIDERBUTTON.HIDE.TOOLTIP : STRINGS.BUTTONS.DISABLED.TOOLTIP;        
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

        bool ISidescreenButtonControl.SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable() => studyable.Studied;

        public void OnSidescreenButtonPressed()
        {
            enableSidescreen = !enableSidescreen;
            if (kSelectable.IsSelected)
            {
                SelectTool.Instance.Select(null, true);
                SelectTool.Instance.Select(kSelectable, true);
            }
        }

        public int HorizontalGroupID() => -1;

        public int ButtonSideScreenSortOrder() => 1;

        public void Sim1000ms(float dt)
        {
            if (!runSim) { return; }
            runSim = false;            
            geyser.smi.StopSM("Resync SM with new time values");
            geyser.smi.StartSM();
            if (kSelectable.IsSelected) 
            { 
                SelectTool.Instance.Select(null, true);
                SelectTool.Instance.Select(kSelectable, true);
            }                        
            return;

        }

        internal class ResetButton : KMonoBehaviour, ISidescreenButtonControl
        {
            [MyCmpReq]
            private readonly GeyserSliders geyserSliders;
            [MyCmpReq]
            private readonly Studyable studyable;
            [MyCmpReq]
            private readonly KSelectable kSelectable;            
            private bool onCooldown = false;            
            private int clicks = 0;            
            public string SidescreenButtonText
            {
                get
                {
                    if (Config.Instance.disableCooldowns)
                        return STRINGS.BUTTONS.RESETBUTTON.NAME;
                    else
                        return clicks > 0 ? STRINGS.BUTTONS.CONFIRM.NAME : STRINGS.BUTTONS.RESETBUTTON.NAME;                    
                }
            } 
            public string SidescreenButtonTooltip => studyable.Studied ? STRINGS.BUTTONS.RESETBUTTON.TOOLTIP : STRINGS.BUTTONS.DISABLED.TOOLTIP;
            public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

            bool ISidescreenButtonControl.SidescreenEnabled() => true;

            public bool SidescreenButtonInteractable() 
            { 
                if (Config.Instance.disableCooldowns)
                    return studyable.Studied;
                else
                    return studyable.Studied && !onCooldown;
            }//=> studyable.Studied && !onCooldown;

            public void OnSidescreenButtonPressed()
            {
                clicks++;
                if (!Config.Instance.disableCooldowns)
                {
                    StopAllCoroutines();
                    onCooldown = true;
                    StartCoroutine(ResetButtonCooldown(kSelectable));
                    if (clicks == 1)
                        StartCoroutine(ResetButtonConfirmRevert(kSelectable));
                }                                
                if (clicks > 1 || Config.Instance.disableCooldowns)
                {
                    var geyser = geyserSliders.geyser;
                    var newconfig = geyserSliders.newconfig;
                    geyser.configuration.Init(true);
                    geyser.ApplyConfigurationEmissionValues(geyser.configuration);
                    newconfig.scaledRate = geyser.configuration.scaledRate;
                    newconfig.scaledIterationLength = geyser.configuration.scaledIterationLength;
                    newconfig.scaledIterationPercent = geyser.configuration.scaledIterationPercent;
                    newconfig.scaledYearLength = geyser.configuration.scaledYearLength;
                    newconfig.scaledYearPercent = geyser.configuration.scaledYearPercent;
                    geyserSliders.runSim = true;
                    clicks = 0;
                    geyser.RemoveModification(geyserSliders.tempModification);
                    geyserSliders.tempModification.temperatureModifier = 0;
                    geyser.AddModification(geyserSliders.tempModification);

                }
                

                if (kSelectable.IsSelected)
                {
                    SelectTool.Instance.Select(null, true);
                    SelectTool.Instance.Select(kSelectable, true);
                }
            }
            IEnumerator ResetButtonCooldown(KSelectable kSelectable)
            {
                yield return new WaitForSecondsRealtime(0.25f);
                onCooldown = false;
                if (kSelectable.IsSelected)
                {
                    SelectTool.Instance.Select(null, true);
                    SelectTool.Instance.Select(kSelectable, true);
                }
            }
            IEnumerator ResetButtonConfirmRevert(KSelectable kSelectable) 
            {
                yield return new WaitForSecondsRealtime(3f);
                if (clicks != 1)
                    yield break;
                clicks = 0;
                if (kSelectable.IsSelected)
                {
                    SelectTool.Instance.Select(null, true);
                    SelectTool.Instance.Select(kSelectable, true);
                }
            }            
            public int HorizontalGroupID() => -1;

            public int ButtonSideScreenSortOrder() => 2;
        }

        internal class RandomizeSlidersButton : KMonoBehaviour, ISidescreenButtonControl
        {
            [MyCmpReq]
            private readonly Geyser geyser;
            [MyCmpReq]
            private readonly GeyserSliders geyserSliders;
            [MyCmpReq]
            private readonly Studyable studyable;
            [MyCmpReq]
            private readonly KSelectable kSelectable;           
            private int clicks = 0;            
            private bool onCooldown = false;
            public string SidescreenButtonText
            {
                get
                {
                    if (Config.Instance.disableCooldowns)
                        return STRINGS.BUTTONS.RANDOMIZESLIDERSBUTTON.NAME;
                    else
                        return clicks > 0 ? STRINGS.BUTTONS.CONFIRM.NAME : STRINGS.BUTTONS.RANDOMIZESLIDERSBUTTON.NAME;
                }
            }//=> STRINGS.BUTTONS.RANDOMIZESLIDERSBUTTON.NAME;
            public string SidescreenButtonTooltip => studyable.Studied ? STRINGS.BUTTONS.RANDOMIZESLIDERSBUTTON.TOOLTIP : STRINGS.BUTTONS.DISABLED.TOOLTIP;
            public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
            bool ISidescreenButtonControl.SidescreenEnabled() => true;
            public bool SidescreenButtonInteractable()
            {
                if (Config.Instance.disableCooldowns)
                    return studyable.Studied;
                else
                    return studyable.Studied && !onCooldown;
            }
            //=> studyable.Studied;// && !onCooldown;
            public void OnSidescreenButtonPressed()
            {
                clicks++;
                if (!Config.Instance.disableCooldowns)
                {
                    StopAllCoroutines();
                    onCooldown = true;
                    StartCoroutine(RandomizeButtonCooldown(kSelectable));
                    if (clicks == 1)
                        StartCoroutine(RandomizeButtonConfirmRevert(kSelectable));
                }
                if (clicks > 1 || Config.Instance.disableCooldowns)
                {
                    float newMass = UnityEngine.Random.Range(geyserSliders.sliderControls[0].GetSliderMin(0), geyserSliders.sliderControls[0].GetSliderMax(0));
                    float newIterationLength = UnityEngine.Random.Range(geyserSliders.sliderControls[1].GetSliderMin(0), geyserSliders.sliderControls[1].GetSliderMax(0));
                    float newIterationPercent = UnityEngine.Random.Range(geyserSliders.sliderControls[2].GetSliderMin(0), geyserSliders.sliderControls[2].GetSliderMax(0)) / 100;
                    float newYearLength = UnityEngine.Random.Range(geyserSliders.sliderControls[3].GetSliderMin(0), geyserSliders.sliderControls[3].GetSliderMax(0));
                    float newYearPercent = UnityEngine.Random.Range(geyserSliders.sliderControls[4].GetSliderMin(0), geyserSliders.sliderControls[4].GetSliderMax(0)) / 100;
                    geyserSliders.newconfig.scaledRate = newMass;
                    geyserSliders.newconfig.scaledIterationLength = newIterationLength;
                    geyserSliders.newconfig.scaledIterationPercent = newIterationPercent;
                    geyserSliders.newconfig.scaledYearLength = newYearLength;
                    geyserSliders.newconfig.scaledYearPercent = newYearPercent;
                    geyser.configuration.scaledRate = newMass;
                    geyser.configuration.scaledIterationLength = newIterationLength;
                    geyser.configuration.scaledIterationPercent = newIterationPercent;
                    geyser.configuration.scaledYearLength = newYearLength;
                    geyser.configuration.scaledYearPercent = newYearPercent;

                    geyserSliders.geyser.ApplyConfigurationEmissionValues(geyser.configuration);
                    geyserSliders.runSim = true;

                    clicks = 0;
                }

                if (kSelectable.IsSelected)
                {
                    SelectTool.Instance.Select(null, true);
                    SelectTool.Instance.Select(kSelectable, true);
                }

            }
            IEnumerator RandomizeButtonCooldown(KSelectable kSelectable)
            {
                yield return new WaitForSecondsRealtime(0.25f);
                onCooldown = false;
                if (kSelectable.IsSelected)
                {
                    SelectTool.Instance.Select(null, true);
                    SelectTool.Instance.Select(kSelectable, true);
                }
            }
            IEnumerator RandomizeButtonConfirmRevert(KSelectable kSelectable)
            {
                yield return new WaitForSecondsRealtime(3);
                if (clicks != 1)
                    yield break;
                clicks = 0;
                if (kSelectable.IsSelected)
                {
                    SelectTool.Instance.Select(null, true);
                    SelectTool.Instance.Select(kSelectable, true);
                }
            }
            public int HorizontalGroupID() => -1;

            public int ButtonSideScreenSortOrder() => 2;
        }


    }

    internal class DormancyButton : KMonoBehaviour, ISidescreenButtonControl
    {
        [MyCmpReq]
        private readonly Geyser geyser;
        [MyCmpReq]
        private readonly Studyable studyable;
        [MyCmpReq]
        private readonly KSelectable kSelectable;
        private bool onCooldown = false;
        private int clicks = 0;
        public string SidescreenButtonText 
        { 
            get
            {
                if (clicks != 1 || Config.Instance.disableCooldowns)
                    return IsDormant(geyser) ? STRINGS.BUTTONS.DORMANCYBUTTON.EXIT.NAME : STRINGS.BUTTONS.DORMANCYBUTTON.ENTER.NAME;                
                return STRINGS.BUTTONS.CONFIRM.NAME;
                
            }
        }// => IsDormant(geyser) ? STRINGS.BUTTONS.DORMANCYBUTTON.EXIT.NAME : STRINGS.BUTTONS.DORMANCYBUTTON.ENTER.NAME;
        public string SidescreenButtonTooltip //=> IsDormant(geyser) ? STRINGS.BUTTONS.DORMANCYBUTTON.EXIT.TOOLTIP : STRINGS.BUTTONS.DORMANCYBUTTON.ENTER.TOOLTIP;
        {
            get
            {
                if (!studyable.Studied)
                    return STRINGS.BUTTONS.DISABLED.TOOLTIP;
                return IsDormant(geyser) ? STRINGS.BUTTONS.DORMANCYBUTTON.EXIT.TOOLTIP : STRINGS.BUTTONS.DORMANCYBUTTON.ENTER.TOOLTIP;
            }
        }
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

        bool ISidescreenButtonControl.SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable()
        {
            if (Config.Instance.disableCooldowns)
                return studyable.Studied;
            else
                return studyable.Studied && !onCooldown;
        }
        //=> studyable.Studied && !onCooldown;

        public void OnSidescreenButtonPressed()
        {
            clicks++;
            if (!Config.Instance.disableCooldowns)
            {
                StopAllCoroutines();
                onCooldown = true;                
                StartCoroutine(DormancyButtonCooldown(kSelectable));
                if (clicks == 1)
                    StartCoroutine(DormancyButtonConfirmRevert(kSelectable));
            }
            //Debug.Log(smi.GetCurrentState().name);
            //if (smi.GetCurrentState().name != "root.dormant")
            if (clicks > 1 || Config.Instance.disableCooldowns)
            {
                if (!IsDormant(geyser))
                {
                    geyser.ShiftTimeTo(Geyser.TimeShiftStep.DormantState);
                    geyser.smi.GoTo(geyser.smi.sm.dormant);
                }
                else
                {
                    geyser.ShiftTimeTo(Geyser.TimeShiftStep.ActiveState);
                    geyser.smi.GoTo(geyser.smi.sm.idle);
                }
                clicks = 0;
            }
            if (kSelectable.IsSelected)
            {
                SelectTool.Instance.Select(null, true);
                SelectTool.Instance.Select(kSelectable, true);
            }
            
            //Debug.Log(smi.GetCurrentState().name);
        }
        public bool IsDormant(Geyser geyser)
        {
            return geyser.smi.GetCurrentState() == geyser.smi.sm.dormant;
        }
        IEnumerator DormancyButtonCooldown(KSelectable kSelectable)
        {
            yield return new WaitForSecondsRealtime(0.25f);
            onCooldown = false;
            if (kSelectable.IsSelected)
            {
                SelectTool.Instance.Select(null, true);
                SelectTool.Instance.Select(kSelectable, true);
            }
        }
        IEnumerator DormancyButtonConfirmRevert(KSelectable kSelectable)
        {
            yield return new WaitForSecondsRealtime(3);
            if (clicks != 1)
                yield break;
            clicks = 0;
            if (kSelectable.IsSelected)
            {
                SelectTool.Instance.Select(null, true);
                SelectTool.Instance.Select(kSelectable, true);
            }
        }
        public int HorizontalGroupID() => -1;

        public int ButtonSideScreenSortOrder() => 2;
    }
    internal class EruptionButton : KMonoBehaviour, ISidescreenButtonControl
    {
        [MyCmpReq]
        private readonly Geyser geyser;
        [MyCmpReq]
        private readonly Studyable studyable;
        [MyCmpReq]
        private readonly KSelectable kSelectable;        
        private bool onCooldown = false;
        private int clicks = 0;
        public string SidescreenButtonText
        {
            get
            {
                if (clicks != 1 || Config.Instance.disableCooldowns)
                    return STRINGS.BUTTONS.ERUPTIONBUTTON.NAME;
                return STRINGS.BUTTONS.CONFIRM.NAME;                
            }
        }//=> STRINGS.BUTTONS.ERUPTIONBUTTON.NAME;
        public string SidescreenButtonTooltip => studyable.Studied ?  STRINGS.BUTTONS.ERUPTIONBUTTON.TOOLTIP : STRINGS.BUTTONS.DISABLED.TOOLTIP;
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
        bool ISidescreenButtonControl.SidescreenEnabled() => true;
        public bool SidescreenButtonInteractable()
        {
            if (Config.Instance.disableCooldowns)
                return studyable.Studied;
            else
                return studyable.Studied && !onCooldown;
        }//=> studyable.Studied && !onCooldown;
        public void OnSidescreenButtonPressed()
        {
            clicks++;
            if (!Config.Instance.disableCooldowns)
            {
                StopAllCoroutines();
                onCooldown = true;                
                StartCoroutine(EruptionButtonCooldown(kSelectable));
                if (clicks == 1)
                    StartCoroutine(EruptionButtonConfirmRevert(kSelectable));
            }

            if (clicks > 1 || Config.Instance.disableCooldowns)
            {
                if (IsDormant(geyser))
                {
                    geyser.ShiftTimeTo(Geyser.TimeShiftStep.ActiveState);
                    geyser.smi.GoTo(geyser.smi.sm.idle);
                }
                else
                {
                    geyser.ShiftTimeTo(Geyser.TimeShiftStep.NextIteration);
                    geyser.smi.GoTo(geyser.smi.sm.pre_erupt);
                }
                clicks = 0;
            }
        }
        IEnumerator EruptionButtonCooldown(KSelectable kSelectable)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            onCooldown = false;
            if (kSelectable.IsSelected)
            {
                SelectTool.Instance.Select(null, true);
                SelectTool.Instance.Select(kSelectable, true);
            }
        }
        IEnumerator EruptionButtonConfirmRevert(KSelectable kSelectable)
        {
            yield return new WaitForSecondsRealtime(3);
            if (clicks != 1)
                yield break;
            clicks = 0;
            if (kSelectable.IsSelected)
            {
                SelectTool.Instance.Select(null, true);
                SelectTool.Instance.Select(kSelectable, true);
            }
        }
        public bool IsDormant(Geyser geyser)
        {
            return geyser.smi.GetCurrentState() == geyser.smi.sm.dormant;
        }              
        public int HorizontalGroupID() => -1;

        public int ButtonSideScreenSortOrder() => 2;        
    }          
    internal class UncapPressureCheckbox : KMonoBehaviour, ICheckboxControl//, ISim4000ms
    {
        [MyCmpReq]
        private readonly GeyserSliders geyserSliders;
        [Serialize]
        public bool uncappedPressure = false;
        public string CheckboxTitleKey => "STRINGS.CHECKBOX.UNCAPPRESSURE.NAME";//{ get; }

        public string CheckboxLabel => STRINGS.CHECKBOX.UNCAPPRESSURE.NAME; //{ get; }

        public string CheckboxTooltip => STRINGS.CHECKBOX.UNCAPPRESSURE.TOOLTIP; //{ get; }

        public bool GetCheckboxValue() => uncappedPressure;

        public void SetCheckboxValue(bool flag)
        {
            if (flag != uncappedPressure)
            {
                //Debug.Log("Set Checkbox");
                uncappedPressure = flag;
                geyserSliders.geyser.RemoveModification(geyserSliders.modification);
                geyserSliders.modification.maxPressureModifier = flag ? -1 + 1e30f / geyserSliders.geyser.configuration.geyserType.maxPressure : 0;//float.MaxValue / 1000000f : 0;
                geyserSliders.geyser.AddModification(geyserSliders.modification);
            }
        }
        /*public void Sim4000ms(float dt)
        {
            Debug.Log(geyserSliders.geyser.configuration.GetMaxPressure());
        }*/
    }
    /* internal class TempControl : KMonoBehaviour, IMultiSliderControl
     {
         [MyCmpReq]
         public readonly GeyserSliders geyserSliders;
         [MyCmpReq]
         public readonly Geyser geyser;
         [Serialize]
         public Geyser.GeyserModification tempModification = new Geyser.GeyserModification() { originID = "GeyserControl" };

         protected ISliderControl[] sliderControls;
         public TempControl()
         {
             this.sliderControls = new ISliderControl[1]
             {
                 new TempController(this)
             };
         }

         string IMultiSliderControl.SidescreenTitleKey
         {
             get => "STRINGS.SLIDERS.TEMPCONTROLLER.NAME";
         }

         ISliderControl[] IMultiSliderControl.sliderControls => this.sliderControls;

         bool IMultiSliderControl.SidescreenEnabled() => true;//geyserSliders.enableSidescreen;//studyable.Studied;
         protected class TempController : ISliderControl
         {
             public TempControl target;
             public TempController(TempControl t) => this.target = t;

             public string SliderTitleKey => "STRINGS.SLIDERS.TEMPCONTROLLER.NAME";
             public string SliderUnits => STRINGS.SLIDERS.TEMPCONTROLLER.UNITS;
             public int SliderDecimalPlaces(int index) => 0;
             public float GetSliderMin(int index) => -200;
             public float GetSliderMax(int index) => 200;
             public float GetSliderValue(int index) => (int)target.tempModification.temperatureModifier;
             public void SetSliderValue(float value, int index)
             {
                 target.geyser.RemoveModification(target.tempModification);
                 target.tempModification.temperatureModifier = value;
                 target.geyser.AddModification(target.tempModification);
             }
             public string GetSliderTooltipKey(int index) => "STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.TOOLTIP";
             public string GetSliderTooltip(int index) => STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.TOOLTIP;
         }


     }*/

    /*internal class TempControl : KMonoBehaviour, ISingleSliderControl
    {
        [MyCmpReq]
        public readonly GeyserSliders geyserSliders;
        [MyCmpReq]
        public readonly Geyser geyser;
        [Serialize]
        public Geyser.GeyserModification tempModification = new Geyser.GeyserModification() { originID = "GeyserControl" };

        protected override void OnSpawn()
        {
            base.OnSpawn();
            geyser.AddModification(tempModification);
        }

        public string SliderTitleKey => "STRINGS.SLIDERS.TEMPCONTROLLER.NAME";
        public string SliderUnits => STRINGS.SLIDERS.TEMPCONTROLLER.UNITS;
        public int SliderDecimalPlaces(int index) => 0;
        public float GetSliderMin(int index) => -200;
        public float GetSliderMax(int index) => 200;
        public float GetSliderValue(int index) => (int)tempModification.temperatureModifier;
        public void SetSliderValue(float value, int index)
        {
            geyser.RemoveModification(tempModification);
            tempModification.temperatureModifier = value;
            geyser.AddModification(tempModification);
        }
        public string GetSliderTooltipKey(int index) => "STRINGS.SLIDERS.TEMPCONTROLLER.TOOLTIP";
        public string GetSliderTooltip(int index) => STRINGS.SLIDERS.TEMPCONTROLLER.TOOLTIP;
        


    }*/

}
