using HarmonyLib;
using PeterHan.PLib.Detours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Forget_Chore_Tool
{
    public class ForgetChoreTool : DragTool
    {
        //public static ForgetChoreTool Instance;

        public static FieldInfo areaVisualizer = AccessTools.Field(typeof(DragTool), "areaVisualizer");
        public static FieldInfo areaVisualizerTextPrefab = AccessTools.Field(typeof(DragTool), "areaVisualizerTextPrefab");
        public static FieldInfo boxCursor = AccessTools.Field(typeof(DragTool), "boxCursor");
        public static FieldInfo Cursor = AccessTools.Field(typeof(InterfaceTool), "cursor");
        public static FieldInfo Visualizer = AccessTools.Field(typeof(InterfaceTool), "visualizer");

        public ForgetChoreToolHover HoverCard;

        protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
        {
            ForgetChore(this.GetRegularizedPos(Vector2.Min((Vector2)downPos, (Vector2)upPos), true), this.GetRegularizedPos(Vector2.Max((Vector2)downPos, (Vector2)upPos), false));
        }


        public static void ForgetChore(Vector2 min, Vector2 max)
        {
            foreach (MinionIdentity minion in Components.LiveMinionIdentities)
            {
                Vector2 xy = (Vector2)Grid.PosToXY(minion.transform.GetPosition());
                if (xy.x >= min.x && xy.x < max.x && xy.y >= min.y && xy.y < max.y)
                {
                    MinionBrain brain = minion.GetComponent<MinionBrain>();
                    brain.Reset("");
                }

            }
        }
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            var inst = CancelTool.Instance;
            //Instance = this;
            if (inst != null)
            {
                HoverCard = gameObject.AddComponent<ForgetChoreToolHover>();
                cursor = (Texture2D) Cursor.GetValue(inst);
                boxCursor.SetValue(this, cursor);
                var areaVis = (GameObject) areaVisualizer.GetValue(inst);
                if (areaVis != null)
                {
                    var areaVisual = Util.KInstantiate(areaVis, gameObject, "ForgetChoreToolVisualizer");
                    areaVisual.SetActive(false);
                    areaVisualizerSpriteRenderer = areaVisual.GetComponent<SpriteRenderer>();
                    areaVisualizer.SetValue(this, areaVisual);
                    var textPrefab = (GameObject) areaVisualizerTextPrefab.GetValue(inst);
                    areaVisualizerTextPrefab.SetValue(this, textPrefab);
                }
                var vis = (GameObject) Visualizer.GetValue(inst);
                visualizer = Util.KInstantiate(vis, gameObject, "ForgetChoreToolSprite");
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
