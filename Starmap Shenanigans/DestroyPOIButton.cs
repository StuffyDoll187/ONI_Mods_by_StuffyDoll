using HarmonyLib;

namespace Starmap_Shenanigans
{
    class DestroyPOIButton : KMonoBehaviour, ISidescreenButtonControl
    {
        public string SidescreenButtonText { get => "DESTROY"; }
        public string SidescreenButtonTooltip { get => DebugHandler.enabled ? "Deletes this object from existence." : "Must Enable Debug Mode"; }
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
        public bool SidescreenEnabled() => DebugHandler.enabled;
        public bool SidescreenButtonInteractable() => DebugHandler.enabled;
        public void OnSidescreenButtonPressed()
        {
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
                   // return;
                if (__instance.TryGetComponent(out HarvestablePOIClusterGridEntity _) || __instance.TryGetComponent(out ArtifactPOIClusterGridEntity _) )// || __instance.TryGetComponent(out ClusterMapMeteorShowerVisualizer _))
                {
                    __instance.FindOrAddComponent<DestroyPOIButton>();                
                }
            }
        }
    }
}
