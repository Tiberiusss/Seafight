using System.Drawing;

namespace SeaFightProject
{
    public class DestroyerShip : Ship
    {
        public DestroyerShip() : base() {}

        public DestroyerShip(Point shipPosition, bool visible, Direction shipDirection, int scale) : base(shipPosition, visible, shipDirection, scale) {}

        protected override void InitializeShip()
        {
            _shipName = "Эсминец";
            _shipLength = 1;
            _imageUrl = @"\Resources\1.png";
        }
    }
}
