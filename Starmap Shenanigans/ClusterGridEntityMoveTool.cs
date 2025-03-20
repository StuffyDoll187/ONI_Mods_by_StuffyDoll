using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Starmap_Shenanigans
{
    public class ClusterGridEntityMoveTool : InterfaceTool
    {                
        public static ClusterGridEntityMoveTool Instance;        
        public bool isActive;
        public bool isClusterMapScreenActive;

        public static ClusterGridEntity clusterGridEntityToMove = null;
        public static AxialI? storedEntityCoord = null;
        public static ClusterMapVisualizer clusterMapVisualizer = null;
        public static ClusterMapHex originalHex = null;

        public static FieldInfo m_gridEntityVis = AccessTools.Field(typeof(ClusterMapScreen), "m_gridEntityVis");
        public static FieldInfo m_hoveredHex = AccessTools.Field(typeof(ClusterMapScreen), "m_hoveredHex");
        public static FieldInfo m_selectedHex = AccessTools.Field(typeof(ClusterMapScreen), "m_selectedHex");

        public static void ClearVariables()
        {
            clusterGridEntityToMove = null;
            storedEntityCoord = null;
            clusterMapVisualizer = null;
            originalHex = null;
        }
        public static void DestroyInstance()
        {
            Instance = null;
        }

        [HarmonyPatch(typeof(Game), "DestroyInstances")]
        public class Game_DestroyInstances_Patch
        {
            public static void Postfix() => DestroyInstance();            
        }    
        
        protected override void OnSpawn()
        {
            base.OnSpawn();                        
        }
        protected override void OnPrefabInit()
        {
            Instance = this;
        } 

        public void Activate()
        {
            m_selectedHex.SetValue(ClusterMapScreen.Instance, null);
            PlayerController.Instance.ActivateTool((InterfaceTool)this);
            ToolMenu.Instance.PriorityScreen.ResetPriority();            
            isActive = true;
        }        

        protected override void OnDeactivateTool(InterfaceTool new_tool)
        {
            if (clusterMapVisualizer != null)
            {
                clusterMapVisualizer.transform.position = originalHex.transform.position;
                ClearVariables();
            }
            base.OnDeactivateTool(new_tool);
            this.ClearHover();            
            isActive = false;
        }

        
        public static void StoreHoveredEntityAndCoord()
        {
            if (ClusterMapScreen.Instance.HasCurrentHover())
            {
                AxialI currentHoverLocation = ClusterMapScreen.Instance.GetCurrentHoverLocation();

                if (ClusterGrid.Instance.cellContents.Count > 0)
                {
                    List<ClusterGridEntity> clusterGridEntities = ClusterGrid.Instance.cellContents[currentHoverLocation];
                    /*foreach (ClusterGridEntity entity in clusterGridEntities)
                    {
                        Debug.Log(entity.Name);
                    }*/
                    if (clusterGridEntities.Count > 0)
                    {
                        clusterGridEntityToMove = clusterGridEntities[0];
                        storedEntityCoord = clusterGridEntities[0].Location;
                        originalHex = (ClusterMapHex) m_hoveredHex.GetValue(ClusterMapScreen.Instance);
                        var dict = (Dictionary<ClusterGridEntity, ClusterMapVisualizer>) m_gridEntityVis.GetValue(ClusterMapScreen.Instance);
                        clusterMapVisualizer = dict[clusterGridEntityToMove];                        
                    }                    
                }                
            }
            else
            {                
                ClearVariables();
            }
                
        }

        
        
        public static void SetNewLocation()
        {
            if (clusterGridEntityToMove == null)
            {
                //Debug.Log("Entity is null");
                return;
            }
            AxialI currentHoverLocation = ClusterMapScreen.Instance.GetCurrentHoverLocation();
            clusterGridEntityToMove.Location = currentHoverLocation;                        
            
            var hoveredHex = (ClusterMapHex)m_hoveredHex.GetValue(ClusterMapScreen.Instance);
            clusterMapVisualizer.transform.position = hoveredHex.transform.position;


            foreach (ClusterTraveler clusterTraveler in Components.ClusterTravelers)
            {                                
                if (clusterTraveler.TryGetComponent(out ClusterDestinationSelector clusterDestinationSelector))
                {
                    if (clusterDestinationSelector.GetDestination() == storedEntityCoord)
                    {                        
                        clusterDestinationSelector.SetDestination(currentHoverLocation);                                             
                    }
                }
            }
            
            ClearVariables();
        }

        public override void OnMouseMove(Vector3 cursor_pos)
        { 
            if(clusterGridEntityToMove != null)
            {
                var hoveredHex = (ClusterMapHex)m_hoveredHex.GetValue(ClusterMapScreen.Instance);
                if (hoveredHex != null)
                    clusterMapVisualizer.transform.position = hoveredHex.transform.position;
            }
            base.OnMouseMove(cursor_pos);
        }

    }
}
