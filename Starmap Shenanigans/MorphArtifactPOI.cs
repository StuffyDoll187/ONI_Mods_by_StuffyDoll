using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Starmap_Shenanigans
{
    public class MorphArtifactPOI : KMonoBehaviour, FewOptionSideScreen.IFewOptionSideScreen, ISidescreenButtonControl
    {
        [MyCmpGet]
        public ArtifactPOIClusterGridEntity POIClusterEntity;
        public static FieldInfo _poiTypes = AccessTools.Field(typeof(ArtifactPOIConfigurator), "_poiTypes");
        public static List<string> POITypeIds;
        public Tag selectedOption;
        public static List<ArtifactPOIConfig.ArtifactPOIParams> artifactPOIParams = new List<ArtifactPOIConfig.ArtifactPOIParams>();
        
        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (POITypeIds == null)
            {
                POITypeIds = new List<string>();
                var poiTypes = (List<ArtifactPOIConfigurator.ArtifactPOIType>)_poiTypes.GetValue(null);
                string doesNotExist = ArtifactPOIConfigurator.defaultArtifactPoiType.id;
                for (int i = 0; i < poiTypes.Count; i++)
                {
                    var poi = poiTypes[i];
                    if (poi.id == doesNotExist)
                        continue;
                    if (poi.requiredDlcIds != null && DlcManager.IsAllContentSubscribed(poi.requiredDlcIds))
                        POITypeIds.Add(poi.id);
                }                
            }
        }
        public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
        {                        
            FewOptionSideScreen.IFewOptionSideScreen.Option[] options = new FewOptionSideScreen.IFewOptionSideScreen.Option[POITypeIds.Count];
            for (int i = 0; i < options.Length; ++i)
            {
                var poiParam = artifactPOIParams[i];
                var animFile = POIClusterEntity.AnimConfigs[0].animFile;
                string tooltipText = "";                
                options[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(
                    tag: poiParam.id,
                    labelText: Strings.Get(poiParam.nameStringKey),
                    iconSpriteColorTuple: new Tuple<Sprite, Color>(Def.GetUISpriteFromMultiObjectAnim(animFile, poiParam.anim), Color.white),
                    tooltipText);
            }
            return options;
        }
        public void OnOptionSelected(
          FewOptionSideScreen.IFewOptionSideScreen.Option option)
        {
            selectedOption = option.tag;
        }
        public Tag GetSelectedOption() => selectedOption;
        public string SidescreenButtonText { get => "Morph!"; }
        public string SidescreenButtonTooltip { get => DebugHandler.enabled ? "Replaces the current POI with one selected from below." : "Must Enable Debug Mode"; }
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
        public bool SidescreenEnabled() => DebugHandler.enabled;
        public bool SidescreenButtonInteractable() => DebugHandler.enabled;
        public void OnSidescreenButtonPressed()
        {
            //Debug.Log(selectedOption);
            if (selectedOption == null)
                return;
            var prefab = Assets.TryGetPrefab(selectedOption);
            if (prefab == null)
                return;
            var go = Util.KInstantiate(prefab);            
            selectedOption = null;
            if (go.TryGetComponent(out ArtifactPOIClusterGridEntity ent))
                ent.Init(POIClusterEntity.Location);
            go.SetActive(true);
            gameObject.DeleteObject();
        }
        public int HorizontalGroupID() => -1;
        public int ButtonSideScreenSortOrder() => 1;
    }

    public class DuplicateArtifactPOI : KMonoBehaviour, ISidescreenButtonControl
    {
        [MyCmpGet]
        MorphArtifactPOI MorphArtifactPOI;
        public string SidescreenButtonText { get => "Duplicate"; }
        public string SidescreenButtonTooltip { get => DebugHandler.enabled ? "Spawns a new instance of this POI at this location" : "Must Enable Debug Mode"; }
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
        public bool SidescreenEnabled() => DebugHandler.enabled;
        public bool SidescreenButtonInteractable() => DebugHandler.enabled;
        public void OnSidescreenButtonPressed()
        {
            var go = Util.KInstantiate(Assets.GetPrefab(MorphArtifactPOI.POIClusterEntity.name));
            if (go.TryGetComponent(out ArtifactPOIClusterGridEntity ent))
                ent.Init(MorphArtifactPOI.POIClusterEntity.Location);
            go.SetActive(true);
        }
        public int HorizontalGroupID() => -1;
        public int ButtonSideScreenSortOrder() => 1;
    }

}
