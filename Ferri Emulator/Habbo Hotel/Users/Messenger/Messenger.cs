using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Database.Mappings;

namespace Ferri_Emulator.Habbo_Hotel.Users.Messenger
{
    public class Messenger
    {
        public virtual int FriendID { get; set; }
        public virtual users GetFriend { get; set; }
    }
}
