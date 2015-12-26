using System.Drawing;

namespace SeaFightProject
{
    public class CarrierShip : Ship
    {
        public CarrierShip()
        {
            InitializeShip();
            ShipPosition = new Point(0, 0);
            ShipDirection = Direction.Horizontal;
            ShipSize = new Size(32 * ShipLength, 32);
            Visible = true;
        }

        public CarrierShip(Point shipPosition, bool visible, Direction shipDirection, int scale)
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
            _shipName = "Авианосец";
            _shipLength = 4;
            _imageUrl = @"\Resources\4.png";
        }
    }
}
