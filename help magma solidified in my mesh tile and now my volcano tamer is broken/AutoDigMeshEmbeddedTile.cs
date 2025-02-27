using HarmonyLib;
using UnityEngine;

namespace help_magma_solidified_in_my_mesh_tile_and_now_my_volcano_tamer_is_broken
{
    public class AutoDigMeshEmbeddedTile : KMonoBehaviour, ISim200ms
    {                
        int cell;
        protected override void OnSpawn()
        {
            base.OnSpawn();
            cell = Grid.PosToCell(this);
        }
        public void Sim200ms(float dt)
        {
            if (Grid.Element[cell].IsSolid)            
                SimMessages.Dig(cell);                                                                                 
        }        
    }
    [HarmonyPatch(typeof(MeshTileConfig), nameof(MeshTileConfig.DoPostConfigureComplete))]
    public class MeshTileConfig_DoPostConfigureComplete_Patch
    {
        public static void Postfix(GameObject go)
        {
            go.AddComponent<AutoDigMeshEmbeddedTile>();
        }
    }
}
