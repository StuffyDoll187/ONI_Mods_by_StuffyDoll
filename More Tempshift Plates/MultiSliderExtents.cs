using System;
using UnityEngine;
using KSerialization;

namespace More_Tempshift_Plates
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class MultiSliderExtents : KMonoBehaviour, IMultiSliderControl, ISim200ms
    {
        [MyCmpReq]
        private readonly Building building;        
        [MyCmpReq]
        private readonly RangeVisualizer rangeVisualizer;
        [MyCmpAdd]
        public CopyBuildingSettings copyBuildingSettings;
        [Serialize]
        private bool runSim200 = true;
        [Serialize]
        private CellOffset[] cellOffsets = new CellOffset[2];
                    
        protected ISliderControl[] sliderControls;

        protected override void OnSpawn()
        {
            base.OnSpawn();            
            this.UpdateExtents();
            //Debug.Log("MultisliderExtents OnSpawn");
            /*if (TryGetComponent<InterPlateHeatExchange>(out InterPlateHeatExchange component))
            {
                component.DefineConductiveCells();
                component.Refresh_InContactBuildings();
            }*/

        }
        protected override void OnPrefabInit()
        {            
            base.OnPrefabInit();
            this.Subscribe(-905833192, new Action<object>(this.OnCopySettings));            
        }

        private void OnCopySettings(object data)
        {
            MultiSliderExtents component = ((GameObject)data).GetComponent<MultiSliderExtents>();
            if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                return;
            //this.cellOffsets = component.cellOffsets;
            this.cellOffsets[0] = component.cellOffsets[0];
            this.cellOffsets[1] = component.cellOffsets[1];                        
            this.runSim200 = true;
            
        }

        public MultiSliderExtents()
        { this.sliderControls = new ISliderControl[4]
            {               
                (ISliderControl)new MultiSliderExtents.MaxYController((MultiSliderExtents) this),
                (ISliderControl)new MultiSliderExtents.MinYController((MultiSliderExtents) this),
                (ISliderControl)new MultiSliderExtents.MinXController((MultiSliderExtents) this),
                (ISliderControl)new MultiSliderExtents.MaxXController((MultiSliderExtents) this)                                           
            };
        }

        string IMultiSliderControl.SidescreenTitleKey
        {
            get => "STRINGS.SLIDERS.MULTISLIDEREXTENTS.NAME";
        }

        ISliderControl[] IMultiSliderControl.sliderControls => this.sliderControls;

        bool IMultiSliderControl.SidescreenEnabled() => true;

        protected class MinXController : ISingleSliderControl, ISliderControl
        {
            
            public MultiSliderExtents target;
            
            public MinXController(MultiSliderExtents t) => this.target = t;
           

            public string SliderTitleKey => "STRINGS.SLIDERS.MINXCONTROLLER.NAME";

            public string SliderUnits => "Tiles";

            public float GetSliderMax(int index) => 5f;

            public float GetSliderMin(int index) => 0f;

            public string GetSliderTooltip(int index)
            {
                return "Number of Columns to the Left";
            }

            public string GetSliderTooltipKey(int index) => "<unused>";

            public float GetSliderValue(int index) => -target.cellOffsets[0].x;

            public void SetSliderValue(float value, int index)
            {                
                target.cellOffsets[0].x = -(int)value;
                //target.rangeVisualizer.RangeMin.x = -(int)value;
                target.UpdateVisualizer();
                target.runSim200 = true;             
            }

            public int SliderDecimalPlaces(int index) => 0;
        }

        protected class MaxXController : ISingleSliderControl, ISliderControl
        {
            protected MultiSliderExtents target;
          

            public MaxXController(MultiSliderExtents t) => this.target = t;

            public string SliderTitleKey => "STRINGS.SLIDERS.MAXXCONTROLLER.NAME";

            public string SliderUnits => "Tiles";

            public float GetSliderMax(int index) => 5f;

            public float GetSliderMin(int index) => 0f;

            public string GetSliderTooltip(int index)
            {
                return "Number of Columns to the Right";
            }

            public string GetSliderTooltipKey(int index) => "<unused>";

            public float GetSliderValue(int index) => target.cellOffsets[1].x;

            public void SetSliderValue(float value, int index)
            {
                target.cellOffsets[1].x = (int)value;
                //target.rangeVisualizer.RangeMax.x = (int)value;
                target.UpdateVisualizer();
                target.runSim200 = true;
            }

            public int SliderDecimalPlaces(int index) => 0;
        }

        protected class MinYController : ISingleSliderControl, ISliderControl
        {
            protected MultiSliderExtents target;
          

            public MinYController(MultiSliderExtents t) => this.target = t;

            public string SliderTitleKey => "STRINGS.SLIDERS.MINYCONTROLLER.NAME";

            public string SliderUnits => "Tiles";

            public float GetSliderMax(int index) => 5f;

            public float GetSliderMin(int index) => 0f;

            public string GetSliderTooltip(int index)
            {
                return "Number of Rows Below";
            }

            public string GetSliderTooltipKey(int index) => "<unused>";

            public float GetSliderValue(int index) => -target.cellOffsets[0].y;

            public void SetSliderValue(float value, int index)
            {
                target.cellOffsets[0].y = -(int)value;
                //target.rangeVisualizer.RangeMin.y = -(int)value;
                target.UpdateVisualizer();
                target.runSim200 = true;
            }

            public int SliderDecimalPlaces(int index) => 0;
        }

        protected class MaxYController : ISingleSliderControl, ISliderControl
        {
            protected MultiSliderExtents target;
                       
            public MaxYController(MultiSliderExtents t) => this.target = t;

            public string SliderTitleKey => "STRINGS.SLIDERS.MAXYCONTROLLER.NAME";

            public string SliderUnits => "Tiles";

            public float GetSliderMax(int index) => 5f;

            public float GetSliderMin(int index) => 0f;

            public string GetSliderTooltip(int index)
            {
                return "Number of Rows Above";
            }

            public string GetSliderTooltipKey(int index) => "<unused>";

            public float GetSliderValue(int index) => target.cellOffsets[1].y;

            public void SetSliderValue(float value, int index)
            {
                target.cellOffsets[1].y = (int)value;
                //target.rangeVisualizer.RangeMax.y = (int)value;
                target.UpdateVisualizer();
                target.runSim200 = true;
            }
            public int SliderDecimalPlaces(int index) => 0;
        }

        private void UpdateVisualizer()
        {
            int cell = Grid.PosToCell(this.gameObject);
            int world = Grid.WorldIdx[cell];
            Grid.CellToXY(cell, out int x, out int y);
            this.rangeVisualizer.RangeMin.x = SafeExtents.ClampXToWorld(x + this.cellOffsets[0].x, world) - x;
            this.rangeVisualizer.RangeMin.y = SafeExtents.ClampYToWorld(y + this.cellOffsets[0].y, world) - y;
            this.rangeVisualizer.RangeMax.x = SafeExtents.ClampXToWorld(x + this.cellOffsets[1].x, world) - x;            
            this.rangeVisualizer.RangeMax.y = SafeExtents.ClampYToWorld(y + this.cellOffsets[1].y, world) - y;           
        }

        private void UpdateExtents()
        {
            StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
            HandleVector<int>.Handle handle = structureTemperatures.GetHandle(this.gameObject);
            StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
            //  if ( payload.overriddenExtents.width == (this.cellOffsets[1].x - this.cellOffsets[0].x + 1) && payload.overriddenExtents.height == (this.cellOffsets[1].y - this.cellOffsets[0].y + 1)) 
            //      return;                       
            int cell = Grid.PosToCell(this.gameObject);
            Extents newExtents = SafeExtents.Extents(cell, this.cellOffsets);
            payload.OverrideExtents(newExtents);
            GameComps.StructureTemperatures.SetPayload(handle, ref payload);
            BuildingDef def = this.building.Def;
            ushort idx = payload.primaryElement.Element.idx;
            SimMessages.ModifyBuildingHeatExchange(payload.simHandleCopy, newExtents, def.MassForTemperatureModification, payload.primaryElement.Temperature, def.ThermalConductivity, 10000f, 0f, idx);
        }

        
        public void Sim200ms(float dt)
        {            
            if (!runSim200) return;
            this.runSim200 = false;
            this.UpdateExtents();
            if (TryGetComponent<InterPlateHeatExchange>(out InterPlateHeatExchange component))
            {
                component.RefreshConductiveCells();
                component.Refresh_InContactBuildings();
            }
            

        }           
    }        
}
