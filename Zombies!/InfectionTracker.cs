using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using KSerialization;
using Klei.AI;

namespace Zombies
{
    public class InfectionTracker : KMonoBehaviour
    {

        [Serialize]
        public Dictionary<Guid, bool> infected = new Dictionary<Guid, bool>();



        [HarmonyPatch(typeof(MinionStorage), "OnPrefabInit")]
        public class MinionStorage_OnPrefabInit
        {
            public static void Postfix(MinionStorage __instance)
            {
                __instance.FindOrAddComponent<InfectionTracker>();
            }
        }

        [HarmonyPatch(typeof(MinionStorage), nameof(MinionStorage.SerializeMinion))]
        public class MinionStorage_SerializeMinion_Patch
        {
            public static void Postfix(GameObject minion, List<MinionStorage.Info> ___serializedMinions, MinionStorage __instance)
            {
                if (CustomGameSettings.Instance.GetCurrentQualitySetting(CustomSettings.Zombies).id == "Disabled")
                    return;
                Debug.Log("[Zombies!]Serializing " + minion.gameObject.name + " into " + __instance.gameObject.name);
                if (__instance.TryGetComponent(out InfectionTracker tracker))
                {
                    var id = ___serializedMinions[___serializedMinions.Count - 1].id;
                    //Debug.Log(id);
                    if (!tracker.infected.TryGetValue(id, out bool _))
                    {
                        var flag = minion.GetComponent<MinionModifiers>().sicknesses.Has(Db.Get().Sicknesses.ZombieSickness);
                        Debug.Log("is infected: " + flag);
                        tracker.infected.Add(id, flag);
                    }
                    else
                        Debug.Log("[Zombies!] Dictionary Entry already exists. Unexpected!");                    
                }
                else
                    Debug.Log("[Zombies!] No InfectionTracker found on MinionStorage: " + __instance.gameObject.name);

            }
        }

        [HarmonyPatch(typeof(MinionStorage), nameof(MinionStorage.DeserializeMinion), new Type[] { typeof(GameObject), typeof(Vector3) })]
        public class MinionStorage_DeserializeMinion_Patch2
        {
            public static void Postfix(GameObject __result, MinionStorage __instance)
            {
                if (CustomGameSettings.Instance.GetCurrentQualitySetting(CustomSettings.Zombies).id == "Disabled")
                    return;
                Debug.Log("[Zombies!] Deserializing " + __result.gameObject.name);
            }
        }

        [HarmonyPatch(typeof(MinionStorage), nameof(MinionStorage.DeserializeMinion), new Type[] { typeof(Guid), typeof(Vector3) })]
        public class MinionStorage_DeserializeMinion_Patch
        {
            public static void Postfix(Guid id, GameObject __result, MinionStorage __instance)
            {
                if (CustomGameSettings.Instance.GetCurrentQualitySetting(CustomSettings.Zombies).id == "Disabled")
                    return;
                //Debug.Log(id);
                Debug.Log("from " + __instance.gameObject.name);
                if (__instance.TryGetComponent(out InfectionTracker tracker))
                {
                    if (tracker.infected.TryGetValue(id, out bool infected))
                    {
                        tracker.infected.Remove(id);
                        if (__result.TryGetComponent(out ZombieInitialInfection initialInfection))
                        {
                            initialInfection.beingDeserialized = true;
                            initialInfection.serializedInfected = infected;
                            Debug.Log("is infected: " + infected);
                        }
                        else
                            Debug.Log("[Zombies!] No ZombieInitialInfection found on: " + __result.gameObject.name);
                    }
                    else
                        Debug.Log("[Zombies!] Minion not found in InfectionTracker Dictionary");
                }
                else
                    Debug.Log("[Zombies!] No InfectionTracker found on MinionStorage: " + __instance.gameObject.name);
            }
        }
    }
}
