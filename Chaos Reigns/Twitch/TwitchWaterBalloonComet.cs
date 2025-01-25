using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

namespace Chaos_Reigns
{
    internal class TwitchWaterBalloonComet : Comet
    {
        protected override void SpawnCraterPrefabs()
        { 
            //Debug.Log("SpawnCraterPrefabs");            
            KBatchedAnimController animController = GetComponent<KBatchedAnimController>();
            
            //for (int i = 0; i < animController.CurrentAnim.animFile.animCount; i++)
                //Debug.Log(animController.CurrentAnim.animFile.GetAnim(i).name);

            animController.Play("splash");
            animController.animScale = 0.02f;

            int cell = Grid.CellAbove(Grid.PosToCell(this));
            SimMessages.EmitMass(cell, ElementLoader.GetElementIndex(SimHashes.Water), Config.Instance.TwitchWaterBalloonContentMass, 318.15f, 0, 0);

            //Debug.Log("Chance For Fish" + Config.Instance.ChanceForFish);
            float rand = UnityEngine.Random.Range(0f, 100f);
            //Debug.Log(rand);
            if ( rand < Config.Instance.TwitchChanceForFish)
            {
                GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("PacuTropical"), Grid.CellToPos(cell));
                //Debug.Log("Where is my fish?");
                gameObject.SetActive(true);
                
            }
            
           
            animController.onAnimComplete += delegate
            {                        
                Util.KDestroyGameObject(base.gameObject);
            };
        }
        public override void RandomizeVelocity()
        {
            //velocity = new UnityEngine.Vector2(0f, -8f);
            base.RandomizeVelocity();
        }

        
    }
    [HarmonyPatch(typeof(Comet), nameof(Comet.Sim33ms))]
    public class Comet_Sim33ms_Patch2
    {
        public static void Postfix(Comet __instance)
        {
            //Debug.Log(__instance);
            if (!__instance.TryGetComponent<TwitchWaterBalloonComet>(out var _))
                return;
            int cell = Grid.PosToCell(__instance);

            if (Grid.IsValidCell(cell) && Grid.IsSubstantialLiquid(cell))
            {
                /*Traverse comet = Traverse.Create(__instance.GetComponent<Comet>());
                Type[] arguments = new Type[4]
                {
                    typeof(Vector3),
                    typeof(int),
                    typeof(int),
                    typeof(Element)
                };                
                Traverse method = comet.Method("Explode", arguments);                
                method.GetValue(new object[] {__instance.transform.GetPosition(), cell, cell, __instance.GetComponent<PrimaryElement>().Element});
                Traverse field = comet.Field("hasExploded").SetValue(true);*/          
                __instance.Explode();
            }
        }

    }
}
