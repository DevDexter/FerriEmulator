using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ferri_Emulator.Habbo_Hotel.Users.Inventory
{
    public class Inventory
    {
        public Dictionary<int, UserItems> All = new Dictionary<int, UserItems>();

        public Inventory(int Userid)
        {
            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM members_furniture WHERE userid = '" + Userid + "'");

            foreach (DataRow Row in Data.Rows)
            {
                UserItems Items = new UserItems()
                {
                    ID = (int)Row["id"],
                    FurniID = (int)Row["furniid"],
                    Room = (int)Row["room"],
                    Wall = Row["wall"].ToString(),
                    X = (int)Row["x"],
                    Y = (int)Row["y"],
                    Z = (int)Row["z"]
                };

                All.Add(Items.ID, Items);
            }
        }

        public List<UserItems> getFloorItems()
        {
            return (from i in All where i.Value.GetFurni().type == "s" && i.Value.Room == 0 select i.Value).ToList<UserItems>();
        }

        public List<UserItems> getWallItems()
        {
            return (from i in All where i.Value.GetFurni().type == "i" && i.Value.Room == 0 select i.Value).ToList<UserItems>();
        }
    }
}
