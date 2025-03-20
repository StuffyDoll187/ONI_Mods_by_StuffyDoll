using ProcGen;
using System.Collections.Generic;
using UnityEngine;


namespace Starmap_Shenanigans
{
    public class EaterOfWorlds
    {
        public static void DestroyWorld(int idToDestroy)
        {            
            Debug.Log("[EaterOfWorlds] Destroying World Id: " + idToDestroy);    
            
            int newTargetWorldId = FindSuitableActiveWorld(idToDestroy);
            if (newTargetWorldId == -1)
            {
                Debug.Log("[EaterOfWorlds] No suitable new active world found. Aborting Destruction.");
                return;
            }            
            if (idToDestroy == ClusterManager.Instance.activeWorldId)
                ClusterManager.Instance.SetActiveWorld(newTargetWorldId);

            WorldContainer world = ClusterManager.Instance.GetWorld(idToDestroy);
            world.CancelChores();
            
            ClearWorldZones(idToDestroy);
            DestroySpawnables(idToDestroy);
            DestroyBuildings(idToDestroy);
            DestroyPickupables(idToDestroy);
            DestroyPrioritizables(idToDestroy);
            DestroyByLayers(idToDestroy);//most of these are probably unecceasary but there were things leftover like OilWells and the SapTree

            //todo: figure out destroying falling Unstable Ground and FallingWater liquids...

            
            var gameplayEvents = new List<GameplayEventInstance>();
            GameplayEventManager.Instance.GetActiveEventsOfType<GameplayEvent>(idToDestroy, ref gameplayEvents);
            foreach (GameplayEventInstance evt in gameplayEvents)
                GameplayEventManager.Instance.RemoveActiveEvent(evt);

            Grid.FreeGridSpace(world.WorldSize, world.WorldOffset);

            GameScheduler.Instance.ScheduleNextFrame("SecondPassForMissedSeeds", obj => DestroyPrioritizables(idToDestroy));
            GameScheduler.Instance.ScheduleNextFrame("ExcludesMeteorsButTheyNoLongerExist", obj => RedirectClusterTravelers(idToDestroy, newTargetWorldId));
            
            GameScheduler.Instance.ScheduleNextFrame("", obj => ClusterManager.Instance.UnregisterWorldContainer(world));
            GameScheduler.Instance.ScheduleNextFrame("", obj => DestroyWorldObject(world));

            if (SpeedControlScreen.Instance.IsPaused)
                SpeedControlScreen.Instance.DebugStepFrame();
        }

        private static void DestroySpawnables(int worldId)
        {
            var list = SaveGame.Instance.worldGenSpawner.GetSpawnables();
            foreach (var spawnable in list)
            {
                if (Grid.IsValidCellInWorld(spawnable.cell, worldId))
                {
                    //Debug.Log("Spawnables ");
                    //Debug.Log(spawnable + " " + spawnable.cell + " " + spawnable.spawnInfo.id + " " + spawnable.spawnInfo.type);
                    spawnable.FreeResources();
                }
            }
        }
        
        private static void DestroyByLayers(int worldId)
        {
            var world = ClusterManager.Instance.GetWorld(worldId);
            var extents = new Extents(world.WorldOffset.x, world.WorldOffset.y, world.Width, world.Height);
            for (int layer = 0; layer < 45; ++layer)
            {
                for (int x = extents.x; x <= extents.x + extents.width; ++x)
                {
                    for (int y = extents.y; y <= extents.y + extents.height; ++y)
                    {
                        int cell = Grid.XYToCell(x, y);
                        GameObject go = Grid.Objects[cell, layer];

                        if (go != null)
                        {
                            //Debug.Log(layer + " Destroying " + go.name);
                            go.DeleteObject();
                        }
                    }
                }
            }
        }
        private static void DestroyWorldObject(WorldContainer world)
        {
            if (world.TryGetComponent(out WorldInventory worldInventory))
                UnityEngine.Object.Destroy(worldInventory);
            world.gameObject.DeleteObject();
        }
        private static void DestroyBuildings(int worldId)
        {
            foreach (BuildingComplete building in Components.BuildingCompletes.GetWorldItems(worldId))
            {
                //Debug.Log("Destroying " + building);
                building.DeleteObject();
            }
        }
        private static void DestroyPrioritizables(int worldId)
        {
            foreach (Prioritizable prioritizable in Components.Prioritizables.GetWorldItems(worldId))
            {
                //Debug.Log("Destroying " + prioritizable);
                prioritizable.DeleteObject();
            }
        }
        private static void DestroyPickupables(int worldId)
        {
            foreach (Pickupable pickupable in Components.Pickupables.GetWorldItems(worldId))
            {
                //Debug.Log("Destroying " + pickupable);
                pickupable.DeleteObject();
            }
        }
        private static void ClearWorldZones(int worldId)
        {
            var world = ClusterManager.Instance.GetWorld(worldId);
            var overworldCells = SaveLoader.Instance.clusterDetailSave.overworldCells;
            for (int i = overworldCells.Count - 1; i >= 0; i--)
            {
                Vector2 vector = overworldCells[i].poly.Centroid();
                if (Grid.IsValidCellInWorld(Grid.XYToCell((int)vector.x, (int)vector.y), worldId))
                {
                    overworldCells[i].zoneType = SubWorld.ZoneType.Space;
                    World.Instance.zoneRenderData.OnActiveWorldChanged();
                    overworldCells.RemoveAt(i);
                }
            }            
            for (int x = world.WorldOffset.x; x < world.maximumBounds.x; x++)
                for (int y = world.WorldOffset.y; y < world.maximumBounds.y; y++)
                {
                    int cell = Grid.XYToCell(x, y);
                    SimMessages.ModifyCellWorldZone(cell, byte.MaxValue);
                }
        }

        private static void RedirectClusterTravelers(int fromWorldID, int toWorldId)
        {
            foreach (ClusterTraveler clusterTraveler in Components.ClusterTravelers)
            {
                if (clusterTraveler.TryGetComponent(out ClusterMapMeteorShowerVisualizer _))
                    continue;

                if (clusterTraveler.TryGetComponent(out ClusterDestinationSelector clusterDestinationSelector))
                {
                    if (clusterDestinationSelector.GetDestination() == ClusterManager.Instance.GetWorld(fromWorldID).GetComponent<ClusterGridEntity>().Location)
                    {
                        clusterDestinationSelector.SetDestination(ClusterManager.Instance.GetWorld(toWorldId).GetComponent<AsteroidGridEntity>().Location);
                        //Debug.Log("Redirected " + clusterTraveler.name);

                        var escapePod = clusterTraveler.GetSMI<TravellingCargoLander.StatesInstance>();
                        if (escapePod != null)
                        {
                            //Debug.Log("Set destinationWorld parameter for Escape Pod");
                            escapePod.sm.destinationWorld.Set(toWorldId, escapePod);
                        }
                        var launcherPayload = clusterTraveler.GetSMI<RailGunPayload.StatesInstance>();
                        if (launcherPayload != null)
                        {
                            launcherPayload.sm.destinationWorld.Set(toWorldId, launcherPayload);
                        }
                    }
                }
            }
        }

        private static int FindSuitableActiveWorld(int excludedWorldId)
        {
            var startWorldId = ClusterManager.Instance.GetStartWorld().id;          
            if (excludedWorldId != startWorldId)
                return startWorldId;
            var list = new List<WorldContainer>();
            foreach (var world in ClusterManager.Instance.WorldContainers)
            {
                if (world.id != excludedWorldId)
                    list.Add(world);
            }                            
            foreach (var world in list)
            {                
                if (world.IsDupeVisited)
                    return world.id;
            }
            foreach (var world in list)
            {
                if (world.IsSurfaceRevealed)
                    return world.id;
            }
            foreach (var world in list)
            {
                if (world.IsDiscovered)
                    return world.id;
            }
            foreach (var world in list)
            {
                if (world.IsModuleInterior)
                    return world.id;
            }
            return -1;
            
            

            
        }


    }
}
