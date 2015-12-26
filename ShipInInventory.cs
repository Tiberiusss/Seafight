using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightProject
{
    public class ShipInInventory
    {
        public ShipInInventory(Ship shipInInventory, int count)
        {
            ShipInv = shipInInventory;
            Count = count;
            IconSize = ShipInv.ShipDirection == Direction.Horizontal ? new Size(32 * ShipInv.ShipLength, 32) : new Size(32, 32 * ShipInv.ShipLength);
        }
        Ship _shipInInventory;
        int _count;
        Size _iconSize;

        public Ship ShipInv
        {
            get { return _shipInInventory; }
            set { _shipInInventory = value; }
        }
        
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        
        public Size IconSize
        {
            get { return _iconSize; }
            set { _iconSize = value; }
        }

        public void Rotate()
        {
            ShipInv.ShipDirection = ShipInv.ShipDirection == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal;
            IconSize = ShipInv.ShipDirection == Direction.Horizontal ? new Size(32 * ShipInv.ShipLength, 32) : new Size(32, 32 * ShipInv.ShipLength);
        }
    }
}
