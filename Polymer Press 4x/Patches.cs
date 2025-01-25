using HarmonyLib;
using UnityEngine;

namespace Polymer_Press_4x
{
    public class Patches
    {
        [HarmonyPatch(typeof(PolymerizerConfig))]
        [HarmonyPatch("ConfigureBuildingTemplate")]
        public class PolymerizerConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                Polymerizer polymerizer = go.GetComponent<Polymerizer>();
                polymerizer.emitMass = 120f;
                ConduitConsumer consumer = go.GetComponent<ConduitConsumer>();
                consumer.consumptionRate = 6.66666667f;
                consumer.capacityKG = 6.66666667f;
                ElementConverter converter = go.GetComponent<ElementConverter>();
                converter.consumedElements = new ElementConverter.ConsumedElement[1]
                {
                    new ElementConverter.ConsumedElement(PolymerizerConfig.INPUT_ELEMENT_TAG, 3.3333333f)
                };
                converter.outputElements = new ElementConverter.OutputElement[3]
                {
                    new ElementConverter.OutputElement(2f, SimHashes.Polypropylene, 348.15f, storeOutput: true),
                    new ElementConverter.OutputElement(0.033333334f, SimHashes.Steam, 473.15f, storeOutput: true),
                    new ElementConverter.OutputElement(0.033333334f, SimHashes.CarbonDioxide, 423.15f, storeOutput: true)
                };
            }
        }



    }
}
