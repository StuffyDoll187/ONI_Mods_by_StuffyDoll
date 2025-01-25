using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace Geyser_Control
{
    public class Patches
    {
        [HarmonyPatch(typeof(GeyserGenericConfig), nameof(GeyserGenericConfig.CreateGeyser))]
        public class GeyserGenericConfig_CreateGeyser_Patch
        {
            public static void Postfix(GameObject __result)
            {
                
                if (Config.Instance.dormancyButton)
                    __result.AddComponent<DormancyButton>();
                if (Config.Instance.eruptionButton)
                __result.AddComponent<EruptionButton>();                
                if (Config.Instance.resetButton)
                __result.AddComponent<GeyserSliders.ResetButton>();
                if (Config.Instance.randomizeSlidersButton)
                __result.AddComponent<GeyserSliders.RandomizeSlidersButton>();

                __result.AddComponent<GeyserSliders>();

                if (Config.Instance.uncapPressureCheckbox)
                    __result.AddComponent<UncapPressureCheckbox>();

                //Max Pressure Slider is conditionally added inside GeyserSliders constructor
                //Max Pressure Slider shelved in favor of UncapPressureCheckbox
                



            }
        }

        public static class Studyable_OnSidescreenButtonPressed_Patch
        {
            public static void Patch(Harmony harmony)
            {
                var original = typeof(Studyable).GetMethod(nameof(Studyable.OnSidescreenButtonPressed));
                var prefix = typeof(Studyable_OnSidescreenButtonPressed_Patch).GetMethod(nameof(Prefix));
                harmony.Patch(original, new HarmonyMethod(prefix), null, null, null);
            }

            public static bool Prefix(Studyable __instance, ref bool ___studied)
            {
                ___studied = true;
                __instance.CancelChore();
                __instance.Trigger((int)GameHashes.StudyComplete);
                __instance.Refresh();
                if (DlcManager.IsExpansion1Active())
                DropDatabanks(__instance);
                return false;
            }

            public static void DropDatabanks(Studyable instance)
            {
                int num = UnityEngine.Random.Range(7, 13);
                for (int index = 0; index <= num; ++index)
                {
                    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab((Tag)"OrbitalResearchDatabank"), instance.transform.position + new Vector3(0.0f, 1f, 0.0f), Grid.SceneLayer.Ore);
                    gameObject.GetComponent<PrimaryElement>().Temperature = 298.15f;
                    gameObject.SetActive(true);
                }
            }
        }

        /*[HarmonyPatch(typeof(Geyser))]
        [HarmonyPatch(nameof(Geyser.GetDescriptors))]
        public class Geyser_GetDescriptors_Patch
        {
            public static void Postfix(ref List<Descriptor> __result, Geyser __instance)
            {
                Descriptor copy;
                for (int i = 0; i < __result.Count; ++i)
                {
                    //Debug.Log(__result[i].text);
                    if (__result[i].text.Contains("Average Output"))
                    {
                        copy = __result[i];
                        copy.text += " (" + "test" + ")";
                        __result[i] = copy;
                    }
                }
                var geyser = __instance.configuration;

                string tooltip = "";
                //geyser.scaledRate
                tooltip += "Rate: " + geyser.scaledRate + "  (" + geyser.geyserType.minRatePerCycle + "-" + geyser.geyserType.maxRatePerCycle + ")";
                tooltip += "\n" + "Iteration Length: " + geyser.scaledIterationLength + "  (" + geyser.geyserType.minIterationLength + "-" + geyser.geyserType.maxIterationLength + ")";
                tooltip += "\n" + "Iteration Percent: " + geyser.scaledIterationPercent + "  (" + geyser.geyserType.minIterationPercent + "-" + geyser.geyserType.maxIterationPercent + ")";
                tooltip += "\n" + "Year Length: " + geyser.scaledYearLength + "  (" + geyser.geyserType.minYearLength + "-" + geyser.geyserType.maxYearLength + ")";
                tooltip += "\n" + "Year Percent: " + geyser.scaledYearPercent + "  (" + geyser.geyserType.minYearPercent + "-" + geyser.geyserType.maxYearPercent + ")";
                //__result.Add(new Descriptor("Roll Information (Hover)", tooltip));

                string str = __instance.smi.GetCurrentState().name + "\nIdle " + __instance.RemainingIdleTime().ToString() + "\nErupt " + __instance.RemainingEruptTime().ToString() + "\nNonErupt " + __instance.RemainingNonEruptTime().ToString() + "\n Active " + __instance.RemainingActiveTime().ToString() + "\nDormant " + __instance.RemainingDormantTime().ToString();
                __result.Add(new Descriptor(str, ""));
            }
        }*/

    }
}
