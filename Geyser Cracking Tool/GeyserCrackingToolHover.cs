//using PeterHan.PLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Geyser_Cracking_Tool
{
    public class GeyserCrackingToolHover : HoverTextConfiguration
    {

        protected override void OnSpawn()
        {
            base.OnSpawn();
            // Take the text configuration from the existing sweep tool's hover text
            var template = ClearTool.Instance?.gameObject?.GetComponent<
                HoverTextConfiguration>();
            if (template != null)
            {
                Styles_BodyText = template.Styles_BodyText;
                Styles_Instruction = template.Styles_Instruction;
                Styles_Title = template.Styles_Title;
                Styles_Values = template.Styles_Values;
            }
        }

        public override void UpdateHoverElements(List<KSelectable> selected)
        {
            //var ts = FilteredMoveSelectTool.Instance.TypeSelect;
            var hoverInstance = HoverTextScreen.Instance;
            // Determine if in default Sweep All mode
            //bool all = ts.IsAllSelected;
            int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.
                GetMousePos()));
            // Draw the tool title
            //string titleStr = all ? STRINGS.UI.TOOLS.MOVETOSELECTTOOL.TOOLNAME :
            //STRINGS.UI.TOOLS.MOVETOSELECTTOOL.TOOL_NAME_FILTERED;
            string titleStr = "Geyser Cracking Tool";
            var drawer = hoverInstance.BeginDrawing();
            drawer.BeginShadowBar(false);
            drawer.DrawText(titleStr.ToUpper(), ToolTitleTextStyle);
            // Draw the instructions
            //ActionName = all ? STRINGS.UI.TOOLS.MOVETOSELECTTOOL.TOOLACTION.text :
            //STRINGS.UI.TOOLS.MOVETOSELECTTOOL.TOOLACTION_FILTERED.text;
            ActionName = "DRAG";
            DrawInstructions(hoverInstance, drawer);
            drawer.EndShadowBar();
            //if (selected != null && Grid.IsValidCell(cell) && Grid.IsVisible(cell))
                //DrawPickupText(selected, drawer, hoverInstance.GetSprite("dash"));
            drawer.EndDrawing();
        }
    }
}
