using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SeaFightProject
{
    public partial class Form1 : Form
    {
        void addWinnerIntoDB(string PlayerName)
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb.Server = "127.0.0.1";
            mscsb.Database = "morskoiboi";
            mscsb.UserID = "Tiberius";
            mscsb.Password = "Tiberius";

            MySqlConnection msc = new MySqlConnection(mscsb.ConnectionString);
            msc.Open();
            string sqlquery = "INSERT INTO `morskoiboi`.`winners` (`player_id`, `game_id`) VALUES (@PLAYER, @GAME)";
            MySqlCommand command = new MySqlCommand(sqlquery, msc);
            command.Parameters.AddWithValue("@GAME", game.gameID);
            command.Parameters.AddWithValue("@PLAYER", returnPlayerID(PlayerName));
            command.ExecuteNonQuery();
            msc.Close();
        }
        Game game;
        SeaFight seafight;
        SeaFight computerSeaFight;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = new Game();
            game.GameStarted += Game_GameStarted;
            game.StageChanged += Game_StageChanged;
        }

        int returnPlayerID(string PlayerName)
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb.Server = "127.0.0.1";
            mscsb.Database = "morskoiboi";
            mscsb.UserID = "Tiberius";
            mscsb.Password = "Tiberius";

            MySqlConnection msc = new MySqlConnection(mscsb.ConnectionString);
            msc.Open();
            string sqlquery = "SELECT * FROM morskoiboi.players WHERE name_player = @NAME";
            MySqlCommand command = new MySqlCommand(sqlquery, msc);
            command.Parameters.AddWithValue("@NAME", PlayerName);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                return (int)dt.Rows[0].ItemArray[0];
            } else
            {
                msc.Close();
                msc.Open();
                sqlquery = "INSERT INTO `morskoiboi`.`players` (`name_player`) VALUES (@NAME)";
                command = new MySqlCommand(sqlquery, msc);
                command.Parameters.AddWithValue("@NAME", PlayerName);
                command.ExecuteNonQuery();
                msc.Close();
                return (int)command.LastInsertedId;
            }
        }

        private void Game_GameStarted(object sender, EventArgs e)
        {
            Menu.Hide();
            seafight = new SeaFight(this, 300, 100, 100, invPanel, false, game.gameID, returnPlayerID(nicknameBox.Text));
            seafight.Square.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Resources\BackgroundImage.png", true);
            invPanel.Show();
            this.MouseWheel += Form1_MouseWheel;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.ClientSize = new Size(300, 300);
            Form1_Resize(this, e);
            seafight.MouseUp += Seafight_MouseUp;
        }

        private void Seafight_MouseUp(object sender, MouseEventArgs e)
        {
             if (e.Button == MouseButtons.Right && game.GameStage == GS.Prepare)
             {
                 Point pos = new Point(e.X, e.Y);
                 seafight.PutShip(pos);
             }
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (game.GameStage == GS.Prepare)
                if (e.Delta < 0)
                {
                    if (seafight.SelectedShipIndex + 1 >= seafight.AvailableShipsCount) seafight.SelectedShipIndex = 0;
                    else seafight.SelectedShipIndex += e.Delta > 0 ? -1 : 1; ;
                }
                else if (e.Delta > 0)
                {
                    if (seafight.SelectedShipIndex - 1 < 0) seafight.SelectedShipIndex = seafight.AvailableShipsCount - 1;
                    else seafight.SelectedShipIndex += e.Delta > 0 ? -1 : 1;
                }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (game.GameStage == GS.Prepare)
            {
                seafight.Size = ClientSize.Height > ClientSize.Width - invPanel.Width ? ClientSize.Width - invPanel.Width : ClientSize.Height;
                seafight.Left = (ClientSize.Width - invPanel.Width) / 2 - (seafight.Size / 2);
                seafight.Top = ClientSize.Height / 2 - seafight.Size / 2;
                seafight.Refresh();
            }
            else if (game.GameStage == GS.Play)
            {
                seafight.Size = ClientSize.Height > ClientSize.Width / 2 - 60 ? ClientSize.Width / 2 - 60 : ClientSize.Height;
                seafight.Left = 5;
                seafight.Top = ClientSize.Height / 2 - seafight.Size / 2;
                seafight.Size -= 30;
                seafight.Top += 30;
                seafight.Refresh();

                computerSeaFight.Size = ClientSize.Height > ClientSize.Width / 2 - 60 ? ClientSize.Width / 2 - 60 : ClientSize.Height;
                computerSeaFight.Left = ClientSize.Width - computerSeaFight.Size - 5;
                computerSeaFight.Top = ClientSize.Height / 2 - computerSeaFight.Size / 2;
                computerSeaFight.Size -= 30;
                computerSeaFight.Top += 30;
                computerSeaFight.Left += 30;
                computerSeaFight.Refresh();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (nicknameBox.Text.Trim() != "")
                game.StartGame();
            else MessageBox.Show("Введите ник!");
        }

        private void Game_StageChanged(object sender, EventArgs e)
        {
            if (game.GameStage == GS.Play)
                invPanel.Hide();
        }

        private void acceptShipsBtn_Click(object sender, EventArgs e)
        {
            if (seafight.AcceptShips())
            {
                computerSeaFight = new SeaFight(this, 300, 100, 100, invPanel, true, game.gameID, returnPlayerID("Computer"));
                computerSeaFight.Square.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Resources\BackgroundImage.png", true);
                computerSeaFight.MouseUp += ComputerSeaFight_MouseUp;
                game.GameStage = GS.Play;
                Form1_Resize(this, e);
            }
        }

        private void ComputerSeaFight_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && game.WhoWalk == WW.Player && game.GameStage == GS.Play)
            {
                Point pos = new Point(e.X, e.Y);
                if (computerSeaFight.Fire(pos) < 1) game.WhoWalk = WW.Computer;
                if (game.WhoWalk == WW.Computer && !computerSeaFight.isWin) timer1.Start();
                else if (computerSeaFight.isWin)
                {
                    game.GameStage = GS.End;
                    addWinnerIntoDB(nicknameBox.Text);
                    MessageBox.Show("Вы выиграли!");
                }
            }
        }

        private void rotateShipsBtn_Click(object sender, EventArgs e)
        {
            seafight.Rotate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!seafight.isWin)
            {
                if (seafight.ComputerMove(seafight) != true) game.WhoWalk = WW.Player;
                if (game.WhoWalk == WW.Player)
                    timer1.Stop();
            }
            else
            {
                timer1.Stop();
                addWinnerIntoDB("Computer");
                MessageBox.Show("Компьютер выиграл");
            }
        }
    }
}
