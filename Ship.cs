using System.Drawing;

namespace SeaFightProject
{
    public enum Direction { Horizontal, Vertical };

    public abstract class Ship
    {
        protected Size _shipSize;
        protected string _imageUrl;
        protected int _shipLength;
        protected bool _visible;
        protected string _shipName;
        protected Point _shipPosition;
        protected Direction _shipDirection;
        protected int _shipScale;
        protected int _shipID;

        public Size ShipSize
        {
            get
            {
                return _shipSize;
            }

            set
            {
                _shipSize = value;
            }
        }
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
        }
        public int ShipLength
        {
            get
            {
                return _shipLength;
            }
        }
        public bool Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                _visible = value;
            }
        }
        public string ShipName
        {
            get
            {
                return _shipName;
            }
        }
        public Point ShipPosition
        {
            get
            {
                return _shipPosition;
            }

            set
            {
                _shipPosition = value;
            }
        }
        public Direction ShipDirection
        {
            get
            {
                return _shipDirection;
            }

            set
            {
                _shipDirection = value;
                ShipSize = value == Direction.Horizontal ? new Size(32 * ShipLength, 32) : new Size(32, 32 * ShipLength);
            }
        }
        public int Scale
        {
            get
            {
                return _shipScale;
            }

            set
            {
                _shipScale = value;
                ShipSize = ShipDirection == Direction.Horizontal ? new Size(value * ShipLength, value) : new Size(value, value * ShipLength);
            }
        }
        public int ShipID
        {
            get
            {
                return 1;
            }
        }

        protected Ship(Point shipPosition, bool visible, Direction shipDirection, int scale)
        {
            InitializeShip();
            ShipPosition = shipPosition;
            ShipDirection = shipDirection;
            Scale = scale;
            ShipSize = ShipDirection == Direction.Horizontal ? new Size(Scale * ShipLength, Scale) : new Size(Scale, Scale * ShipLength);
            Visible = visible;
        }

        protected Ship()
        {
            InitializeShip();
            ShipPosition = new Point(0, 0);
            ShipDirection = Direction.Horizontal;
            Scale = 32;
            ShipSize = ShipDirection == Direction.Horizontal ? new Size(Scale * ShipLength, Scale) : new Size(Scale, Scale * ShipLength);
            Visible = true;
        }

        protected abstract void InitializeShip();
    }
}
