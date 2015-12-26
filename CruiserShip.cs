using System.Drawing;

namespace SeaFightProject
{
    public class CruiserShip: Ship
    {
        public CruiserShip() : base() {}

        public CruiserShip(Point shipPosition, bool visible, Direction shipDirection, int scale) : base(shipPosition, visible, shipDirection, scale) {}

        protected override void InitializeShip()
        {
            _shipName = "Крейсер";
            _shipLength = 2;
            _imageUrl = @"\Resources\2.png";
        }
    }
}
