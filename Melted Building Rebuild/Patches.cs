using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Melted_Rebuild
{
    public class Patches
    {

        [HarmonyPatch(typeof(StructureTemperatureComponents), nameof(StructureTemperatureComponents.DoMelt))]
        public class StructureTemperatureComponents_DoMelt_Patch
        {                        

            public static void Prefix(PrimaryElement primary_element)
            {
                if (!primary_element.TryGetComponent(out Building building))
                    return;
                building.TryGetComponent(out Deconstructable deconstructable);
                List<Tag> constructionElementTags = new List<Tag>();
                if (deconstructable != null)
                {
                    for (int i = 0; i < deconstructable.constructionElements.Length; i++)
                        constructionElementTags.Add(deconstructable.constructionElements[i]);
                }
                else
                    constructionElementTags.Add(primary_element.Element.tag);
                string facadeID = null;
                if (building.TryGetComponent(out BuildingFacade buildingFacade))
                    facadeID = buildingFacade.CurrentFacade;
                Vector3 position = building.transform.position;
                Orientation orientation = building.Orientation;
                GameScheduler.Instance.ScheduleNextFrame("", obj => building.Def.TryPlace(null, position, orientation, constructionElementTags, facadeID, false));

            }            
        }     
    }
}
