using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ferri_Emulator.Habbo_Hotel.Users.Messenger
{
    public class MessengerComponent
    {
        private Dictionary<int, Messenger> MessengerBuddy = new Dictionary<int, Messenger>();

        public MessengerComponent(int UserId)
        {
            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM members_buddies WHERE userid = '" + UserId + "'");

            foreach (DataRow Row in Data.Rows)
            {
                Messenger Messenger = new Messenger()
                {
                    FriendID = (int)Row["friendid"],
                    GetFriend = FluentUsers.GetFromID((int)Row["friendid"])
                };

                MessengerBuddy.Add(Messenger.FriendID, Messenger);
            }
        }
    }
}
