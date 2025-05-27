using HarmonyLib;
using Klei;
using Klei.CustomSettings;
using KMod;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSerialization;
using ProcGenGame;

namespace Grid_Resize_On_Load
{
    public class IncreaseGridOnLoad
    {
        [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Load), new Type[] { typeof(IReader) })]
        public class SaveLoader_Load_Patch
        {
            public static bool Prefix(IReader reader, SaveLoader __instance, ref bool __result)
            {
                Debug.Assert(reader.ReadKleiString() == "world");
                Deserializer deserializer = new Deserializer(reader);
                SaveFileRoot saveFileRoot = new SaveFileRoot();
                deserializer.Deserialize((object)saveFileRoot);
                if ((__instance.GameInfo.saveMajorVersion == 7 || __instance.GameInfo.saveMinorVersion < 8) && saveFileRoot.requiredMods != null)
                {
                    saveFileRoot.active_mods = new List<KMod.Label>();
                    foreach (ModInfo requiredMod in saveFileRoot.requiredMods)
                        saveFileRoot.active_mods.Add(new KMod.Label()
                        {
                            id = requiredMod.assetID,
                            version = (long)requiredMod.lastModifiedTime,
                            distribution_platform = KMod.Label.DistributionPlatform.Steam,
                            title = requiredMod.description
                        });
                    saveFileRoot.requiredMods.Clear();
                }
                KMod.Manager modManager = Global.Instance.modManager;
                modManager.Load(Content.LayerableFiles);
                if (!modManager.MatchFootprint(saveFileRoot.active_mods, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
                    DebugUtil.LogWarningArgs((object)"Mod footprint of save file doesn't match current mod configuration");
                string str = string.Format("Mod Footprint ({0}):", (object)saveFileRoot.active_mods.Count);
                foreach (KMod.Label activeMod in saveFileRoot.active_mods)
                    str = str + "\n  - " + activeMod.title;
                Debug.Log((object)str);
                __instance.LogActiveMods();
                Global.Instance.modManager.SendMetricsEvent();
                WorldGen.LoadSettings();
                CustomGameSettings.Instance.LoadClusters();
                if (__instance.GameInfo.clusterId == null)
                {
                    SaveGame.GameInfo gameInfo = __instance.GameInfo;
                    if (!string.IsNullOrEmpty(saveFileRoot.clusterID))
                    {
                        gameInfo.clusterId = saveFileRoot.clusterID;
                    }
                    else
                    {
                        try
                        {
                            gameInfo.clusterId = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig)CustomGameSettingConfigs.ClusterLayout).id;
                        }
                        catch
                        {
                            gameInfo.clusterId = WorldGenSettings.ClusterDefaultName;
                            CustomGameSettings.Instance.SetQualitySetting((SettingConfig)CustomGameSettingConfigs.ClusterLayout, gameInfo.clusterId);
                        }
                    }
                    __instance.GameInfo = gameInfo;
                }
                Game.clusterId = __instance.GameInfo.clusterId;
                Game.LoadSettings(deserializer);

                //GridSettings.Reset(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells);
                GridSettings.Reset(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells + 64);


                if (Application.isPlaying)
                    Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
                Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
                SimMessages.CreateSimElementsTable(ElementLoader.elements);

                //Sim.AllocateCells(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells);
                Sim.AllocateCells(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells + 64);

                SimMessages.CreateDiseaseTable(Db.Get().Diseases);
                Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, (byte[])null);
                if (Sim.LoadWorld(!saveFileRoot.streamed.ContainsKey("Sim") ? reader : (IReader)new FastReader(saveFileRoot.streamed["Sim"])) != 0)
                {
                    DebugUtil.LogWarningArgs((object)"\n--- Error loading save ---\nSimDLL found bad data\n");
                    Sim.Shutdown();
                    __result = false;
                }
                Sim.Start();
                SceneInitializer.Instance.PostLoadPrefabs();
                __instance.mustRestartOnFail = true;
                if (!__instance.saveManager.Load(reader))
                {
                    Sim.Shutdown();
                    DebugUtil.LogWarningArgs((object)"\n--- Error loading save ---\n");
                    SaveLoader.SetActiveSaveFilePath((string)null);
                    __result = false;
                }
                Grid.Visible = saveFileRoot.streamed["GridVisible"];
                Array.Resize(ref Grid.Visible, Grid.Visible.Length + (saveFileRoot.WidthInCells * 64));
                if (saveFileRoot.streamed.ContainsKey("GridSpawnable"))
                {
                    Grid.Spawnable = saveFileRoot.streamed["GridSpawnable"];
                    Array.Resize(ref Grid.Spawnable, Grid.Spawnable.Length + (saveFileRoot.WidthInCells * 64));
                }

                Grid.Damage = __instance.BytesToFloat(saveFileRoot.streamed["GridDamage"]);
                Array.Resize(ref Grid.Damage, Grid.Damage.Length + (saveFileRoot.WidthInCells * 64));

                Game.Instance.Load(deserializer);
                CameraSaveData.Load(new FastReader(saveFileRoot.streamed["Camera"]));
                ClusterManager.Instance.InitializeWorldGrid();
                SimMessages.DefineWorldOffsets(ClusterManager.Instance.WorldContainers.Select<WorldContainer, SimMessages.WorldOffsetData>((Func<WorldContainer, SimMessages.WorldOffsetData>)(container => new SimMessages.WorldOffsetData()
                {
                    worldOffsetX = container.WorldOffset.x,
                    worldOffsetY = container.WorldOffset.y,
                    worldSizeX = container.WorldSize.x,
                    worldSizeY = container.WorldSize.y
                })).ToList<SimMessages.WorldOffsetData>());
                __result = true;


                return false;
            }
        }

    }
}
