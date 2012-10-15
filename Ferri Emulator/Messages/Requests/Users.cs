using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.SS;
using Ferri.Kernel.Network;
using Ferri_Emulator.Habbo_Hotel.Users;

namespace Ferri_Emulator.Messages.Requests
{
    public class Users : Data
    {
        public static void UpdateBadges(Message Message, Session Session)
        {
            foreach (var Badge in Session.User.Emblems.getEmblems())
            {
                Badge.Value.SlotID = 0;
            }

            Engine.dbManager.DoQuery("UPDATE members_emblems SET slotid = '0' WHERE userid = '" + Session.User.ID + "'");

            for (int i = 0; i < 5; i++)
            {
                int Slot = Message.NextInt32();
                string Badge = Message.NextString();

                if (Badge.Length < 1)
                    continue;

                Session.User.Emblems.GetBadge(Badge).SlotID = Slot;

                Engine.dbManager.DoQuery("UPDATE members_emblems SET slotid = '" + Slot + "' WHERE badge = '" + Badge + "' AND userid = '" + Session.User.ID + "'");
            }

            fuseResponse.New(Opcodes.OpcodesOut.SendBadgeUpdate);
            fuseResponse.Append<int>(Session.User.ID);

            var Wearing = (from ia in Session.User.Emblems.getEmblems() where ia.Value.SlotID > 0 select ia.Value);

            fuseResponse.Append<int>(Wearing.Count());

            foreach (var Wear in Wearing)
            {
                fuseResponse.Append<int>(Wear.SlotID);
                fuseResponse.Append<string>(Wear.Badge);
            }

            if (Session.Room != null)
                Session.Room.SendData(fuseResponse);
            else
                fuseResponse.Send(Session);
        }

        public static void GetInventoryBadges(Message Message, Session Session)
        {
            var Wearing = (from i in Session.User.Emblems.getEmblems() where i.Value.SlotID > 0 select i.Value);
            
            fuseResponse.New(3462);
            fuseResponse.Append<int>(Session.User.Emblems.getEmblems().Count);

            foreach (var Emblems in Session.User.Emblems.getEmblems())
            {
                fuseResponse.Append<int>(Emblems.Value.ID);
                fuseResponse.Append<string>(Emblems.Value.Badge);
            }

            fuseResponse.Append<int>(Wearing.Count());

            foreach (var Badge in Wearing)
            {
                fuseResponse.Append<int>(Badge.SlotID);
                fuseResponse.Append<string>(Badge.Badge);
            }

            fuseResponse.Send(Session);
        }

        public static void GetInventory(Message Message, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendInventory);
            fuseResponse.Append<string>("S");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);

            fuseResponse.Append<int>(Session.User.Inventory.getFloorItems().Count());

            foreach(var Items in Session.User.Inventory.getFloorItems())
            {
                fuseResponse.Append<int>(Items.ID);
                fuseResponse.Append<string>("S");
                fuseResponse.Append<int>(Items.ID);
                fuseResponse.Append<int>(Items.GetFurni().sprite_id);
                fuseResponse.Append<int>(1);
                fuseResponse.Append<string>("");
                fuseResponse.Append<int>(0);
                fuseResponse.Append<bool>(false);
                fuseResponse.Append<bool>(false);
                fuseResponse.Append<bool>(false);
                fuseResponse.Append<bool>(false);
                fuseResponse.Append<int>(-1);
                fuseResponse.Append<string>("");
                fuseResponse.Append<int>(0);
            }

            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendInventory);
            fuseResponse.Append<string>("I");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);

            fuseResponse.Append<int>(Session.User.Inventory.getWallItems().Count());

            foreach (var Items in Session.User.Inventory.getWallItems())
            {
                fuseResponse.Append<int>(Items.ID);
                fuseResponse.Append<string>("I");
                fuseResponse.Append<int>(Items.ID);
                fuseResponse.Append<int>(Items.GetFurni().sprite_id);
                fuseResponse.Append<int>(1);
                fuseResponse.Append<string>("");
                fuseResponse.Append<int>(0);
                fuseResponse.Append<bool>(false);
                fuseResponse.Append<bool>(false);
                fuseResponse.Append<bool>(false);
                fuseResponse.Append<bool>(false);
                fuseResponse.Append<int>(-1);
                fuseResponse.Append<string>("");
                fuseResponse.Append<int>(0);
            }

            fuseResponse.Send(Session);
        }

        public static void GetPurse(Message Message, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendCreditBalance);
            fuseResponse.Append<string>(string.Format("{0}.0", Session.User.Coins));
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendActivityPoints);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(Session.User.Pixels);
            fuseResponse.Append<int>(105);
            fuseResponse.Append<int>(Session.User.OtherCurrency);
            fuseResponse.Send(Session);
        }

        public static void GetUser(Message Msg, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendUserInformation);
            fuseResponse.Append<int>(Session.User.ID);
            fuseResponse.Append<string>(Session.User.Username);
            fuseResponse.Append<string>(Session.User.Figure);
            fuseResponse.Append<string>(Session.User.Gender);
            fuseResponse.Append<string>("");
            fuseResponse.Append<string>(Session.User.Email.Split('@')[0]);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<int>(Session.User.Respect);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<string>("30-09-2012 16:17:34");
            fuseResponse.Append<string>("");
            fuseResponse.Send(Session);
        }

        public static void GetAchievements(Message Message, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendAchievementList);
            fuseResponse.Append<int>(Engine.GetHabboHotel.getAchievementManager.Achievements.Count);

            foreach (var Achievement in Engine.GetHabboHotel.getAchievementManager.Achievements.Values)
            {
                fuseResponse.Append<int>(Achievement.ID);
                fuseResponse.Append<int>(1);
                fuseResponse.Append<string>(Achievement.Type + "1");
                fuseResponse.Append<int>(0);
                fuseResponse.Append<int>(int.Parse(Achievement.Goals[0]));
                fuseResponse.Append<int>(int.Parse(Achievement.Rewards[0]));
                fuseResponse.Append<int>(0);
                fuseResponse.Append<int>(0);
                fuseResponse.Append<bool>(false);
                fuseResponse.Append<string>(Achievement.Category);
                fuseResponse.Append<string>("");
                fuseResponse.Append<int>(Achievement.Levels.Count());
                fuseResponse.Append<int>(0);
            }

            fuseResponse.Append<string>("");
            fuseResponse.Send(Session);
        }
    }
}
