using System.Drawing;
using System.Windows.Forms;

namespace SeaFightProject
{
   public enum TypeCell
    {
        Blank,
        OccupiedShip,
        Wounded,
        Miss
    };

    public  class Cell
    {
        public Point position;
        public int size;
        public TypeCell type;
        public Graphics owner;

        public Cell(Point position, int size, TypeCell type, Graphics owner)
        {
            this.position = position;
            this.size = size;
            this.type = type;
            this.owner = owner;
        }

        public void DrawCell()
        {
            if (type == TypeCell.Wounded)
                owner.DrawImage(Image.FromFile(Application.StartupPath + @"\Resources\Crest.png", true), position.X * size + 5, position.Y * size + 5, size - 10, size - 10);
            else if (type == TypeCell.Miss)
                owner.DrawImage(Image.FromFile(Application.StartupPath + @"\Resources\Miss.png", true), position.X * size + 5, position.Y * size + 5, size - 10, size - 10);
        }
    }
}
