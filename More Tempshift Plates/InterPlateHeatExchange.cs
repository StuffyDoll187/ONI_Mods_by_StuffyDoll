using System;
using System.Collections.Generic;
using UnityEngine;
using KSerialization;

namespace More_Tempshift_Plates
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class InterPlateHeatExchange : KMonoBehaviour
    {
        [MyCmpGet]
        private Building building;
        [MyCmpAdd]
        public CopyBuildingSettings copyBuildingSettings;
        private List<int> conductiveCells;
        private HashSet<int> inContactBuildings = new HashSet<int>();
        private bool hasBeenRegister;
        private bool buildingDestroyed;
        private int selfHandle;
        [Serialize]
        private bool allowHeatExchange = Config.Instance.Default_Toggle_Setting;

        private static readonly EventSystem.IntraObjectHandler<InterPlateHeatExchange> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<InterPlateHeatExchange>((Action<InterPlateHeatExchange, object>)((component, data) => component.OnCopySettings(data)));
        private static readonly EventSystem.IntraObjectHandler<InterPlateHeatExchange> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<InterPlateHeatExchange>((Action<InterPlateHeatExchange, object>)((component, data) => component.OnRefreshUserMenu(data)));
        protected static readonly EventSystem.IntraObjectHandler<InterPlateHeatExchange> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<InterPlateHeatExchange>((Action<InterPlateHeatExchange, object>)((component, data) => component.OnStructureTemperatureRegistered(data)));


        private void OnRefreshUserMenu(object data)
        {
            Game.Instance.userMenu.AddButton(this.gameObject, this.allowHeatExchange ? new KIconButtonMenu.ButtonInfo("action_building_disabled", "Disable Heat Exchange", new System.Action(this.DisableHeatExchange), tooltipText: "Click to Clear this Plate's Heat Exchange Targets") : new KIconButtonMenu.ButtonInfo("action_switch_toggle", "Enable Heat Exchange", new System.Action(this.EnableHeatExchange), tooltipText: "Click to Reaquire Heat Exchange Targets"), 10f);
        }

        private void EnableHeatExchange()
        {
            this.allowHeatExchange = true;
            this.RefreshConductiveCells();
            this.Refresh_InContactBuildings();
        }

        private void DisableHeatExchange()
        {
            this.allowHeatExchange = false;
            this.RefreshConductiveCells();
            this.Refresh_InContactBuildings();
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<InterPlateHeatExchange>(-1555603773, InterPlateHeatExchange.OnStructureTemperatureRegisteredDelegate);
            this.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
        }

        private void OnCopySettings(object data)
        {
            InterPlateHeatExchange component = ((GameObject)data).GetComponent<InterPlateHeatExchange>();
            if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                return;
            this.allowHeatExchange = component.allowHeatExchange;            
        }
        protected override void OnSpawn()
        {
            base.OnSpawn();
            //Debug.Log("InterPlateHeatExchange OnSpawn");
            this.DefineConductiveCells();
            this.Subscribe<InterPlateHeatExchange>(493375141, InterPlateHeatExchange.OnRefreshUserMenuDelegate);
            GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
            
        }

        protected override void OnCleanUp()
        {
            GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));            
            this.UnregisterToSIM();
            base.OnCleanUp();
        }

        private void OnStructureTemperatureRegistered(object _sim_handle)
        {
            this.RegisterToSIM((int)_sim_handle);
        }

        private void RegisterToSIM(int sim_handle1)
        {
            string name = this.building.Def.Name;
            HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle = Game.Instance.simComponentCallbackManager.Add((Action<int, object>)((sim_handle2, callback_data) => this.OnSimRegistered(sim_handle2)), (object)null, "InterPlateHeatExchange.SimRegister");
            SimMessages.RegisterBuildingToBuildingHeatExchange(sim_handle1, handle.index);
        }

        private void OnSimRegistered(int sim_handle)
        {
            if (sim_handle == -1)
                return;
            this.selfHandle = sim_handle;
            this.hasBeenRegister = true;
            if (this.buildingDestroyed)
                this.UnregisterToSIM();
            else
                this.Refresh_InContactBuildings();
        }

        private void UnregisterToSIM()
        {
            if (this.hasBeenRegister)
                SimMessages.RemoveBuildingToBuildingHeatExchange(this.selfHandle);
            this.buildingDestroyed = true;
        }

        internal void RefreshConductiveCells()
        {
            this.conductiveCells.Clear();
            if (!this.allowHeatExchange)
                return;
            StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
            HandleVector<int>.Handle handle = structureTemperatures.GetHandle(this.gameObject);
            StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
            int x = payload.overriddenExtents.x;
            int y = payload.overriddenExtents.y;
            int x1 = payload.overriddenExtents.width;
            int y1 = payload.overriddenExtents.height;
            for (int i = x; i < x + payload.overriddenExtents.width; i++)
            {
                for (int j = y; j < y + payload.overriddenExtents.height; j++)
                {
                    this.conductiveCells.Add(Grid.XYToCell(i, j));
                }
            }
            //Debug.Log("Number of Conductive Cells " + this.conductiveCells.Count);
        }
        private void DefineConductiveCells()
        {
            //Debug.Log("DefineConductiveCells");
            //this.conductiveCells = new List<int>((IEnumerable<int>)this.building.PlacementCells);
            //this.conductiveCells.Remove(this.building.GetUtilityInputCell());
            //this.conductiveCells.Remove(this.building.GetUtilityOutputCell());
            this.conductiveCells = new List<int>();
            RefreshConductiveCells();
        }

        private void Add(
          StructureToStructureTemperature.InContactBuildingData buildingData)
        {
            if (!this.inContactBuildings.Add(buildingData.buildingInContact))
                return;
            SimMessages.AddBuildingToBuildingHeatExchange(this.selfHandle, buildingData.buildingInContact, buildingData.cellsInContact);
        }

        private void Remove(int building)
        {
            if (!this.inContactBuildings.Contains(building))
                return;
            this.inContactBuildings.Remove(building);
            SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchange(this.selfHandle, building);
        }
        

        private void OnAnyBuildingChanged(int _cell, object _data)
        {

            
            
            if (!this.hasBeenRegister)
                return;
            StructureToStructureTemperature.BuildingChangedObj buildingChangedObj = (StructureToStructureTemperature.BuildingChangedObj)_data;
            //Debug.Log(buildingChangedObj.building.gameObject.layer.ToString() + "  " + buildingChangedObj.building.name.ToString());
            /*if (buildingChangedObj.building.GetComponent<InterPlateHeatExchange>() == null)
                return;*/
             
            


            bool flag = false;
            int num = 0;
            for (int index = 0; index < buildingChangedObj.building.PlacementCells.Length; ++index)
            {
                if (this.conductiveCells.Contains(buildingChangedObj.building.PlacementCells[index]))
                {
                    flag = true;
                    ++num;
                }
            }
            if (!flag)
                return;
            int simHandler = buildingChangedObj.simHandler;
            switch (buildingChangedObj.changeType)
            {
                case StructureToStructureTemperature.BuildingChangeType.Created:
                    if (Config.Instance.Restrict_To_Other_Plates && !buildingChangedObj.building.TryGetComponent<InterPlateHeatExchange>(out _))
                        break;
                    this.Add(new StructureToStructureTemperature.InContactBuildingData()
                    {
                        buildingInContact = simHandler,
                        cellsInContact = num
                    });
                    break;
                case StructureToStructureTemperature.BuildingChangeType.Destroyed:
                    this.Remove(simHandler);
                    break;
            }
        }

        internal void Refresh_InContactBuildings()
        {
            foreach (int building in this.inContactBuildings)
                SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchange(this.selfHandle, building);
            this.inContactBuildings.Clear();
            //Debug.Log("Refresh InContactBuildings");
            if (!this.allowHeatExchange)
                return;
            foreach (StructureToStructureTemperature.InContactBuildingData inContactBuilding in this.GetAll_InContact_Buildings())
                this.Add(inContactBuilding);
        }

        private List<StructureToStructureTemperature.InContactBuildingData> GetAll_InContact_Buildings()
        {
            //Debug.Log("GetAll InContactBuildings");
            Dictionary<Building, int> dictionary = new Dictionary<Building, int>();
            List<StructureToStructureTemperature.InContactBuildingData> contactBuildings = new List<StructureToStructureTemperature.InContactBuildingData>();
            List<GameObject> buildingsInCell = new List<GameObject>();
            foreach (int conductiveCell in this.conductiveCells)
            {
                int cell = conductiveCell;
                buildingsInCell.Clear();
                Action<int> action = (Action<int>)(layer =>
                {
                    GameObject gameObject = Grid.Objects[cell, layer];
                    
                    /*if (gameObject != null)
                    {
                        Debug.Log("Layer " + layer);
                        Debug.Log(gameObject.name);
                        
                    }*/
                    if (!((UnityEngine.Object)gameObject != (UnityEngine.Object)null) || buildingsInCell.Contains(gameObject))
                        return;
                    
                    buildingsInCell.Add(gameObject);
                });
                //for (int i = 0; i < 45; ++i) {action(i); }
                //action(1); //Buildings
                //action(2); //Background(Tempshift and Drywall)
                if (!Config.Instance.Restrict_To_Other_Plates)
                {
                    action(1); //Buildings
                    action(9); //Tiles + Gantry
                    action(26); //Power
                    action(27); //Power
                    action(31); //Automation
                    action(32); //Automation
                    action(30); //Automation
                                //29 liquid logic valve
                    action(12); //Ventilation
                    action(13); //Ventilation
                    action(16); //Plumbing
                    action(17); //Plumbing
                                //23 solid conduit inbox, solid logic valve, solid conduit bridge
                    action(24); //UNKNOWN
                                //29 HEP Spawner                
                    action(19); //Conduction Panel+
                }
                action(2); //Background

                for (int index = 0; index < buildingsInCell.Count; ++index)
                {
                    Building component = (UnityEngine.Object)buildingsInCell[index] == (UnityEngine.Object)null ? (Building)null : buildingsInCell[index].GetComponent<Building>();
                    if ((!((UnityEngine.Object)component != (UnityEngine.Object)null) || !component.Def.UseStructureTemperature ? 0 : (component.PlacementCellsContainCell(cell) ? 1 : 0)) != 0)
                    {
                        if (Config.Instance.Restrict_To_Other_Plates && !component.TryGetComponent<InterPlateHeatExchange>(out InterPlateHeatExchange _))
                            continue;
                        //if (component.GetComponent<InterPlateHeatExchange>() == null) continue;
                        //if (component.GetComponent<InterPlateHeatExchange>().allowHeatExchange == false) continue;
                        if (!dictionary.ContainsKey(component))
                            dictionary.Add(component, 0);
                        dictionary[component]++;
                    }
                }
            }
            foreach (Building key in dictionary.Keys)
            {
                HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle((MonoBehaviour)key);
                if (handle != HandleVector<int>.InvalidHandle)
                {
                    int simHandleCopy = GameComps.StructureTemperatures.GetPayload(handle).simHandleCopy;
                    StructureToStructureTemperature.InContactBuildingData contactBuildingData = new StructureToStructureTemperature.InContactBuildingData()
                    {
                        buildingInContact = simHandleCopy,
                        cellsInContact = dictionary[key]
                    };
                    contactBuildings.Add(contactBuildingData);
                }
            }
            return contactBuildings;
        }

        /*public enum BuildingChangeType
        {
            Created,
            Destroyed,
            Moved,
        }

        public struct InContactBuildingData
        {
            public int buildingInContact;
            public int cellsInContact;
        }

        public struct BuildingChangedObj
        {
            public InterPlateHeatExchange.BuildingChangeType changeType;
            public int simHandler;
            public Building building;

            public BuildingChangedObj(
              InterPlateHeatExchange.BuildingChangeType _changeType,
              Building _building,
              int sim_handler)
            {
                this.changeType = _changeType;
                this.building = _building;
                this.simHandler = sim_handler;
            }
        }*/
    }
}





    