using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Habbo_Hotel.Items;
using Ferri_Emulator.Database.Mappings;

namespace Ferri_Emulator.Habbo_Hotel.Users
{
    public class UserItems
    {
        public virtual int ID { get; set; }
        public virtual int FurniID { get; set; }
        public virtual int Room { get; set; }
        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public virtual int Z { get; set; }
        public virtual string Wall { get; set; }
        public virtual furniture GetFurni()
        {
            return Engine.GetHabboHotel.getItemDefinitions.Definitions[(uint)ID];
        }
    }
}
