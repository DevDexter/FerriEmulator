using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ferri_Emulator.Habbo_Hotel.Achievements
{
    public class Achievement
    {
        public int ID;
        public string Type;
        public string Category;
        public string[] Levels;
        public string[] Rewards;
        public string[] Goals;

        public Achievement(DataRow Row)
        {
            this.ID = (int)Row["id"];
            this.Type = Row["type"].ToString();
            this.Category = Row["category"].ToString();
            this.Levels = Row["levels"].ToString().Split(';');
            this.Rewards = Row["rewards"].ToString().Split(';');
            this.Goals = Row["goals"].ToString().Split(';');
        }
    }
}
