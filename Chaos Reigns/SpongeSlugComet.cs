using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Chaos_Reigns
{
    internal class SpongeSlugComet : Comet
    {
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
                KBatchedAnimController component = GetComponent<KBatchedAnimController>();
                component.FlipX = flag;
            }
        }

        protected override void SpawnCraterPrefabs()
        {
            KBatchedAnimController animController = GetComponent<KBatchedAnimController>();
            animController.Play("wtr_ui");
            animController.onAnimComplete += delegate
            {
                if (craterPrefabs != null && craterPrefabs.Length != 0)
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
                    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(craterPrefabs[UnityEngine.Random.Range(0, craterPrefabs.Length)]), position);
                    Vector3 vector = gameObject.transform.position + mooSpawnImpactOffset;
                    if (!Grid.Solid[Grid.PosToCell(vector)])
                    {
                        gameObject.transform.position = vector;
                    }

                    gameObject.GetComponent<KBatchedAnimController>().FlipX = animController.FlipX;
                    gameObject.SetActive(value: true);


                    if (Config.Instance.SlugContentElementChoice1 != Config.LiquidOptions.None)
                    {
                        var storage = gameObject.GetComponent<Storage>();
                        storage.AddLiquid(Config.Instance.SlugContentElement1, Config.Instance.SlugContentMass1, Config.Instance.SlugContentTemp1 + 273.15f, 0, 0);
                    }
                    if (Config.Instance.SlugContentElementChoice2 != Config.LiquidOptions.None)
                    {
                        var storage = gameObject.GetComponent<Storage>();
                        storage.AddLiquid(Config.Instance.SlugContentElement2, Config.Instance.SlugContentMass2, Config.Instance.SlugContentTemp2 + 273.15f, 0, 0);
                    }
                    if (Config.Instance.SlugContentElementChoice3 != Config.LiquidOptions.None)
                    {
                        var storage = gameObject.GetComponent<Storage>();
                        storage.AddLiquid(Config.Instance.SlugContentElement3, Config.Instance.SlugContentMass3, Config.Instance.SlugContentTemp3 + 273.15f, 0, 0);
                    }

                    var agemonitor = gameObject.GetSMI<AgeMonitor.Instance>();
                    
                    agemonitor.age.value = agemonitor.age.GetMax() - (Config.Instance.SlugTTL / 600f);//* 0.99995f;
                }

                Util.KDestroyGameObject(base.gameObject);
            };
        }
    }
}
