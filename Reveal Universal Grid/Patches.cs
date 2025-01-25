using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static STRINGS.DUPLICANTS.PERSONALITIES;

namespace Reveal_Universal_Grid
{
    public class Patches
    {

        //removes black cover of non-active worlds
        [HarmonyPatch(typeof(ClusterCoverPostFX), "SetupUVs")]
        public class ClusterCoverPostFX_SetupUVs_Patch
        {
            public static void Postfix(ref Material ___material)
            {
                //__instance.id = 0;
                ___material.SetFloat("_HideSurface", 0.0f);
                ___material.SetVector("_WorldCoords", new Vector4(0f, 0f, Grid.WidthInCells, Grid.HeightInCells));

                
            }
        }

        //reveal every cell on load
        [HarmonyPatch(typeof(ClusterManager), nameof(ClusterManager.InitializeWorldGrid))]
        public class ClusterManager_InitializeWorldGrid_Patch
        {
            public static void Postfix()
            {

                for (int i = 0; i < Grid.CellCount; i++)
                {
                    Grid.Reveal(i, 255, true);
                    FogOfWarMask.ClearMask(i);
                }



            }
        }


        //do not constrain camera
        [HarmonyPatch(typeof(CameraController))]
        [HarmonyPatch("ConstrainToWorld")]
        public static class CameraController_ConstrainToWorld_Patch
        {
            public static bool Prefix()
            {
                return false;
            }
        }


        [HarmonyPatch(typeof(CameraController), nameof(CameraController.GetWorldCamera))]
        public class CameraController_GetWorldCamera_Patch
        {
            public static bool Prefix(out Vector2I worldOffset, out Vector2I worldSize)
            {

                worldOffset = new Vector2I(0, 0);
                worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
                return false;
            }
        }

        /*[HarmonyPatch(typeof(SkyVisibilityVisualizerEffect), "FindWorldBounds")]
        public class SkyVisibilityVisualizerEffect_FindWorldBounds_Patch
        {
            public static void Postfix(out Vector2I world_min, out Vector2I world_max)
            {
                world_min.x = 0;
                world_min.y = 0;
                world_max.x = Grid.WidthInCells;
                world_max.y = Grid.HeightInCells;
            }
        }*/

        /*[HarmonyPatch(typeof(WorldContainer), nameof(WorldContainer.SetID))]
        public class WorldContainer_SetID_Patch
        {
            public static void Postfix(WorldContainer __instance)
            {
                
                
            }
        }*/


        /*[HarmonyPatch(typeof(GridSettings), nameof(GridSettings.Reset))]
        public class GridSettings_Reset_Patch
        {
            public static void Postfix(GridSettings __instance)
            {
                
            }
        }*/



        /*[HarmonyPatch(typeof(Grid), nameof(Grid.Reveal))]
        public class Grid_Reveal_Patch
        {
            public static void Prefix(ref byte visibility, ref bool forceReveal)
            {
                visibility = 255;
                forceReveal = true;
            }
        }*/
        [HarmonyPatch(typeof(Grid), nameof(Grid.IsActiveWorld))]
        public class Grid_IsActiveWorld_Patch
        {
            public static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }

        /*[HarmonyPatch(typeof(FogOfWarMask), nameof(FogOfWarMask.RevealFogOfWarMask))]
        public class FogOfWarMask_RevealFogOfWarMask_Patch
        {
            public static bool Prefix(int cell)
            {
                Grid.PreventFogOfWarReveal[cell] = false;
                Grid.Reveal(cell);
                return false;
            }
        }*/

        /*[HarmonyPatch(typeof(FogOfWarMask), "OnSpawn")]
        public class FogOfWarMask_OnSpawn_Patch
        {
            public static bool Prefix(FogOfWarMask __instance)
            {
                __instance.gameObject.DeleteObject();                
                return false;
            }
        }*/


        /*[HarmonyPatch(typeof(FogOfWarMask), nameof(FogOfWarMask.ClearMask))]
        public class FogOfWarMask_ClearMask_Patch
        {
            public static bool Prefix(int cell)
            {
                GameUtil.FloodCollectCells(cell, new Func<int, bool>(FogOfWarMask.RevealFogOfWarMask));
                return false;
            }
        }*/



        [HarmonyPatch(typeof(GridVisibleArea), nameof(GridVisibleArea.GetVisibleAreaExtended))]
        public class GridVisibleArea_GetVisibleAreaExtended_Patch
        {
            public static void Postfix(ref GridArea __result)
            {

                __result.SetArea(0, Grid.WidthInCells, Grid.HeightInCells);
            }
        }




        [HarmonyPatch(typeof(Grid), nameof(Grid.GetVisibleCellRangeInActiveWorld))]
        public class Grid_GetVisibleCellRangeInActiveWorld_Patch
        {
            public static void Postfix(out Vector2I min, out Vector2I max, int padding = 4, float rangeScale = 1.5f)
            {
                min = new Vector2I(0, 0);
                max = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
            }
        }

        [HarmonyPatch(typeof(Grid), nameof(Grid.GetVisibleExtents), new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) }, new ArgumentType[] { ArgumentType.Out, ArgumentType.Out, ArgumentType.Out, ArgumentType.Out })]
        public class Grid_GetVisibleExtents_Patch
        {
            public static void Postfix(out int min_x, out int min_y, out int max_x, out int max_y)
            {
                min_x = 0;
                min_y = 0;
                max_x = Grid.WidthInCells;
                max_y = Grid.HeightInCells;
            }
        }
    }
}
