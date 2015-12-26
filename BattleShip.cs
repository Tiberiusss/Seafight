using System.Drawing;

namespace SeaFightProject
{
    public class BattleShip : Ship
    {
        public BattleShip() : base() {}

        public BattleShip(Point shipPosition, bool visible, Direction shipDirection, int scale) : base(shipPosition, visible, shipDirection, scale) {}

        protected override void InitializeShip()
        {
            _shipName = "Линкор";
            _shipLength = 3;
            _imageUrl = @"\Resources\3.png";
        }
    }
}
