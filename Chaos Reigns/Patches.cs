using Database;
using HarmonyLib;
using Klei.AI;
using TUNING;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProcGenGame;


namespace Chaos_Reigns
{
    public class Patches
    {
        

        [HarmonyPatch(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToFertileCreature), new Type[]
        {            
            typeof(GameObject),
            typeof(IHasDlcRestrictions),
            typeof(string),
            typeof(string) ,
            typeof(string) ,
            typeof(string) ,
            typeof(float) ,
            typeof(string) ,
            typeof(float ),
            typeof(float) ,
            typeof(List<FertilityMonitor.BreedingChance>),                        
            typeof(int) ,
            typeof(bool ),
            typeof(bool ),
            typeof(float ),
            typeof(bool )
        })]

        public class EntityTemplates_ExtendEntityToFertileCreature_Patch
        {
            public static void Postfix(ref GameObject __result, IHasDlcRestrictions dlcRestrictions)
            {
                string[] requiredDlcOrNull = null;
                if (dlcRestrictions != null)
                    requiredDlcOrNull = dlcRestrictions.GetRequiredDlcIds();

                


                //Debug.Log(__result.PrefabID());
                //Debug.Log(__result.PrefabID() +" "+ DlcManager.IsAllContentSubscribed(requiredDlcOrNull) + " " +__result.HasTag(GameTags.OriginalCreature));
                
                if (!DlcManager.IsAllContentSubscribed(requiredDlcOrNull))
                    return;
                if ((Config.Instance.ZoologicalIncludeMorphs == false) && !__result.HasTag(GameTags.OriginalCreature))
                    return;
                if ((Config.Instance.ZoologicalIncludeMorphs == false) && (__result.PrefabID() == "GoldBelly"))
                    return;

                ZoologicalComet.critters.Add(__result.PrefabID().ToString());
                //Debug.Log("Added "+ __result.PrefabID().ToString()+ZoologicalComet.critters.Count);
                
            }
        }
        [HarmonyPatch(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToFertileCreature), new Type[]
        {
            typeof(GameObject),
            typeof(IHasDlcRestrictions),
            typeof(string),
            typeof(string) ,
            typeof(string) ,
            typeof(string) ,
            typeof(float) ,
            typeof(string) ,
            typeof(float ),
            typeof(float) ,
            typeof(List<FertilityMonitor.BreedingChance>),
            typeof(int) ,
            typeof(bool ),
            typeof(bool ),
            typeof(float ),
            typeof(bool )
        })]

        public class EntityTemplates_ExtendEntityToFertileCreature_Patch2
        {
            public static void Postfix(ref GameObject __result, IHasDlcRestrictions dlcRestrictions)
            {
                string[] requiredDlcOrNull = null;
                if (dlcRestrictions != null)
                    requiredDlcOrNull = dlcRestrictions.GetRequiredDlcIds();
                //Debug.Log(__result.PrefabID());
                //Debug.Log(__result.PrefabID() +" "+ DlcManager.IsAllContentSubscribed(dlcIds) + " " +__result.HasTag(GameTags.OriginalCreature));

                if (!DlcManager.IsAllContentSubscribed(requiredDlcOrNull))
                    return;
                if ((Config.Instance.TwitchZoologicalIncludeMorphs == false) && !__result.HasTag(GameTags.OriginalCreature))
                    return;
                if ((Config.Instance.TwitchZoologicalIncludeMorphs == false) && (__result.PrefabID() == "GoldBelly"))
                    return;

                TwitchZoologicalComet.critters.Add(__result.PrefabID().ToString());
                //Debug.Log("Twitch Added " + __result.PrefabID().ToString() + TwitchZoologicalComet.critters.Count);

            }
        }

        [HarmonyPatch(typeof(ClusterMapMeteorShowerConfig), nameof(ClusterMapMeteorShowerConfig.CreatePrefabs))]
        public class ClusterMapMeteorShowerConfig_CreatePrefabs_Patch
        {
            public static void Postfix(ref List<GameObject> __result)
            {
                if (Config.Instance.EnableMoltenSlugs)
                    __result.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("SpongeSlug", "SpongeSlugMeteorEvent", "SpongeSlug Meteor Shower", "Desc", "caterpillar_kanim", initial_anim: "wtr_ui", requiredDlcIds: DlcManager.EXPANSION1, forbiddenDlcIds: null));
                if (Config.Instance.EnableZoological)
                    __result.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Zoological", "ZoologicalMeteorEvent", "Zoological Meteor Shower", "Desc", "shower_question_mark_kanim", initial_anim: "ui", requiredDlcIds: DlcManager.EXPANSION1, forbiddenDlcIds: null));
                if (Config.Instance.EnableWaterBalloons)
                    __result.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("WaterBalloon", "WaterBalloonMeteorEvent", "Water Balloon Meteor Shower", "Desc", "balloon_basic_red_kanim", initial_anim: "floor_floor_1_0_loop", requiredDlcIds: DlcManager.EXPANSION1, forbiddenDlcIds: null));
            }
        }


        [HarmonyPatch(typeof(GameplaySeasons), "Expansion1Seasons")]
        public class GameplaySeasons_Expansion1Seasons_Patch
        {
            public static void Postfix(GameplaySeasons __instance)
            {
                
                if (Config.Instance.EnableMoltenSlugs)    
                    SpongeSlugMeteorShowers = __instance.Add(new MeteorShowerSeason("SpongeSlugMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", Config.Instance.SlugPeriod, false, startActive: true, affectedByDifficultySettings: false, clusterTravelDuration: Config.Instance.TravelDuration, numEventsToStartEachPeriod: 1)
                        .AddEvent(SpongeSlugMeteorEvent));
                if (Config.Instance.EnableZoological)    
                    ZoologicalMeteorShowers = __instance.Add(new MeteorShowerSeason("ZoologicalMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", Config.Instance.ZoologicalPeriod, false, startActive: true, affectedByDifficultySettings: false, clusterTravelDuration: Config.Instance.TravelDuration, numEventsToStartEachPeriod: 1)
                        .AddEvent(ZoologicalMeteorEvent));
                if (Config.Instance.EnableWaterBalloons)    
                    WaterBalloonMeteorShowers = __instance.Add(new MeteorShowerSeason("WaterBalloonMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", Config.Instance.WaterBalloonPeriod, false, startActive: true, affectedByDifficultySettings: false, clusterTravelDuration: Config.Instance.TravelDuration, numEventsToStartEachPeriod: 1)
                        .AddEvent(WaterBalloonMeteorEvent));
            }
        }

        [HarmonyPatch(typeof(GameplayEvents), "Expansion1MeteorEvents")]
        public class GameplayEvents_Expansion1MeteorEvents_Patch
        {
            public static void Postfix(GameplayEvents __instance)
            {
                if (Config.Instance.EnableMoltenSlugs)
                {
                    string fullId = ClusterMapMeteorShowerConfig.GetFullID("SpongeSlug");
                    SpongeSlugMeteorEvent = __instance.Add(new MeteorShowerEvent("SpongeSlugMeteorEvent", Config.Instance.SlugShowerDuration, (float)Config.Instance.SlugShowerDuration / Config.Instance.SlugAvgMeteors, METEORS.BOMBARDMENT_OFF.NONE, secondsBombardmentOn: new MathUtil.MinMax(Config.Instance.SlugShowerDuration, Config.Instance.SlugShowerDuration), fullId, false)
                        .AddMeteor(SpongeSlugCometConfig.ID, 1f));
                }
                if (Config.Instance.EnableZoological)
                {
                    string fullId2 = ClusterMapMeteorShowerConfig.GetFullID("Zoological");
                    ZoologicalMeteorEvent = __instance.Add(new MeteorShowerEvent("ZoologicalMeteorEvent", Config.Instance.ZoologicalDuration, (float)Config.Instance.ZoologicalDuration / Config.Instance.ZoologicalAvgMeteors, METEORS.BOMBARDMENT_OFF.NONE, secondsBombardmentOn: new MathUtil.MinMax(Config.Instance.ZoologicalDuration, Config.Instance.ZoologicalDuration), fullId2, false)
                        .AddMeteor(ZoologicalCometConfig.ID, 1f));
                }
                if (Config.Instance.EnableWaterBalloons)
                {
                    string fullId3 = ClusterMapMeteorShowerConfig.GetFullID("WaterBalloon");
                    WaterBalloonMeteorEvent = __instance.Add(new MeteorShowerEvent("WaterBalloonMeteorEvent", Config.Instance.WaterBalloonDuration, (float)Config.Instance.WaterBalloonDuration / Config.Instance.WaterBalloonAvgMeteors, METEORS.BOMBARDMENT_OFF.NONE, secondsBombardmentOn: new MathUtil.MinMax(Config.Instance.WaterBalloonDuration, Config.Instance.WaterBalloonDuration), fullId3, false)
                        .AddMeteor(WaterBalloonCometConfig.ID, 1f));
                }
            }
        }

        public static GameplayEvent SpongeSlugMeteorEvent { get; set; }
        public static GameplaySeason SpongeSlugMeteorShowers { get; set; }
        public static GameplayEvent ZoologicalMeteorEvent { get; set; }
        public static GameplaySeason ZoologicalMeteorShowers { get; set;}
        public static GameplayEvent WaterBalloonMeteorEvent { get; set; }
        public static GameplaySeason WaterBalloonMeteorShowers { get;set; }

        
        [HarmonyPatch(typeof(WorldContainer), nameof(WorldContainer.SetWorldDetails))]
        public class WorldContainer_SetWorldDetails
        {
            public static void Postfix(ref WorldGen world, ref List<string> ___m_seasonIds, WorldContainer __instance)
            {
                /*if (Config.Instance.TwitchIntegrationMode)
                    return;*/
                //Debug.Log(__instance.id + __instance.name);
                if (world != null)
                {                    
                    //Debug.Log(__instance.id + " ! " + __instance.name + " ! " + __instance.overrideName + " ! " + __instance.worldName + " !" + __instance.worldType);                    
                    if (!Config.Instance.UseYAMLorCGM)
                    {
                        if (Config.Instance.EnableZoological)
                        {
                            if (__instance.worldName == "expansion1::worlds/MarshyMoonlet")
                                ___m_seasonIds.Add(ZoologicalMeteorShowers.Id);
                        }

                        if (Config.Instance.EnableMoltenSlugs)
                        {
                            if (__instance.worldName == "expansion1::worlds/NiobiumMoonlet")
                                ___m_seasonIds.Add(SpongeSlugMeteorShowers.Id);
                        }
                        if (Config.Instance.EnableWaterBalloons)
                        {
                            if (__instance.worldName == "expansion1::worlds/WaterMoonlet")
                                ___m_seasonIds.Add(WaterBalloonMeteorShowers.Id);
                        }
                    } 
                    else
                    {
                        SaveGlobal saveGlobal = SaveGame.Instance.GetComponent<SaveGlobal>();
                        saveGlobal.IsYAMLorCGMsave = true;
                    }
                }                   
            }
        }


        [HarmonyPatch(typeof(WorldContainer), nameof(WorldContainer.GetSeasonIds))]
        public class WorldContainer_GetSeasonIds_Patch
        {
            public static void Postfix(ref List<string> __result, WorldContainer __instance)
            {
                /*if (Config.Instance.TwitchIntegrationMode)
                    return;*/
                SaveGlobal cmp = SaveGame.Instance.GetComponent<SaveGlobal>();
                if (cmp.IsYAMLorCGMsave)
                    return;
                if (Config.Instance.AddToExistingSaveOnLoad)
                {
                    if (Config.Instance.EnableZoological && __instance.worldName == "expansion1::worlds/MarshyMoonlet")
                    {
                        if (!__result.Contains(ZoologicalMeteorShowers.Id))
                            __result.Add(ZoologicalMeteorShowers.Id);
                    }
                    if (Config.Instance.EnableMoltenSlugs && __instance.worldName == "expansion1::worlds/NiobiumMoonlet")
                    {
                        if (!__result.Contains(SpongeSlugMeteorShowers.Id))
                            __result.Add(SpongeSlugMeteorShowers.Id);
                    }
                    if (Config.Instance.EnableWaterBalloons && __instance.worldName == "expansion1::worlds/WaterMoonlet")
                    {
                        if (!__result.Contains(WaterBalloonMeteorShowers.Id))
                            __result.Add(WaterBalloonMeteorShowers.Id);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(WorldContainer), "OnPrefabInit")]
        public class WorldContainer_OnPrefabInit_Patch
        {
            public static void Postfix(WorldContainer __instance)
            {     
                /*if (Config.Instance.TwitchIntegrationMode)
                    return;*/
                if (Config.Instance.EnableRain)
                {
                    if (!__instance.IsModuleInterior)
                        __instance.gameObject.AddComponent<RainMaker>();
                }              
                                            
            }
        }
    }
}
