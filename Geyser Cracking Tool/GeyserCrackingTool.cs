extern alias destination;
using HarmonyLib;
using PeterHan.PLib.Detours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

//using HexiGeyserCracking;

namespace Geyser_Cracking_Tool
{
    public class GeyserCrackingTool : DragTool
    {        

        public static FieldInfo areaVisualizer = AccessTools.Field(typeof(DragTool), "areaVisualizer");
        public static FieldInfo areaVisualizerTextPrefab = AccessTools.Field(typeof(DragTool), "areaVisualizerTextPrefab");
        public static FieldInfo boxCursor = AccessTools.Field(typeof(DragTool), "boxCursor");
        public static FieldInfo Cursor = AccessTools.Field(typeof(InterfaceTool), "cursor");
        public static FieldInfo Visualizer = AccessTools.Field(typeof(InterfaceTool), "visualizer");

        public GeyserCrackingToolHover HoverCard;

        protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
        {
            MarkGeysersForCracking(this.GetRegularizedPos(Vector2.Min((Vector2)downPos, (Vector2)upPos), true), this.GetRegularizedPos(Vector2.Max((Vector2)downPos, (Vector2)upPos), false));
        }

        public static void MarkGeysersForCracking(Vector2 min, Vector2 max)
        {
            int worldID = ClusterManager.Instance.activeWorldId;            
            foreach (Geyser geyser in Components.Geysers.WorldItemsEnumerate(worldID, otherWorldIds: null))
            {
                Vector2 xy = (Vector2)Grid.PosToXY(geyser.transform.GetPosition());
                if (xy.x >= min.x && xy.x < max.x && xy.y >= min.y && xy.y < max.y)
                {
                     if (geyser.TryGetComponent<destination.HexiGeyserCracking.Crackable>(out destination.HexiGeyserCracking.Crackable crackable))
                     {
                        if (geyser.TryGetComponent<Studyable>(out Studyable studyable) && studyable.Studied)
                        {
                            if (!crackable.maxCracked)
                            {
                                if (crackable.markedForCracking == false)
                                {
                                    crackable.markedForCracking = true;
                                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, "Marked For Cracking", crackable.transform, lifetime: 2.25f);
                                }
                                else
                                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, "Already Marked", crackable.transform, lifetime: 2.25f);
                            }
                            else
                                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, "Max Cracking Reached", crackable.transform, lifetime: 2.25f);
                        }
                        else
                            PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, "Needs Analysis", crackable.transform, lifetime: 2.25f);
                    }
                }
            }            
        }
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            var inst = HarvestTool.Instance;            
            if (inst != null)
            {
                HoverCard = gameObject.AddComponent<GeyserCrackingToolHover>();
                cursor = (Texture2D) Cursor.GetValue(inst);
                boxCursor.SetValue(this, cursor);
                var areaVis = (GameObject) areaVisualizer.GetValue(inst);
                if (areaVis != null)
                {
                    var areaVisual = Util.KInstantiate(areaVis, gameObject, "GeyserCrackingToolVisualizer");
                    areaVisual.SetActive(false);
                    areaVisualizerSpriteRenderer = areaVisual.GetComponent<SpriteRenderer>();
                    areaVisualizer.SetValue(this, areaVisual);
                    var textPrefab = (GameObject) areaVisualizerTextPrefab.GetValue(inst);
                    areaVisualizerTextPrefab.SetValue(this, textPrefab);
                }
                var vis = (GameObject) Visualizer.GetValue(inst);
                visualizer = Util.KInstantiate(vis, gameObject, "GeyserCrackingToolSprite");
                visualizer.SetActive(false);

            }
            
        }
        protected override void OnActivateTool()
        {
            base.OnActivateTool();
            
        }

        protected override void OnDeactivateTool(InterfaceTool new_tool)
        {
            base.OnDeactivateTool(new_tool);
           
        }
    }
}
