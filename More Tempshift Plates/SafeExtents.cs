using System;
using UnityEngine;

namespace More_Tempshift_Plates
{
    internal class SafeExtents
    {
        public static Extents Extents(int cell, CellOffset[] offsets)
        {

            int num = 0;
            int num2 = 0;
            Grid.CellToXY(cell, out num, out num2);
            int num3 = num;
            int num4 = num2;
            foreach (CellOffset offset in offsets)
            {
                int val = 0;
                int val2 = 0;
                int offsetCell = SafeExtents.OffsetCell(cell, offset);
                Grid.CellToXY(offsetCell, out val, out val2);
                num = Math.Min(num, val);
                num2 = Math.Min(num2, val2);
                num3 = Math.Max(num3, val);
                num4 = Math.Max(num4, val2);
            }
            Extents extents = new Extents(num, num2, num3 - num + 1, num4 - num2 + 1);
            return extents;
            /* x = num;
             y = num2;
             width = num3 - num + 1;
             height = num4 - num2 + 1;*/
        }
        public static int OffsetCell(int cell, CellOffset offset)
        {
            int world = Grid.WorldIdx[cell];
            Grid.CellToXY(cell, out int x, out int y);
            int x1 = ClampXToWorld(x + offset.x, world);
            int y1 = ClampYToWorld(y + offset.y, world);
            int offsetCell = Grid.XYToCell(x1, y1);
            return offsetCell;


            // return cell + offset.x + offset.y * Grid.WidthInCells;
        }

        public static int ClampXToWorld(int x, int world) //Column
        {
            WorldContainer worldContainer = ClusterManager.Instance.GetWorld(world);
            return (int)Mathf.Clamp(x, worldContainer.minimumBounds.x, worldContainer.maximumBounds.x);
        }

        public static int ClampYToWorld(int y, int world) //Row
        {
            WorldContainer worldContainer = ClusterManager.Instance.GetWorld(world);
            return (int)Mathf.Clamp(y, worldContainer.minimumBounds.y, worldContainer.maximumBounds.y - 2);
        }



    }
}
