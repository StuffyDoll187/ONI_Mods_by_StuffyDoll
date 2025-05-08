/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace Remote_Worker_Errands_Expanded
{
    class CancellableMoveRemote : RemoteWorkable
    {
        [MyCmpGet]
        CancellableMove CancellableMove;
        public override Chore RemoteDockChore => CancellableMove.fetchChore;

        [HarmonyPatch(typeof(CancellableMove), "OnSpawn")]
        public class CancellableMove_OnSpawn_Patch
        {
            public static void Postfix(CancellableMove __instance)
            {
                __instance.FindOrAddComponent<CancellableMoveRemote>();
            }
        }


        [HarmonyPatch(typeof(MovePickupableChore.States), nameof(MovePickupableChore.States.InitializeStates))]
        public class dfjaskfld
        {
            public static void Postfix(MovePickupableChore.States __instance)
            {
                __instance.fetch.Enter(smi => DebugStuff(smi, __instance));
            }
            public static void DebugStuff(MovePickupableChore.StatesInstance smi, MovePickupableChore.States instance)
            {
                Debug.Log(instance.deliverer.Get(smi));
                Debug.Log(instance.pickupablesource.Get(smi));
                Debug.Log(instance.requestedamount.Get(smi));
                Debug.Log(instance.actualamount.Get(smi));
            }
        }
        
        
    }
}
*/