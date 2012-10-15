using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Habbo_Hotel.Users;
using Ferri_Emulator.Habbo_Hotel.Users.Inventory;
using Ferri_Emulator.Habbo_Hotel.Users.Badges;
using Ferri_Emulator.Habbo_Hotel.Users.Messenger;

namespace Ferri_Emulator.Database.Mappings
{
    public class users
    {
        public virtual int ID { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string Figure { get; set; }
        public virtual string Gender { get; set; }
        public virtual int Coins { get; set; }
        public virtual int Respect { get; set; }
        public virtual string Tags { get; set; }
        public virtual string Online { get; set; }
        public virtual string SsoTicket { get; set; }
        public virtual int Pixels { get; set; }
        public virtual int OtherCurrency { get; set; }
        public virtual string Motto { get; set; }
        public List<int> RatedRooms = new List<int>();
        public virtual RoomUser RoomUser { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual EmblemsManager Emblems { get; set; }
        public virtual MessengerComponent MessengerComponent { get; set; }
        public virtual int favouritegroup { get; set; }
    }
}
