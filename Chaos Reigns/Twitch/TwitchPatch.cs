using System;
using HarmonyLib;
using ONITwitchLib;
using ONITwitchLib.Core;

namespace Chaos_Reigns
{
    internal class TwitchPatch
    {
        [HarmonyPatch(typeof(WorldContainer), "OnPrefabInit")]
        public class WorldContainer_OnPrefabInit_Patch
        {
            public static void Postfix(WorldContainer __instance)
            {
                //if (TwitchModInfo.TwitchIsPresent)
                //{
                    if (Config.Instance.EnableTwitchMagmaRain)
                        __instance.gameObject.AddComponent<TwitchMagmaRain>();
                    if (Config.Instance.EnableTwitchZoological)
                        __instance.gameObject.AddComponent<TwitchZoologicalMeteors>();
                    if (Config.Instance.EnableTwitchWaterBalloons)
                        __instance.gameObject.AddComponent<TwitchWaterBalloons>();
                    if (Config.Instance.EnableTwitchMoltenSlugs)
                        __instance.gameObject.AddComponent<TwitchMoltenSpongeSlugs>();
                    if (Config.Instance.EnableTwitchNuclearWasteRain)
                        __instance.gameObject.AddComponent<TwitchNuclearWasteRain>();
                //}
            }
        }

        [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        [HarmonyAfter(TwitchModInfo.StaticID)]
        public static class DB_Initialize_Patch
        {
            public static void Postfix()
            {
                if (!TwitchModInfo.TwitchIsPresent)
                    return;

                EventGroup group = EventGroup.GetOrCreateGroup("ChaosReignsEventGroup");
                
                // 0,1,2,5,10,20,40,80
                if (Config.Instance.EnableTwitchMagmaRain)
                {   
                    EventInfo eventinfo = group.AddEvent("MagmaRain", 1);
                    eventinfo.FriendlyName = "Magma Rain";
                    eventinfo.Danger = Danger.Deadly;                
                    eventinfo.AddListener(new Action<object>(MagmaRain));
                    eventinfo.AddCondition(new Func<object, bool>(PreconditionMagmaRain));
                }
                if (Config.Instance.EnableTwitchNuclearWasteRain)
                {
                    EventInfo eventinfo2 = group.AddEvent("NuclearWasteRain", 2);
                    eventinfo2.FriendlyName = "Nuclear Waste Rain";
                    eventinfo2.Danger = Danger.Extreme;
                    eventinfo2.AddListener(new Action<object>(NuclearWasteRain));
                    eventinfo2.AddCondition(new Func<object, bool>(PreconditionNuclearWasteRain));
                }
                if (Config.Instance.EnableTwitchZoological)
                {
                    EventInfo eventinfo3 = group.AddEvent("ZoologicalMeteors", 5);
                    eventinfo3.FriendlyName = "ZoologicalMeteors";
                    eventinfo3.Danger = Danger.None;
                    eventinfo3.AddListener(new Action<object>(ZoologicalMeteors));
                    eventinfo3.AddCondition(new Func<object, bool>(PreconditionZoologicalMeteors));
                }
                if (Config.Instance.EnableTwitchWaterBalloons)
                {
                    EventInfo eventinfo4 = group.AddEvent("WaterBalloonMeteors", 20);
                    eventinfo4.FriendlyName = "Water Balloons";
                    eventinfo4.Danger = Danger.Small;
                    eventinfo4.AddListener(new Action<object>(WaterBalloons));
                    eventinfo4.AddCondition(new Func<object, bool>(PreconditionWaterBalloons));
                }
                if (Config.Instance.EnableTwitchMoltenSlugs)
                {
                    EventInfo eventinfo5 = group.AddEvent("MoltenSlugMeteors", 1);
                    eventinfo5.FriendlyName = "Molten Slug Meteors";
                    eventinfo5.Danger = Danger.Deadly;
                    eventinfo5.AddListener(new Action<object>(MoltenSlugs));
                    eventinfo5.AddCondition(new Func<object, bool>(PreconditionMoltenSlugs));
                }


                TwitchDeckManager.Instance.AddGroup(group);
                
            }
        }
        public static bool PreconditionMagmaRain(object data)
        {            
            return Game.Instance.savedInfo.discoveredSurface && GameClock.Instance.GetCycle() > 150;
        }
        public static void MagmaRain(object data) 
        {
            WorldContainer startworld = ClusterManager.Instance.GetStartWorld();
            TwitchMagmaRain.StatesInstance smi = startworld.GetComponent<TwitchMagmaRain>().smi;                        
            smi.master.TimeRemaining = Config.Instance.TwitchMagmaRainDuration;
            float x = (startworld.minimumBounds.x + startworld.maximumBounds.x) / 2;
            float y = (startworld.maximumBounds.y - 50);
            UnityEngine.Vector3 position = new UnityEngine.Vector3(x, y, 0);            
            //CameraController.Instance.SetTargetPos(position,50f, true);
            ToastManager.InstantiateToastWithPosTarget("Magma Rain", "2000 degree Magma is raining down at a rate of "+ (Config.Instance.TwitchMagmaRainMass * 5).ToString() +"Kg/s for the next "+ (Config.Instance.TwitchMagmaRainDuration).ToString() + " seconds", position, 50f);

        }
        public static bool PreconditionNuclearWasteRain(object data)
        {
            return Game.Instance.savedInfo.discoveredSurface && GameClock.Instance.GetCycle() > 100;
        }
        public static void NuclearWasteRain(object data)
        {
            WorldContainer startworld = ClusterManager.Instance.GetStartWorld();
            TwitchNuclearWasteRain.StatesInstance smi = startworld.GetComponent<TwitchNuclearWasteRain>().smi;
            smi.master.TimeRemaining = Config.Instance.TwitchNuclearWasteRainDuration;
            float x = (startworld.minimumBounds.x + startworld.maximumBounds.x) / 2;
            float y = (startworld.maximumBounds.y - 50);
            UnityEngine.Vector3 position = new UnityEngine.Vector3(x, y, 0);
            //CameraController.Instance.SetTargetPos(position, 50f, true);
            ToastManager.InstantiateToastWithPosTarget("Nuclear Waste Rain", "500 degree Nuclear Waste is raining down at a rate of "+ (Config.Instance.TwitchNuclearWasteRainMass * 5).ToString() +"Kg/s for the next "+ (Config.Instance.TwitchNuclearWasteRainDuration).ToString() + " seconds", position, 50f);


        }
        public static bool PreconditionZoologicalMeteors(object data)
        {
            return Game.Instance.savedInfo.discoveredSurface && GameClock.Instance.GetCycle() > 50;
        }
        public static void ZoologicalMeteors(object data)
        {
            WorldContainer startworld = ClusterManager.Instance.GetStartWorld();
            TwitchZoologicalMeteors.StatesInstance smi = startworld.GetComponent<TwitchZoologicalMeteors>().smi;
            smi.master.TimeRemaining = Config.Instance.TwitchZoologicalDuration;
            float x = (startworld.minimumBounds.x + startworld.maximumBounds.x) / 2;
            float y = (startworld.maximumBounds.y - 50);
            UnityEngine.Vector3 position = new UnityEngine.Vector3(x, y, 0);
            //CameraController.Instance.SetTargetPos(position, 50f, true);
            ToastManager.InstantiateToastWithPosTarget("Zoological Meteors", "Random Critters are raining down from the heavens at a rate of 1 per second for the next " + (Config.Instance.TwitchZoologicalDuration).ToString() + " seconds", position, 50f);

        }
        public static bool PreconditionWaterBalloons(object data)
        {
            return Game.Instance.savedInfo.discoveredSurface && GameClock.Instance.GetCycle() > 50;
        }
        public static void WaterBalloons(object data) 
        {
            WorldContainer startworld = ClusterManager.Instance.GetStartWorld();
            TwitchWaterBalloons.StatesInstance smi = startworld.GetComponent<TwitchWaterBalloons>().smi;
            smi.master.TimeRemaining = Config.Instance.TwitchWaterBalloonDuration;
            float x = (startworld.minimumBounds.x + startworld.maximumBounds.x) / 2;
            float y = (startworld.maximumBounds.y - 50);
            UnityEngine.Vector3 position = new UnityEngine.Vector3(x, y, 0);
            //CameraController.Instance.SetTargetPos(position, 50f, true);
            ToastManager.InstantiateToastWithPosTarget("Water Balloons", "Water Balloons are being tossed down from on high! Be careful, they might be full of fish!", position, 50f);

        }
        public static bool PreconditionMoltenSlugs(object data)
        {
            return Game.Instance.savedInfo.discoveredSurface && GameClock.Instance.GetCycle() > 150;
        }
        public static void MoltenSlugs(object data) 
        {
            WorldContainer startworld = ClusterManager.Instance.GetStartWorld();
            TwitchMoltenSpongeSlugs.StatesInstance smi = startworld.GetComponent<TwitchMoltenSpongeSlugs>().smi;
            smi.master.TimeRemaining = Config.Instance.TwitchSlugShowerDuration;
            float x = (startworld.minimumBounds.x + startworld.maximumBounds.x) / 2;
            float y = (startworld.maximumBounds.y - 50);
            UnityEngine.Vector3 position = new UnityEngine.Vector3(x, y, 0);
            //CameraController.Instance.SetTargetPos(position, 50f, true);
            ToastManager.InstantiateToastWithPosTarget("Molten Slugs", "Sponge Slugs filled with Tons of Molten Liquids have decided to make your asteroid their home! Better contain them quickly...", position, 50f);

        }
        
    }
}
