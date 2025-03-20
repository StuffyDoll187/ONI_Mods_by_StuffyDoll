using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Upgradeable_Dupes_And_Critters
{
    


public class GravitasCreatureManipulator :
  GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>
    {
        public State inoperational;
        public ActiveStates operational;
        public TargetParameter creatureTarget;
        public FloatParameter cooldownTimer;
        public FloatParameter workingTimer;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = inoperational;
            this.serializable = SerializeType.ParamsOnly;
            //Enter((StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State.Callback)(smi => smi.DropCritter())).
            this.root.Enter(smi => smi.DropCritter()).Enter((smi => smi.UpdateMeter())).EventHandler(GameHashes.BuildingActivated, (smi, activated) =>
            {
                if (!(bool)activated)
                    return;
                StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
            });
            this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.operational.awaitingvial, (smi => smi.GetComponent<Operational>().IsOperational));
            this.operational.DefaultState(this.operational.awaitingvial).EventTransition(GameHashes.OperationalChanged, this.inoperational, (smi => !smi.GetComponent<Operational>().IsOperational));
            this.operational.awaitingvial.Transition(this.operational.idle, (smi => HasVial(smi)), UpdateRate.SIM_4000ms);
            //this.operational.awaitingvial.Enter((StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State.Callback)(smi => Debug.Log("Enter Awaiting Vial")));
            this.operational.idle.PlayAnim("idle", KAnim.PlayMode.Loop)
                .Enter(new StateMachine<GravitasCreatureManipulator, Instance, IStateMachineTarget, Def>.State.Callback(CheckForCritter))
                .ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWaiting)
                .ParamTransition<GameObject>(this.creatureTarget, this.operational.capture, ((smi, p) => p != null && !smi.IsCritterStored))
                .ParamTransition<GameObject>(this.creatureTarget, this.operational.working.pre, ((smi, p) => p != null && smi.IsCritterStored))
                .ParamTransition<float>(this.cooldownTimer, this.operational.cooldown, IsGTZero);
            //this.operational.idle.Enter((StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State.Callback)(smi => Debug.Log("Enter Idle")));
            this.operational.capture.PlayAnim("working_capture").OnAnimQueueComplete(this.operational.working.pre);
            this.operational.working.DefaultState(this.operational.working.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWorking);
            double num1;
            this.operational.working.pre.PlayAnim("working_pre")
                .OnAnimQueueComplete(this.operational.working.loop)
                .Enter((smi => smi.StoreCreature()))
                .Exit((smi => num1 = (double)smi.sm.workingTimer.Set(smi.def.workingDuration, smi)))
                .OnTargetLost(this.creatureTarget, this.operational.awaitingvial)
                .Target(this.creatureTarget).ToggleStationaryIdling();
            double num2;
            this.operational.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(((smi, dt) => num2 = (double)smi.sm.workingTimer.DeltaClamp(-dt, 0.0f, float.MaxValue, smi)), UpdateRate.SIM_1000ms).ParamTransition<float>(this.workingTimer, this.operational.working.pst, IsLTEZero).OnTargetLost(this.creatureTarget, this.operational.awaitingvial);
            this.operational.working.pst.PlayAnim("working_pst").Enter(smi => smi.DropCritter()).OnAnimQueueComplete(this.operational.cooldown);
            double num3;
            State state = this.operational.cooldown.PlayAnim("working_cooldown", KAnim.PlayMode.Loop).Update((smi, dt) => num3 = (double)smi.sm.cooldownTimer.DeltaClamp(-dt, 0.0f, float.MaxValue, smi), UpdateRate.SIM_1000ms).ParamTransition<float>(this.cooldownTimer, this.operational.awaitingvial, IsLTEZero);
            string name = (string)CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.NAME;
            string tooltip = (string)CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.TOOLTIP;
            HashedString render_overlay = new HashedString();
            StatusItemCategory main = Db.Get().StatusItemCategories.Main;
            Func<string, Instance, string> resolve_string_callback = new Func<string, Instance, string>(Processing);
            Func<string, Instance, string> resolve_tooltip_callback = new Func<string, Instance, string>(ProcessingTooltip);
            StatusItemCategory category = main;
            state.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, resolve_string_callback: resolve_string_callback, resolve_tooltip_callback: resolve_tooltip_callback, category: category);
        }

        private static bool HasVial(Instance smi)
        {
            Storage storage = smi.master.GetComponent<Storage>();
            //Debug.Log(smi.master.name + !storage.IsEmpty());
            //Debug.Log(storage.Has("RadioactiveSerum"));
            return storage.Has("RadioactiveSerum");
        }
        private static string Processing(string str, Instance smi)
        {
            return str.Replace("{percent}", GameUtil.GetFormattedPercent((float)((1.0 - (double)smi.sm.cooldownTimer.Get(smi) / (double)smi.def.cooldownDuration) * 100.0)));
        }

        private static string ProcessingTooltip(string str, Instance smi)
        {
            return str.Replace("{timeleft}", GameUtil.GetFormattedTime(smi.sm.cooldownTimer.Get(smi)));
        }

        private static void CheckForCritter(Instance smi)
        {
            if (!smi.sm.creatureTarget.IsNull(smi))
                return;
            GameObject gameObject1 = Grid.Objects[smi.pickupCell, 3];
            if (!(gameObject1 != null))
                return;
            ObjectLayerListItem objectLayerListItem = gameObject1.GetComponent<Pickupable>().objectLayerListItem;
            while (objectLayerListItem != null)
            {
                GameObject gameObject2 = objectLayerListItem.gameObject;
                objectLayerListItem = objectLayerListItem.nextItem;
                if (!(gameObject2 == null) && smi.IsAccepted(gameObject2))
                {
                    smi.SetCritterTarget(gameObject2);
                    break;
                }
            }
        }

        public class Def : BaseDef
        {
            public CellOffset pickupOffset;
            public CellOffset dropOffset;
            public int numSpeciesToUnlockMorphMode;
            public float workingDuration;
            public float cooldownDuration;
        }

        public class WorkingStates :
          State
        {
            public State pre;
            public State loop;
            public State pst;
        }

        public class ActiveStates :
          State
        {
            public State awaitingvial;
            public State idle;
            public State capture;
            public WorkingStates working;
            public State cooldown;
        }

        public new class Instance :
          GameInstance
        {
            public int pickupCell;
            [MyCmpGet]
            private Storage m_storage;
            [Serialize]
            public HashSet<Tag> ScannedSpecies = new HashSet<Tag>();
            [Serialize]
            private bool m_introPopupSeen;
            /*[Serialize]
            private bool m_morphModeUnlocked;*/
            private bool m_morphModeUnlocked = true;
            private EventInfoData eventInfo;
            private Notification m_endNotification;
            private MeterController m_progressMeter;
            private HandleVector<int>.Handle m_partitionEntry;
            private HandleVector<int>.Handle m_largeCreaturePartitionEntry;

            public Instance(IStateMachineTarget master, Def def)
              : base(master, def)
            {
                this.pickupCell = Grid.OffsetCell(Grid.PosToCell(master.gameObject), this.smi.def.pickupOffset);
                this.m_partitionEntry = GameScenePartitioner.Instance.Add(nameof(GravitasCreatureManipulator), (object)this.gameObject, this.pickupCell, GameScenePartitioner.Instance.pickupablesChangedLayer, new System.Action<object>(this.DetectCreature));
                this.m_largeCreaturePartitionEntry = GameScenePartitioner.Instance.Add("GravitasCreatureManipulator.large", (object)this.gameObject, Grid.CellLeft(this.pickupCell), GameScenePartitioner.Instance.pickupablesChangedLayer, new System.Action<object>(this.DetectLargeCreature));
                this.m_progressMeter = new MeterController((KAnimControllerBase)this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.TileFront, Array.Empty<string>());
            }

            public override void StartSM()
            {
                base.StartSM();
                this.UpdateStatusItems();
                this.UpdateMeter();
                /*StoryManager.Instance.ForceCreateStory(Db.Get().Stories.CreatureManipulator, this.gameObject.GetMyWorldId());
                if (this.ScannedSpecies.Count >= this.smi.def.numSpeciesToUnlockMorphMode)
                    StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
                this.TryShowCompletedNotification();
                //this.Subscribe(-1503271301, new System.Action<object>(this.OnBuildingSelect));
                StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.CreatureManipulator);*/
            }

            public override void StopSM(string reason)
            {
                //this.Unsubscribe(-1503271301, new System.Action<object>(this.OnBuildingSelect));
                base.StopSM(reason);
            }

            /*private void OnBuildingSelect(object obj)
            {
                if (!(bool)obj)
                    return;
                if (!this.m_introPopupSeen)
                    this.ShowIntroNotification();
                if (this.m_endNotification == null)
                    return;
                this.m_endNotification.customClickCallback(this.m_endNotification.customClickData);
            }*/

            public bool IsMorphMode => this.m_morphModeUnlocked;

            public bool IsCritterStored => this.m_storage.Count > 1;

            private void UpdateStatusItems()
            {
                KSelectable component = this.gameObject.GetComponent<KSelectable>();
                component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorProgress, !this.IsMorphMode, (object)this);
                component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphMode, this.IsMorphMode, (object)this);
                component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphModeLocked, !this.IsMorphMode, (object)this);
            }

            public void UpdateMeter()
            {
                //this.m_progressMeter.SetPositionPercent(Mathf.Clamp01((float)this.ScannedSpecies.Count / (float)this.smi.def.numSpeciesToUnlockMorphMode));
                this.m_progressMeter.SetPositionPercent(1f);
            }

            public bool IsAccepted(GameObject go)
            {
                KPrefabID component = go.GetComponent<KPrefabID>();
                //Debug.Log(component.name);
                //bool flag0 = (component.name == "Glom" || component.name == "Bee" || component.name == "BeeBaby");
                CreatureBrain brain = go.GetComponent<CreatureBrain>();
                bool flag = false;
                if (!(brain == null) && !component.HasTag(GameTags.Robot))

                {
                    bool flag1 = brain.HasTag(GameTags.Creatures.Wild);
                    //Debug.Log((brain.TryGetComponent<CritterUpgradeTracker>(out CritterUpgradeTracker cmp1)));
                    CritterUpgradeTracker cmp = brain.GetComponent<CritterUpgradeTracker>();
                    bool flag2 = cmp.Upgrades < CritterUpgradeTracker.MaxUpgradeLevel;
                    bool flag3 = (component.name == "Glom" || component.name == "Bee" || component.name == "BeeBaby");
                    flag = (!flag1) && flag2 && (!flag3);
                    //Debug.Log(go.name + flag + flag1 + flag2);
                }
                
                return component.HasTag(GameTags.Creature) && !component.HasTag(GameTags.Robot) && component.PrefabTag != GameTags.Creature && flag && !component.HasTag(GameTags.Dead);
            }

            private void DetectLargeCreature(object obj)
            {
                Pickupable pickupable = obj as Pickupable;
                if ((UnityEngine.Object)pickupable == (UnityEngine.Object)null || (double)pickupable.GetComponent<KCollider2D>().bounds.size.x <= 1.5)
                    return;
                this.DetectCreature(obj);
            }

            private void DetectCreature(object obj)
            {
                Pickupable pickupable = obj as Pickupable;
                if (!((UnityEngine.Object)pickupable != (UnityEngine.Object)null) || !this.IsAccepted(pickupable.gameObject) || !this.smi.sm.creatureTarget.IsNull(this.smi) || !this.smi.IsInsideState((StateMachine.BaseState)this.smi.sm.operational.idle))
                    return;
                this.SetCritterTarget(pickupable.gameObject);
            }

            public void SetCritterTarget(GameObject go)
            {
                this.smi.sm.creatureTarget.Set(go.gameObject, this.smi, false);
            }

            public void StoreCreature() => this.m_storage.Store(this.smi.sm.creatureTarget.Get(this.smi));

            public void DropCritter()
            {
                if (!(this.m_storage.Count > 1)) return;
                List<GameObject> collect_dropped_items = new List<GameObject>();
                this.m_storage.DropAll(Grid.CellToPosCBC(Grid.PosToCell(this.smi), Grid.SceneLayer.Creatures), offset: this.smi.def.dropOffset.ToVector3(), collect_dropped_items: collect_dropped_items);
                foreach (GameObject go in collect_dropped_items)
                {                    
                    if (go.name == "RadioactiveSerum") //&& (this.smi.GetCurrentState().name == "root.operational.working.pst"))
                    {                        
                        go.DeleteObject();
                        continue;
                    }
                    CreatureBrain component = go.GetComponent<CreatureBrain>();
                    if (!(component == null))
                    {
                        //Debug.Log(component.HasTag("Upgraded".ToTag()));
                        //this.Scan(component.species);
                        if (this.IsMorphMode)
                            this.UpgradeCritter(component);                        
                        else
                            go.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString)"idle_loop");
                        //Debug.Log(component.HasTag("Upgraded".ToTag()));
                    }
                }
                this.smi.sm.creatureTarget.Set(null, this.smi);
            }

            /*private void Scan(Tag species)
            {
                if (this.ScannedSpecies.Add(species))
                {
                    this.gameObject.Trigger(1980521255);
                    this.UpdateStatusItems();
                    this.UpdateMeter();
                    this.ShowCritterScannedNotification(species);
                }
                this.TryShowCompletedNotification();
            }*/

            public void UpgradeCritter(CreatureBrain brain)
            {
                //brain.AddTag("Upgraded".ToTag());
                CritterUpgradeTracker cmp = brain.GetComponent<CritterUpgradeTracker>();                
                Traits traits = brain.GetComponent<Traits>();
                if (cmp.Upgrades > 0)
                traits.Remove(Db.Get().traits.Get("CritterUpgrade" + cmp.Upgrades.ToString()));
                cmp.Upgrades += 1;
                traits.Add(Db.Get().traits.Get("CritterUpgrade" + cmp.Upgrades.ToString()));
                
                
            }

            /*public void SpawnMorph(Brain brain)
            {
                Tag tag1 = Tag.Invalid;
                BabyMonitor.Instance smi1 = brain.GetSMI<BabyMonitor.Instance>();
                FertilityMonitor.Instance smi2 = brain.GetSMI<FertilityMonitor.Instance>();
                bool flag1 = smi1 != null;
                bool flag2 = smi2 != null;
                if (flag2)
                    tag1 = FertilityMonitor.EggBreedingRoll(smi2.breedingChances, true);
                else if (flag1)
                {
                    FertilityMonitor.Def def = Assets.GetPrefab(smi1.def.adultPrefab).GetDef<FertilityMonitor.Def>();
                    if (def == null)
                        return;
                    tag1 = FertilityMonitor.EggBreedingRoll(def.initialBreedingWeights, true);
                }
                if (!tag1.IsValid)
                    return;
                Tag tag2 = Assets.GetPrefab(tag1).GetDef<IncubationMonitor.Def>().spawnedCreature;
                if (flag2)
                    tag2 = Assets.GetPrefab(tag2).GetDef<BabyMonitor.Def>().adultPrefab;
               *//* Vector3 position = brain.transform.GetPosition() with
                {
                    z = Grid.GetLayerZ(Grid.SceneLayer.Creatures)
                };*//*

                var position1 = new Vector3[1]
                {
                    brain.transform.GetPosition()
                };               
                Vector3 position = position1[0];
                position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);

                GameObject go = Util.KInstantiate(Assets.GetPrefab(tag2), position);
                go.SetActive(true);
                go.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString)"growup_pst");
                foreach (AmountInstance amount in (Modifications<Amount, AmountInstance>)brain.gameObject.GetAmounts())
                {
                    AmountInstance amountInstance = amount.amount.Lookup(go);
                    if (amountInstance != null)
                    {
                        float num = amount.value / amount.GetMax();
                        amountInstance.value = num * amountInstance.GetMax();
                    }
                }
                go.Trigger(-2027483228, (object)brain.gameObject);
                KSelectable component = brain.gameObject.GetComponent<KSelectable>();
                if ((UnityEngine.Object)SelectTool.Instance != (UnityEngine.Object)null && (UnityEngine.Object)SelectTool.Instance.selected != (UnityEngine.Object)null && (UnityEngine.Object)SelectTool.Instance.selected == (UnityEngine.Object)component)
                    SelectTool.Instance.Select(go.GetComponent<KSelectable>());
                double num1 = (double)this.smi.sm.cooldownTimer.Set(this.smi.def.cooldownDuration, this.smi);
                brain.gameObject.DeleteObject();
            }*/

            /*public void ShowIntroNotification()
            {
                Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_initial");
                this.m_introPopupSeen = true;
                EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.NAME, (string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.DESCRIPTION, (string)CODEX.STORY_TRAITS.CLOSE_BUTTON, "crittermanipulatoractivate_kanim", EventInfoDataHelper.PopupType.BEGIN));
            }

            public void ShowCritterScannedNotification(Tag species)
            {
                Game.Instance.unlocks.Unlock(GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(species), false);
                ShowCritterScannedNotificationAndWaitForClick().Then((System.Action)(() => GravitasCreatureManipulator.Instance.ShowLoreUnlockedPopup(species)));

                Promise ShowCritterScannedNotificationAndWaitForClick()
                {
                    return new Promise((System.Action<System.Action>)(resolve =>
                    {
                        Notification notification1 = new Notification((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_NOTIFICATION.NAME, NotificationType.Event, (Func<List<Notification>, object, string>)((notifications, obj) =>
                        {
                            string str = (string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_NOTIFICATION.TOOLTIP;
                            foreach (Notification notification2 in notifications)
                            {
                                string tooltipData = notification2.tooltipData as string;
                                str = str + "\n • " + (string)Strings.Get("STRINGS.CREATURES.FAMILY_PLURAL." + tooltipData);
                            }
                            return str;
                        }), (object)species.ToString().ToUpper(), false, custom_click_callback: (Notification.ClickCallback)(obj => resolve()), clear_on_click: true);
                        this.gameObject.AddOrGet<Notifier>().Add(notification1);
                    }));
                }
            }*/

            /*public static void ShowLoreUnlockedPopup(Tag species)
            {
                InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.NAME).AddDefaultOK();
                int num = CodexCache.GetEntryForLock(GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(species)) != null ? 1 : 0;
                Option<string> contentForSpeciesTag = GravitasCreatureManipulatorConfig.GetBodyContentForSpeciesTag(species);
                if (num != 0 && contentForSpeciesTag.HasValue)
                    infoDialogScreen.AddPlainText(contentForSpeciesTag.Value).AddOption((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITCRITTERMANIPULATOR"));
                else
                    infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
            }*/

            /*public void TryShowCompletedNotification()
            {
                if (this.ScannedSpecies.Count < this.smi.def.numSpeciesToUnlockMorphMode || this.IsMorphMode)
                    return;
                this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData((string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.NAME, (string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.DESCRIPTION, (string)CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.BUTTON, "crittermanipulatormorphmode_kanim", EventInfoDataHelper.PopupType.COMPLETE);
                this.m_endNotification = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.UnlockMorphMode));
                this.gameObject.AddOrGet<Notifier>().Add(this.m_endNotification);
                this.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, (object)this.smi);
            }*/

            /*public void ClearEndNotification()
            {
                this.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
                if (this.m_endNotification != null)
                    this.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
                this.m_endNotification = (Notification)null;
            }*/

            /*public void UnlockMorphMode(object _)
            {
                if (this.m_morphModeUnlocked)
                    return;
                Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_complete");
                if (this.m_endNotification != null)
                    this.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
                this.m_morphModeUnlocked = true;
                this.UpdateStatusItems();
                this.ClearEndNotification();
                Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance)this.smi), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
                StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, this.gameObject.GetComponent<MonoBehaviour>(), new FocusTargetSequence.Data()
                {
                    WorldId = this.smi.GetMyWorldId(),
                    OrthographicSize = 6f,
                    TargetSize = 6f,
                    Target = posCcc,
                    PopupData = this.eventInfo,
                    CompleteCB = new System.Action(this.OnStorySequenceComplete),
                    CanCompleteCB = (Func<bool>)null
                });
            }*/

            /*private void OnStorySequenceComplete()
            {
                Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance)this.smi), new CellOffset(-1, 1)), Grid.SceneLayer.Ore);
                StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, posCcc);
                this.eventInfo = (EventInfoData)null;
            }*/

            protected override void OnCleanUp()
            {
                GameScenePartitioner.Instance.Free(ref this.m_partitionEntry);
                GameScenePartitioner.Instance.Free(ref this.m_largeCreaturePartitionEntry);
                if (this.m_endNotification == null)
                    return;
                this.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
            }
        }
    }




}

