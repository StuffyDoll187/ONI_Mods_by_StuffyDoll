using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            public static List<Tag> ConstructionElements = new List<Tag>();
            public static string CurrentFacade;

            public static void Prefix(PrimaryElement primary_element)
            {
                if (primary_element.TryGetComponent(out Building))
                    Building.TryGetComponent(out Deconstructable);

            }
            public static void Postfix(PrimaryElement primary_element)
            {
                if (Building == null)
                    return;
                ConstructionElements.Clear();
                if (Deconstructable != null)
                {
                    for (int i = 0; i < Deconstructable.constructionElements.Length; i++)
                        ConstructionElements.Add(Deconstructable.constructionElements[i]);
                }
                else
                    ConstructionElements.Add(primary_element.Element.tag);


                if (Building.TryGetComponent(out BuildingFacade buildingFacade))
                    CurrentFacade = buildingFacade.CurrentFacade;
                else
                    CurrentFacade = null;

                //Debug.Log(Building.Def.PrefabID);
                
                if (Exclusions.Contains(Building.Def.PrefabID))
                    return;

                Building.Def.TryPlace(Building.gameObject, Building.transform.position, Building.Orientation, ConstructionElements, CurrentFacade, false);

            }
            public static string[] Exclusions = new string[]
            {
                GasLimitValveConfig.ID,
                LiquidLimitValveConfig.ID,                
                LogicRibbonReaderConfig.ID,
                LogicRibbonWriterConfig.ID

            };

        }     
    }
}
