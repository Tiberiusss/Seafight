using System.Drawing;

namespace SeaFightProject
{
    public class BattleShip : Ship
    {
        public BattleShip()
        {
            InitializeShip();
            ShipPosition = new Point(0, 0);
            ShipDirection = Direction.Horizontal;
            ShipSize = new Size(32 * ShipLength, 32);
            Visible = true;
        }

        public BattleShip(Point shipPosition, bool visible, Direction shipDirection, int scale)
        {
            InitializeShip();
            ShipPosition = shipPosition;
            ShipDirection = shipDirection;
            Scale = scale;
            ShipSize = ShipDirection == Direction.Horizontal ? new Size(Scale * ShipLength, Scale) : new Size(Scale, Scale * ShipLength);
            Visible = visible;
        }

        protected override void InitializeShip()
        {
            _shipName = "Линкор";
            _shipLength = 3;
            _imageUrl = @"\Resources\3.png";
        }
    }
}
