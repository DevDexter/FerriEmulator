using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Ferri_Emulator.Database.Mappings;

namespace Ferri_Emulator.Habbo_Hotel.Navigator
{
    public class FluentFrontpage
    {
        public frontpage this[int i]
        {
            get
            {
                return Engine.GetHabboHotel.getFrontpage.Frontpage[i];
            }
        }

        private Dictionary<int, frontpage> Frontpage;

        public void CacheFrontpage()
        {
            Frontpage = new Dictionary<int, frontpage>();

            DateTime Start = DateTime.Now;
            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM navigator_frontpage");
            var RowEnum = Data.Rows.GetEnumerator();

            while (RowEnum.MoveNext())
            {
                DataRow Row = (DataRow)RowEnum.Current;

                Frontpage.Add((int)Row["id"], new frontpage()
                {
                    ID = (int)Row["id"],
                    Name = Row["name"].ToString(),
                    Description = Row["description"].ToString(),
                    BannerLength = int.Parse(Row["bannerlength"].ToString()),
                    Category_ID = (int)Row["category_id"],
                    Image = Row["image"].ToString(),
                    Recommended = (Row["recommended"].ToString() == "1"),
                    Type = int.Parse(Row["type"].ToString())
                });
            }

            DateTime End = DateTime.Now;
            TimeSpan Expired = (End - Start);
            Engine.Logging.WriteTagLine("Cache", "Loaded {0} Frontpage Items in {1} s and {2} ms", Frontpage.Count, Expired.Seconds, Expired.Milliseconds);
        }

        public int getCount()
        {
            return Frontpage.Count;
        }

        public Dictionary<int, frontpage> getList()
        {
            return Frontpage;
        }
    }
}
