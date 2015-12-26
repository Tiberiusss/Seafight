using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace SeaFightProject
{
    enum MovePosition
    {
        TOPLEFTCORNER,
        TOPRIGHTCORNER,
        BOTTOMLEFTCORNER,
        BOTTOMRIGHTCORNER
    };

    class SeaFight
    {
        void AddPutShipIntoDB(Ship ship)
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb.Server = "127.0.0.1";
            mscsb.Database = "morskoiboi";
            mscsb.UserID = "Tiberius";
            mscsb.Password = "Tiberius";

            MySqlConnection msc = new MySqlConnection(mscsb.ConnectionString);
            msc.Open();
            string sqlquery = "INSERT INTO `morskoiboi`.`put_ships` (`coordinate_X`, `coordinate_Y`, `direction`, `type_ship_id`, `game_id`, `player_id`) VALUES (@X, @Y, @DIR, @TYPE, @GAME, @PLAYER)";
            MySqlCommand command = new MySqlCommand(sqlquery, msc);
            command.Parameters.AddWithValue("@X", ship.ShipPosition.X);
            command.Parameters.AddWithValue("@Y", ship.ShipPosition.Y);
            command.Parameters.AddWithValue("@DIR", ship.ShipDirection == Direction.Horizontal ? 0 : 1);
            command.Parameters.AddWithValue("@TYPE", ship.ShipID);
            command.Parameters.AddWithValue("@GAME", gameID);
            command.Parameters.AddWithValue("@PLAYER", playerID);
            command.ExecuteNonQuery();
            msc.Close();
        }

        void AddMoveIntoDB(Point position)
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb.Server = "127.0.0.1";
            mscsb.Database = "morskoiboi";
            mscsb.UserID = "Tiberius";
            mscsb.Password = "Tiberius";

            MySqlConnection msc = new MySqlConnection(mscsb.ConnectionString);
            msc.Open();
            string sqlquery = "INSERT INTO `morskoiboi`.`moves` (`coordinate_X`, `coordinate_Y`, `player_ID`, `game_ID`) VALUES (@X, @Y, @PLAYER, @GAME)";
            MySqlCommand command = new MySqlCommand(sqlquery, msc);
            command.Parameters.AddWithValue("@X", position.X);
            command.Parameters.AddWithValue("@Y", position.Y);
            command.Parameters.AddWithValue("@GAME", gameID);
            command.Parameters.AddWithValue("@PLAYER", playerID);
            command.ExecuteNonQuery();
            msc.Close();
        }

        List<MovePosition> movePositions;
        MovePosition availablemovePosition;
        List<Cell> cells;
        List<Ship> shipsOnSquare;
        int selectedShipIndex;
        List<ShipInInventory> availableShips;
        PictureBox square;
        Control invPanel;
        int size;
        public bool isComputer;
        int maxCountExplodedShips, countExplodedShips;
        public event MouseEventHandler MouseUp;
        Random rnd;
        bool isTargetAcquired;
        public bool isWin;
        public int gameID;
        public int playerID;

        public SeaFight(Control owner, int size, int left, int top, Control invPanel, bool isComputer, int gameID, int playerID)
        {
            this.gameID = gameID;
            shipsOnSquare = new List<Ship>();
            availableShips = new List<ShipInInventory>();
            cells = new List<Cell>();
            square = new PictureBox();
            square.BackgroundImageLayout = ImageLayout.Stretch;
            square.Parent = owner;
            Size = size;
            Left = left;
            Top = top;
            this.playerID = playerID;
            isWin = false;
            availableShips.Add(new ShipInInventory(new DestroyerShip(new Point(0, 0), true, Direction.Horizontal, size / 10), 4));
            availableShips.Add(new ShipInInventory(new CruiserShip(new Point(0, 0), true, Direction.Horizontal, size / 10), 3));
            availableShips.Add(new ShipInInventory(new BattleShip(new Point(0, 0), true, Direction.Horizontal, size / 10), 2));
            availableShips.Add(new ShipInInventory(new CarrierShip(new Point(0, 0), true, Direction.Horizontal, size / 10), 1));
            maxCountExplodedShips = 0;
            isTargetAcquired = false;
            for (int i = 0; i < availableShips.Count; i++)
                maxCountExplodedShips += availableShips[i].Count;
            this.isComputer = isComputer;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    cells.Add(new Cell(new Point(i, j), Size, TypeCell.Blank, square.CreateGraphics()));
            if (!isComputer)
                this.invPanel = invPanel;
            else
                PutShips();
            square.MouseUp += Square_MouseUp;
            rnd = new Random();
            movePositions = new List<MovePosition>();
        }

        void PutShips()
        {
            Random rnd = new Random();
            for (int i = 0; i < AvailableShipsCount; i++)
            {
                bool isExit = false;
                while (!isExit)
                {
                    selectedShipIndex = i;
                    Point position = new Point(rnd.Next(0, 10), rnd.Next(0, 10));
                    if (!PutShip(position))
                    {
                        foreach (ShipInInventory shipin in availableShips)
                            shipin.Rotate();
                        PutShip(position);
                    }
                    isExit = availableShips[i].Count == 0 ? true : false;
                }
            }
        }
        Point attackPosition;
        Point targetPosition;
        Point prevAttackPosition;
        enum AttackDirection { Left, Up, Right, Down };
        bool leftMove, rightMove, upMove, downMove;
        MovePosition tempPosition;
        public bool ComputerMove(SeaFight Target)
        {
            Cell cell;
            if (!isTargetAcquired)
            {
                movePositions = Enum.GetValues(typeof(MovePosition)).Cast<MovePosition>().Where(v => v != availablemovePosition).ToList();
                availablemovePosition = movePositions[rnd.Next(0, movePositions.Count)];
                tempPosition = availablemovePosition;
                int xMin = 0, xMax = 0, yMin = 0, yMax = 0;
                switch (availablemovePosition)
                {
                    case MovePosition.TOPLEFTCORNER:
                        xMin = 0;
                        xMax = 4;
                        yMin = 0;
                        yMax = 4;
                        break;
                    case MovePosition.TOPRIGHTCORNER:
                        xMin = 5;
                        xMax = 9;
                        yMin = 0;
                        yMax = 4;
                        break;
                    case MovePosition.BOTTOMLEFTCORNER:
                        xMin = 0;
                        xMax = 4;
                        yMin = 5;
                        yMax = 9;
                        break;
                    case MovePosition.BOTTOMRIGHTCORNER:
                        xMin = 5;
                        xMax = 9;
                        yMin = 5;
                        yMax = 9;
                        break;
                }
                List<Cell> availableCells = cells.Where(v => v.position.X >= xMin && v.position.X <= xMax && v.position.Y >= yMin &&
               v.position.Y <= yMax && v.type != TypeCell.Wounded && v.type != TypeCell.Miss).Cast<Cell>().ToList();
                while (availableCells.Count == 0)
                {
                    movePositions = movePositions.Where(v => v.ToString() != availablemovePosition.ToString()).ToList();
                    if (movePositions.Count == 0) availablemovePosition = tempPosition;
                    else availablemovePosition = movePositions[rnd.Next(0, movePositions.Count)];
                    xMin = xMax = yMin = yMax = 0;
                    switch (availablemovePosition)
                    {
                        case MovePosition.TOPLEFTCORNER:
                            xMin = 0;
                            xMax = 4;
                            yMin = 0;
                            yMax = 4;
                            break;
                        case MovePosition.TOPRIGHTCORNER:
                            xMin = 5;
                            xMax = 9;
                            yMin = 0;
                            yMax = 4;
                            break;
                        case MovePosition.BOTTOMLEFTCORNER:
                            xMin = 0;
                            xMax = 4;
                            yMin = 5;
                            yMax = 9;
                            break;
                        case MovePosition.BOTTOMRIGHTCORNER:
                            xMin = 5;
                            xMax = 9;
                            yMin = 5;
                            yMax = 9;
                            break;
                    }
                    availableCells = cells.Where(v => v.position.X >= xMin && v.position.X <= xMax && v.position.Y >= yMin &&
               v.position.Y <= yMax && v.type != TypeCell.Wounded && v.type != TypeCell.Miss).Cast<Cell>().ToList();
                }
                cell = availableCells[rnd.Next(0, availableCells.Count)];
                attackPosition = cell.position;
            } else
            {
                if (leftMove)
                {
                    prevAttackPosition = new Point(prevAttackPosition.X - 1, prevAttackPosition.Y);
                    cell = cells.FirstOrDefault(c => c.position == prevAttackPosition);
                    if (cell == null || cell.type == TypeCell.Miss)
                    {
                        leftMove = false;
                        rightMove = true;
                        prevAttackPosition = targetPosition;
                    }
                }
                if (rightMove)
                {
                    prevAttackPosition = new Point(prevAttackPosition.X + 1, prevAttackPosition.Y);
                    cell = cells.FirstOrDefault(c => c.position == prevAttackPosition);
                    if (cell == null || cell.type == TypeCell.Miss)
                    {
                        rightMove = false;
                        upMove = true;
                        prevAttackPosition = targetPosition;
                    }
                }
                if (upMove)
                {
                    prevAttackPosition = new Point(prevAttackPosition.X, prevAttackPosition.Y - 1);
                    cell = cells.FirstOrDefault(c => c.position == prevAttackPosition);
                    if (cell == null || cell.type == TypeCell.Miss)
                    {
                        upMove = false;
                        downMove = true;
                        prevAttackPosition = targetPosition;
                    }
                }
                if (downMove)
                {
                    prevAttackPosition = new Point(prevAttackPosition.X, prevAttackPosition.Y + 1);
                    cell = cells.FirstOrDefault(c => c.position == prevAttackPosition);
                    if (cell == null || cell.type == TypeCell.Miss)
                    {
                        downMove = false;
                        leftMove = true;
                        prevAttackPosition = targetPosition;
                    }
                }
                attackPosition = prevAttackPosition;
            }
            int result = Target.Fire(attackPosition);
            if (result == 1 && !isTargetAcquired)
            {
                isTargetAcquired = true;
                leftMove = true;
                rightMove = false;
                upMove = false;
                downMove = false;
                targetPosition = attackPosition;
                prevAttackPosition = attackPosition;
            }
            else if (result == 2) { isTargetAcquired = false; }
            else if (result == 0 && isTargetAcquired)
            {
                if (leftMove)
                {
                    leftMove = false;
                    rightMove = true;
                    prevAttackPosition = targetPosition;
                }
                else if (rightMove)
                {
                    rightMove = false;
                    upMove = true;
                    prevAttackPosition = targetPosition;
                }
                else if (upMove)
                {
                    upMove = false;
                    downMove = true;
                    prevAttackPosition = targetPosition;
                }
            }
            return result >= 1 ? true : false;
        }

        public int Fire(Point position)
        {
            int result = -1;
            Point pos = new Point();
            if (isComputer)
                pos = new Point(position.X / (size / 10), position.Y / (size / 10));
            else pos = new Point(position.X, position.Y);
            Cell cell = cells.FirstOrDefault(c => c.position == pos);
            if (cell.type == TypeCell.Blank || cell.type == TypeCell.OccupiedShip)
            {
                if (cell.type == TypeCell.OccupiedShip)
                {
                    cell.type = TypeCell.Wounded;
                    result = 1;
                }
                else
                {
                    cell.type = TypeCell.Miss;
                    result = 0;
                }
                Ship ship = shipsOnSquare.SingleOrDefault(s =>
                {
                    bool res = false;
                    switch (s.ShipDirection)
                    {
                        case Direction.Horizontal:
                            res = ((cell.position.X >= s.ShipPosition.X) && (cell.position.X < s.ShipPosition.X + s.ShipLength && cell.position.Y == s.ShipPosition.Y));
                            break;
                        case Direction.Vertical:
                            res = (cell.position.Y >= s.ShipPosition.Y && cell.position.Y < s.ShipPosition.Y + s.ShipLength && cell.position.X == s.ShipPosition.X);
                            break;
                    }
                    return res;
                });
                bool isShipExplode = true;
                if (ship != null)
                {
                    cell = null;
                    for (int i = 0; i < ship.ShipLength; i++)
                        switch (ship.ShipDirection)
                        {
                            case Direction.Horizontal:
                                cell = cells.SingleOrDefault(c => c.position == new Point(ship.ShipPosition.X + i, ship.ShipPosition.Y));
                                if (cell.type == TypeCell.OccupiedShip) isShipExplode = false;
                                break;
                            case Direction.Vertical:
                                cell = cells.SingleOrDefault(c => c.position == new Point(ship.ShipPosition.X, ship.ShipPosition.Y + i));
                                if (cell.type == TypeCell.OccupiedShip) isShipExplode = false;
                                break;
                        }
                    if (isShipExplode)
                    {
                        for (int i = 0; i <= ship.ShipLength + 1; i++)
                            for (int j = 0; j < 3; j++)
                            {
                                cell = null;
                                switch (ship.ShipDirection)
                                {
                                    case Direction.Horizontal:
                                        cell = cells.SingleOrDefault(c => c.position == new Point(ship.ShipPosition.X + i - 1, ship.ShipPosition.Y + j - 1));
                                        break;
                                    case Direction.Vertical:
                                        cell = cells.SingleOrDefault(c => c.position == new Point(ship.ShipPosition.X + j - 1, ship.ShipPosition.Y + i - 1));
                                        break;
                                }
                                if (cell != null && cell.type == TypeCell.Blank) { cell.type = TypeCell.Miss; }
                            }
                        ship.Visible = true;
                        countExplodedShips++;
                        if (countExplodedShips == maxCountExplodedShips)
                            isWin = true;
                        result = 2;
                    }
                }
                Refresh();
            }
            else result = 1;
            if (result > 0) AddMoveIntoDB(pos);
            return result;
        }

        public bool PutShip(Point pos)
        {
            Point position = new Point();
            if (!isComputer)
                position= new Point(pos.X / (size / 10), pos.Y / (size / 10));
            else position = new Point(pos.X, pos.Y);
            bool isAdd = true;
            if (shipsOnSquare.Count != maxCountExplodedShips && availableShips[selectedShipIndex].Count > 0)
            {
                for (int i = 0; i <= availableShips[selectedShipIndex].ShipInv.ShipLength + 1; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Cell cell = null;
                        switch (availableShips[selectedShipIndex].ShipInv.ShipDirection)
                        {
                            case Direction.Horizontal:
                                cell = cells.SingleOrDefault(c => c.position == new Point(position.X + i - 1, position.Y + j - 1));
                                break;
                            case Direction.Vertical:
                                cell = cells.SingleOrDefault(c => c.position == new Point(position.X + j - 1, position.Y + i - 1));
                                break;
                        }

                        if (cell != null && cell.type == TypeCell.OccupiedShip) { isAdd = false; break; }
                    }
                }
                for (int i = 0; i < availableShips[selectedShipIndex].ShipInv.ShipLength; i++)
                {
                    Cell cell = null;
                    switch (availableShips[selectedShipIndex].ShipInv.ShipDirection)
                    {
                        case Direction.Horizontal:
                            cell = cells.SingleOrDefault(c => c.position == new Point(position.X + i, position.Y));
                            break;
                        case Direction.Vertical:
                            cell = cells.SingleOrDefault(c => c.position == new Point(position.X, position.Y + i));
                            break;
                    }
                    if (cell == null) { isAdd = false; break; }
                }
                if (isAdd)
                {
                    Ship ship = (Ship)Assembly.GetExecutingAssembly().CreateInstance(availableShips[selectedShipIndex].ShipInv.GetType().ToString());
                    shipsOnSquare.Add(ship);
                    availableShips[selectedShipIndex].Count--;
                    shipsOnSquare[shipsOnSquare.Count - 1].ShipDirection = availableShips[selectedShipIndex].ShipInv.ShipDirection;
                    shipsOnSquare[shipsOnSquare.Count - 1].Scale = size / 10;
                    shipsOnSquare[shipsOnSquare.Count - 1].ShipPosition = position;
                    if (isComputer)
                        shipsOnSquare[shipsOnSquare.Count - 1].Visible = false;
                    for (int i = 0; i < shipsOnSquare[shipsOnSquare.Count - 1].ShipLength; i++)
                    {
                        Cell cell = null;
                        switch (availableShips[selectedShipIndex].ShipInv.ShipDirection)
                        {
                            case Direction.Horizontal:
                                cell = cells.Single(c => c.position == new Point(shipsOnSquare[shipsOnSquare.Count - 1].ShipPosition.X + i, shipsOnSquare[shipsOnSquare.Count - 1].ShipPosition.Y));
                                break;
                            case Direction.Vertical:
                                cell = cells.Single(c => c.position == new Point(shipsOnSquare[shipsOnSquare.Count - 1].ShipPosition.X, shipsOnSquare[shipsOnSquare.Count - 1].ShipPosition.Y + i));
                                break;
                        }
                        cell.type = TypeCell.OccupiedShip;
                    }
                    AddPutShipIntoDB(shipsOnSquare[shipsOnSquare.Count - 1]);
                    Refresh();
                }
            }
            return isAdd;
        }

        private void Square_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(sender, e);
        }

        public PictureBox Square
        {
            get
            {
                return square;
            }

            set
            {
                square = value;
            }
        }

        public bool AcceptShips()
        {
            ShipInInventory ship = availableShips.FirstOrDefault(s => s.Count > 0);
            if (ship == null) return true;
            else return false;
        }

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = 10 * (value / 10);
                square.Width = 10 * (value / 10);
                square.Height = 10 * (value / 10);
                foreach (Cell cell in cells)
                    cell.size = size / 10;
                foreach (Ship ship in shipsOnSquare)
                    ship.Scale = size / 10;
            }
        }

        int left;
        public int Left
        {
            get
            {
                return left;
            }

            set
            {
                left = value;
                square.Left = left;
            }
        }

        int top;
        public int Top
        {
            get
            {
                return top;
            }

            set
            {
                top = value;
                square.Top = top;
            }
        }

        public int SelectedShipIndex
        {
            get
            {
                return selectedShipIndex;
            }

            set
            {
                selectedShipIndex = value;
                Refresh();
            }
        }

        public int AvailableShipsCount
        {
            get
            {
                return availableShips.Count;
            }
        }

        void DrawGrid()
        {
            using (Graphics gps = square.CreateGraphics())
            {
                for (int i = 0; i < 10; i++)
                {
                    gps.DrawLine(Pens.Black, size / 10 * i, 0, size / 10 * i, size);
                    gps.DrawLine(Pens.Black, 0, size / 10 * i, size, size / 10 * i);
                }
            };
        }

        void DrawInv()
        {
            using (Graphics gps = invPanel.CreateGraphics())
            {
                gps.Clear(Color.White);
                int top = 0;
                int i = 0;
                foreach (ShipInInventory ship in availableShips)
                {
                    gps.DrawString(ship.Count.ToString() + 'x', new Font(invPanel.Font, FontStyle.Bold), Brushes.Black, new PointF(10, top + (ship.IconSize.Height / 2 - TextRenderer.MeasureText(ship.Count.ToString() + "x", new Font(invPanel.Font, FontStyle.Bold)).Height/2)));
                    Pen pen = new Pen(Color.Aqua, 5);
                    Size textSize = TextRenderer.MeasureText(ship.Count.ToString() + "x", new Font(invPanel.Font, FontStyle.Bold));
                    Rectangle rect = new Rectangle(new Point(textSize.Width + 10, top), ship.IconSize);
                    if (selectedShipIndex == i)
                        gps.DrawRectangle(pen, rect);
                    Bitmap img = new Bitmap(Image.FromFile(Application.StartupPath + ship.ShipInv.ImageUrl));
                    if (ship.ShipInv.ShipDirection == Direction.Vertical)
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    gps.DrawImage(img, rect);
                    top += ship.IconSize.Height + 20;
                    i++;
                }
            }
        }

        void DrawCells()
        {
            foreach (Cell cell in cells)
            {
                cell.owner = square.CreateGraphics();
                cell.DrawCell();
            }
        }

        void DrawShips()
        {
            using (Graphics gps = square.CreateGraphics())
            {
                foreach (Ship ship in shipsOnSquare)
                {
                    if (ship.Visible)
                    {
                        Bitmap img = new Bitmap(Image.FromFile(Application.StartupPath + ship.ImageUrl));
                        if (ship.ShipDirection == Direction.Vertical)
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        gps.DrawImage(img, new Rectangle(new Point(ship.ShipPosition.X * ship.Scale, ship.ShipPosition.Y * ship.Scale), ship.ShipSize));
                    }
                }
            }
        }

        public void Rotate()
        {
            foreach (ShipInInventory ship in availableShips)
                ship.Rotate();
            if (!isComputer)
                DrawInv();
        }

        public void Refresh()
        {
            square.Refresh();
            DrawGrid();
            DrawShips();
            DrawCells();
            if (!isComputer)
                DrawInv();
        }
    }
}
