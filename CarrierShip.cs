using System.Drawing;

namespace SeaFightProject
{
    public class CarrierShip : Ship
    {
        public CarrierShip() : base() {}

        public CarrierShip(Point shipPosition, bool visible, Direction shipDirection, int scale) : base(shipPosition, visible, shipDirection, scale) {}

        protected override void InitializeShip()
        {
            _shipName = "Авианосец";
            _shipLength = 4;
            _imageUrl = @"\Resources\4.png";
        }
    }
}
