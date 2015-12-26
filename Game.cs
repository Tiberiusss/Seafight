using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SeaFightProject
{
    public enum GS { Off, Prepare, Play, End };
    public enum WW { Player, Computer };

    class Game
    {
        void AddGameIntoDB()
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb.Server = "127.0.0.1";
            mscsb.Database = "morskoiboi";
            mscsb.UserID = "Tiberius";
            mscsb.Password = "Tiberius";

            MySqlConnection msc = new MySqlConnection(mscsb.ConnectionString);
            msc.Open();
            string sqlquery = "INSERT INTO `morskoiboi`.`games` (`date_game`) VALUES (@dateUpdated)";
            MySqlCommand command = new MySqlCommand(sqlquery, msc);
            command.Parameters.AddWithValue("@dateUpdated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            command.ExecuteNonQuery();
            gameID = (int)command.LastInsertedId;
            msc.Close();
        }
        public event EventHandler StageChanged;
        public event EventHandler GameStarted;

        GS gameStage;
        WW whoWalk;
        public int gameID;

        public Game()
        {
            gameStage = GS.Off;
            whoWalk = WW.Player;
        }

        public GS GameStage
        {
            get { return gameStage; }
            set
            {
                gameStage = value;
                if (StageChanged != null) StageChanged(null, new EventArgs());
            }
        }

        public WW WhoWalk
        {
            get { return whoWalk; }
            set
            {
                whoWalk = value;
            }
        }

        public void StartGame()
        {
            GameStage = GS.Prepare;
            AddGameIntoDB();
            if (GameStarted != null) GameStarted(null, new EventArgs());
        }
    }
}
