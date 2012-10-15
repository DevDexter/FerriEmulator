using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ferri_Emulator.Habbo_Hotel.Users.Badges
{
    public class EmblemsManager
    {
        private Dictionary<string, Emblems> Badges = new Dictionary<string, Emblems>();

        public void LoadBadges(int userid)
        {
            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM members_emblems WHERE userid = '" + userid + "'");

            foreach (DataRow Row in Data.Rows)
            {
                Emblems Emblem = new Emblems()
                {
                    ID = (int)Row["id"],
                    Badge = Row["badge"].ToString(),
                    SlotID = (int)Row["slotid"]
                };

                Badges.Add(Emblem.Badge, Emblem);
            }
        }

        public Emblems GetBadge(string badge)
        {
            return Badges[badge];
        }

        public List<KeyValuePair<string, Emblems>> getEmblems()
        {
            return Badges.ToList();
        }
    }
}
