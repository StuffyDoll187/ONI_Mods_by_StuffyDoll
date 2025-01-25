using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Chaos_Reigns
{
    internal class TwitchZoologicalComet : Comet
    {       
        public static List<string> critters = new List<string>();

        public const float MOO_ANGLE = 15f;

        public Vector3 mooSpawnImpactOffset = new Vector3(-0.5f, 0f, 0f);

        private bool? initialFlipState;

        public void SetCustomInitialFlip(bool state)
        {
            initialFlipState = state;
        }

        public override void RandomizeVelocity()
        {
            bool flag = false;
            byte id = Grid.WorldIdx[Grid.PosToCell(base.gameObject.transform.position)];
            WorldContainer world = ClusterManager.Instance.GetWorld(id);
            if (!(world == null))
            {
                int num = world.WorldOffset.x + world.Width / 2;
                if (Grid.PosToXY(base.gameObject.transform.position).x > num)
                {
                    flag = true;
                }

                if (initialFlipState.HasValue)
                {
                    flag = initialFlipState.Value;
                }

                float num2 = (flag ? (-75f) : (-105f));
                float f = num2 * (float)Math.PI / 180f;
                float num3 = UnityEngine.Random.Range(spawnVelocity.x, spawnVelocity.y);
                velocity = new Vector2((0f - Mathf.Cos(f)) * num3, Mathf.Sin(f) * num3);
                //KBatchedAnimController component = GetComponent<KBatchedAnimController>();
                //component.FlipX = flag;
            }
        }

        protected override void SpawnCraterPrefabs()
        {
            KBatchedAnimController animController = GetComponent<KBatchedAnimController>();
            animController.Play("ui");
            animController.onAnimComplete += delegate
            {


                //if (craterPrefabs != null && craterPrefabs.Length != 0)
                if (critters != null && critters.Count != 0)
                {
                    byte world = Grid.WorldIdx[Grid.PosToCell(base.gameObject.transform.position)];
                    float x = 0f;
                    int cell = Grid.PosToCell(base.transform.GetPosition());
                    int num = Grid.OffsetCell(cell, 0, 1);
                    int num2 = Grid.OffsetCell(cell, 0, -1);
                    cell = ((!Grid.IsValidCellInWorld(num, world)) ? num2 : num);
                    if (Grid.Solid[cell])
                    {
                        bool flipX = animController.FlipX;
                        int num3 = Grid.OffsetCell(cell, -1, 0);
                        int num4 = Grid.OffsetCell(cell, 2, 0);
                        if (!flipX && Grid.IsValidCell(num3) && !Grid.Solid[num3])
                        {
                            cell = num3;
                        }
                        else if (flipX && Grid.IsValidCell(num4) && !Grid.Solid[num4])
                        {
                            cell = num4;
                        }
                    }
                    else
                    {
                        x = base.gameObject.transform.position.x - Mathf.Floor(base.gameObject.transform.position.x);
                    }

                    Vector3 position = Grid.CellToPos(cell) + new Vector3(x, 0f, Grid.GetLayerZ(Grid.SceneLayer.Creatures));                    
                    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(TwitchZoologicalComet.critters[UnityEngine.Random.Range(0, TwitchZoologicalComet.critters.Count)]), position);
                    //GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(craterPrefabs[UnityEngine.Random.Range(0, craterPrefabs.Length)]), position);                    
                    Vector3 vector = gameObject.transform.position + mooSpawnImpactOffset;
                    if (!Grid.Solid[Grid.PosToCell(vector)])
                    {
                        gameObject.transform.position = vector;
                    }

                    gameObject.GetComponent<KBatchedAnimController>().FlipX = animController.FlipX;
                    gameObject.SetActive(value: true);

                    /*var storage = gameObject.GetComponent<Storage>();
                    storage.AddLiquid(SimHashes.MoltenUranium, 10000f, 2273.15f, 0, 0);
                    //storage.AddLiquid(SimHashes.MoltenAluminum, 10000f, 2273.15f, 0, 0);
                    //storage.AddLiquid(SimHashes.Ethanol, 10000f, 50f + 273.15f, 0, 0);
                    storage.AddLiquid(SimHashes.Magma, 10000f, 2273.15f, 0, 0);
                    //storage.AddLiquid(SimHashes.CrudeOil, 10000f, 573.15f, 0, 0);
                    storage.AddLiquid(SimHashes.MoltenCobalt, 10000f, 2273.15f, 0, 0);*/

                    var agemonitor = gameObject.GetSMI<AgeMonitor.Instance>();
                    agemonitor.RandomizeAge();
                    //agemonitor.age.value = Mathf.Max( 0, agemonitor.age.GetMax() - Config.Instance.ZoologicalPeriod);
                }

                Util.KDestroyGameObject(base.gameObject);
            };
        }
    }
}
