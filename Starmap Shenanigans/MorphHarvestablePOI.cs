using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Starmap_Shenanigans
{
    public class MorphHarvestablePOI : KMonoBehaviour, FewOptionSideScreen.IFewOptionSideScreen, ISidescreenButtonControl
    {
        [MyCmpGet]
        public HarvestablePOIClusterGridEntity POIClusterEntity;
        public static FieldInfo _poiTypes = AccessTools.Field(typeof(HarvestablePOIConfigurator), "_poiTypes");
        public static List<string> POITypeIds;
        public Tag selectedOption;
        public static List<HarvestablePOIConfig.HarvestablePOIParams> harvestablePOIParams = new List<HarvestablePOIConfig.HarvestablePOIParams>();
                                
        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (POITypeIds == null)
            {
                POITypeIds = new List<string>();
                var poiTypes = (List<HarvestablePOIConfigurator.HarvestablePOIType>)_poiTypes.GetValue(null);
                for (int i = 0; i < poiTypes.Count; i++)
                {
                    var poi = poiTypes[i];
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
                var poiParam = harvestablePOIParams[i];
                var animFile = POIClusterEntity.AnimConfigs[0].animFile;                                                                                           
                string tooltipText = "";                
                var elementWeightPairList = new List<KeyValuePair<SimHashes, float>>();
                var totalWeight = 0f;
                foreach (var elementWeightPair in poiParam.poiType.harvestableElements)
                {
                    totalWeight += elementWeightPair.Value;
                    elementWeightPairList.Add(elementWeightPair);
                }
                elementWeightPairList.Sort((a, b) => b.Value.CompareTo(a.Value));
                foreach(var elementWeightPair in elementWeightPairList)
                {
                    tooltipText += ElementLoader.GetElement(elementWeightPair.Key.CreateTag()).name + " " + GameUtil.GetFormattedPercent(elementWeightPair.Value / totalWeight * 100) + "\n";
                }                
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
        public string SidescreenButtonTooltip { get => "Replaces the current POI with one selected from below."; }
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
        public bool SidescreenEnabled() => DebugHandler.enabled;
        public bool SidescreenButtonInteractable() => DebugHandler.enabled;
        public void OnSidescreenButtonPressed()
        {            
            if (selectedOption == null)
                return;
            var prefab = Assets.TryGetPrefab(selectedOption);
            if (prefab == null)
                return;
            var go = Util.KInstantiate(prefab);
            selectedOption = null;
            if (go.TryGetComponent(out HarvestablePOIClusterGridEntity ent))
                ent.Init(POIClusterEntity.Location);
            go.SetActive(true);
            gameObject.DeleteObject();
        }
        public int HorizontalGroupID() => -1;
        public int ButtonSideScreenSortOrder() => 1;
    }

    public class DuplicateHarvestablePOI : KMonoBehaviour, ISidescreenButtonControl
    {
        [MyCmpGet]
        MorphHarvestablePOI MorphHarvestablePOI;
        public string SidescreenButtonText { get => "Duplicate"; }
        public string SidescreenButtonTooltip { get => "Spawns a new instance of this POI at this location"; }
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
        public bool SidescreenEnabled() => DebugHandler.enabled;
        public bool SidescreenButtonInteractable() => DebugHandler.enabled;
        public void OnSidescreenButtonPressed()
        {
            var go = Util.KInstantiate(Assets.GetPrefab(MorphHarvestablePOI.POIClusterEntity.name));
            if (go.TryGetComponent(out HarvestablePOIClusterGridEntity ent))
                ent.Init(MorphHarvestablePOI.POIClusterEntity.Location);
            go.SetActive(true);
        }
        public int HorizontalGroupID() => -1;
        public int ButtonSideScreenSortOrder() => 1;
    }
}
