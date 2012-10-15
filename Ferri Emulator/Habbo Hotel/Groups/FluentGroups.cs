using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Database.Mappings;
using System.Data;

namespace Ferri_Emulator.Habbo_Hotel.Groups
{
    public class FluentGroups
    {
        public Dictionary<int, groups> Groups = new Dictionary<int, groups>();

        public void CacheGroups()
        {
            Groups = new Dictionary<int, groups>();
            DateTime Start = DateTime.Now;

            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM groups");
            var RowEnum = Data.Rows.GetEnumerator();

            while (RowEnum.MoveNext())
            {
                DataRow Row = (DataRow)RowEnum.Current;

                groups group = new groups()
                {
                    ID = (int)Row["id"],
                    Admins = Row["admins"].ToString().Split(';'),
                    Badges = Row["badge"].ToString(),
                    Created = Row["created"].ToString(),
                    Description = Row["description"].ToString(),
                    Members = Row["members"].ToString().Split(';'),
                    Name = Row["name"].ToString(),
                    Owner = Row["owner"].ToString()
                };

                Groups.Add(group.ID, group);
            }

            DateTime End = DateTime.Now;
            TimeSpan Expired = (End - Start);
            Engine.Logging.WriteTagLine("Cache", "Loaded {0} Groups in {1} s and {2} ms", Groups.Count, Expired.Seconds, Expired.Milliseconds);
        }
    }
}
