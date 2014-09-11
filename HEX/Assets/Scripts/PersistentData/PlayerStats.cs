using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


	public class PlayerStats
	{
        public enum place {  LEAVE = -1, FIRST = 1, SECOND = 2, THIRD = 3, FOURTH = 4};

        private place _placement;
        public place Placement {
            get { return _placement; }
            set { _placement = value; }
        }

        private int _kills;
        private int _deaths;

        public int Kills {
            get { return _kills; }
            set { _kills = value; }
        }

        public int Deaths
        {
            get { return _deaths; }
            set { _deaths = value; }
        }

        public PlayerStats() { }

        public PlayerStats(int kills, int deaths)
        {
            Kills = kills;
            Deaths = deaths;
        }
	}
