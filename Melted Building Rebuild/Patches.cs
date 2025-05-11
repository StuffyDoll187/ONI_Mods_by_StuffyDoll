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
            public static Building Building;
            public static Deconstructable Deconstructable;
            public static void Prefix(PrimaryElement primary_element)
            {
                Building = primary_element.GetComponent<Building>();
                if (Building.TryGetComponent(out Deconstructable deconstructable))
                    Deconstructable = deconstructable;
                else
                    Deconstructable = null;
            }
            public static void Postfix(PrimaryElement primary_element)
            {
                var constructionElements = new List<Tag>();                
                if (Deconstructable != null)
                {                   
                    for (int i = 0; i < Deconstructable.constructionElements.Length; i++)
                        constructionElements.Add(Deconstructable.constructionElements[i]);
                }
                else                
                    constructionElements.Add(primary_element.Element.tag);                
                Building.Def.TryPlace(Building.gameObject, Building.transform.position, Building.Orientation, constructionElements);
                Building = null;
                Deconstructable = null;
            }
        }
     
    }
}
