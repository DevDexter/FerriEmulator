using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ferri_Emulator.Habbo_Hotel.Achievements
{
    public class AchievementManager
    {
        public Dictionary<int, Achievement> Achievements;

        public void CacheAchievements()
        {
            Achievements = new Dictionary<int, Achievement>();
            DateTime Start = DateTime.Now;

            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM system_achievements");
            
            foreach(DataRow Row in Data.Rows)
            {
                Achievement Ach = new Achievement(Row);

                Achievements.Add(Ach.ID, Ach);
            }

            DateTime End = DateTime.Now;
            TimeSpan Expired = (End - Start);
            Engine.Logging.WriteTagLine("Cache", "Loaded {0} Achievements in {1} s and {2} ms", Achievements.Count, Expired.Seconds, Expired.Milliseconds);
        }
    }
}
