using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;

namespace Starmap_Shenanigans
{
    public class DestroyMeteorButton : KMonoBehaviour, ISidescreenButtonControl
    {
        [MySmiReq]
        ClusterMapMeteorShower.Instance Instance;
        public string SidescreenButtonText { get => "DESTROY"; }

        public string SidescreenButtonTooltip { get => "Deletes this meteor from existence."; }

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

        public bool SidescreenEnabled() => DebugHandler.enabled;

        public bool SidescreenButtonInteractable() => DebugHandler.enabled;



        public void OnSidescreenButtonPressed()
        {
            var meteorShowerEvents = new List<GameplayEventInstance>();
            GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(Instance.DestinationWorldID, ref meteorShowerEvents);
            foreach(GameplayEventInstance evt in meteorShowerEvents)
                GameplayEventManager.Instance.RemoveActiveEvent(evt);
            gameObject.DeleteObject();
        }

        public int HorizontalGroupID() => -1;

        public int ButtonSideScreenSortOrder() => 1;

        [HarmonyPatch(typeof(ClusterGridEntity), "OnSpawn")]
        public class ClusterGridEntity_OnSpawn_Patch
        {
            public static void Postfix(ClusterGridEntity __instance)
            {
                //if (!DebugHandler.enabled)
                    //return;
                if (__instance.TryGetComponent(out ClusterMapMeteorShowerVisualizer _))
                {
                    __instance.FindOrAddComponent<DestroyMeteorButton>();

                }
            }
        }
    }
}

