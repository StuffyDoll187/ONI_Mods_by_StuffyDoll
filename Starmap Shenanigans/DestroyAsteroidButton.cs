using HarmonyLib;

namespace Starmap_Shenanigans
{   
    class DestroyAsteroidButton : KMonoBehaviour, ISidescreenButtonControl
    {
        //[MyCmpGet]
        //private AsteroidGridEntity m_asteroid;
        [MyCmpGet]
        private WorldContainer m_worldContainer;

        [HarmonyPatch(typeof(AsteroidGridEntity), "OnSpawn")]
        public class AsteroidGridEntity_OnPrefabInit_Patch
        {
            public static void Postfix(AsteroidGridEntity __instance, WorldContainer ___m_worldContainer)
            {
                if (!DebugHandler.enabled)// || ___m_worldContainer.IsStartWorld)
                    return;
                __instance.FindOrAddComponent<DestroyAsteroidButton>();                
            }
        }
        public string SidescreenButtonText { get => "DESTROY! : Experimental"; }
        public string SidescreenButtonTooltip { get => "Use with extreme caution.\nNot recommended."; }
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
        public bool SidescreenEnabled() => true;
        public bool SidescreenButtonInteractable() => true;        
        public void OnSidescreenButtonPressed()
        {           
            EaterOfWorlds.DestroyWorld(m_worldContainer.id);                        
        }
        public int HorizontalGroupID() => -1;
        public int ButtonSideScreenSortOrder() => 1;

    }
}
