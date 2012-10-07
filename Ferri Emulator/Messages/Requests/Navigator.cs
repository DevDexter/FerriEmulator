using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri.Kernel.Network;
using Ferri_Emulator.SS;
using Ferri_Emulator.Database.Mappings;
using Ferri_Emulator.Habbo_Hotel.Rooms;

namespace Ferri_Emulator.Messages.Requests
{ 
    class Navigator : Data
    {
        public static void NavigatorFrontpage(Message Msg, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendNavigatorFrontpage);
            fuseResponse.Append<int>(Engine.GetHabboHotel.getFrontpage.getCount());

            foreach (var Item in Engine.GetHabboHotel.getFrontpage.getList().Values)
            {
                fuseResponse.Append<int>(Item.ID);
                fuseResponse.Append<string>(Item.Name);
                fuseResponse.Append<string>(Item.Description);
                fuseResponse.Append<int>(Item.BannerLength);
                fuseResponse.Append<string>(Item.Name);
                fuseResponse.Append<string>(Item.Image);
                fuseResponse.Append<int>(Item.Category_ID);
                fuseResponse.Append<int>(0);
                fuseResponse.Append<int>(Item.Type);

                switch (Item.Type)
                {
                    case 1:
                        {
                            fuseResponse.Append<string>("tag");
                            break;
                        }
                    case 4:
                        {
                            fuseResponse.Append<bool>(false);
                            break;
                        }
                }
            }

            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Send(Session);
        }

        public static void OwnRooms(Message Msg, Session Session)
        {
            var All = FluentRooms.GetByOwner(Session.User.Username);
            var RoomsEnum = All.GetEnumerator();

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomDataNavigator);
            fuseResponse.Append<int>(5);
            fuseResponse.Append<string>("");
            fuseResponse.Append<int>(All.Count);

            while (RoomsEnum.MoveNext())
            {
                var Room = (rooms)RoomsEnum.Current;
                FluentRooms.Serialize(fuseResponse, Room);
            }

            fuseResponse.Append<bool>(false);
            fuseResponse.Send(Session);
        }
    }
}
