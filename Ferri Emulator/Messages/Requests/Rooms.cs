using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri.Kernel.Network;
using Ferri_Emulator.SS;
using Ferri_Emulator.Habbo_Hotel.Rooms;
using Ferri_Emulator.Habbo_Hotel.Users;

namespace Ferri_Emulator.Messages.Requests
{
    public class Rooms : Data
    {
        public static void BeginEnterRoom(Message Msg, Session Session)
        {
            int ID = Msg.NextInt32();
            var Rooms = FluentRooms.GetById(ID);
            var Room = Rooms[0];
            string Password = Msg.NextString();

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomInitialize);
            fuseResponse.Send(Session);

            if (Room.State == 1)
            {
                fuseResponse.New(Opcodes.OpcodesOut.SendUserOutofRoom);
                fuseResponse.Send(Session);

                fuseResponse.New(Opcodes.OpcodesOut.SendDoorbellNoAnswer);
                fuseResponse.Send(Session);
            }

            if (Room.State == 2)
            {
                if (Password != Room.Password)
                {
                    fuseResponse.New(Opcodes.OpcodesOut.SendUserOutofRoom);
                    fuseResponse.Send(Session);

                    fuseResponse.New(Opcodes.OpcodesOut.SendWrongPassword);
                    fuseResponse.Append<int>(-100002);
                    fuseResponse.Send(Session);
                }
            }

            Session.Room = Room;

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomModelInfo);
            fuseResponse.Append<string>(Session.Room.Model_Name);
            fuseResponse.Append<uint>(Session.Room.ID);
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomDecoration);
            fuseResponse.Append<string>("landscape");
            fuseResponse.Append<string>("0.0");
            fuseResponse.Send(Session);

            if (Session.Room.Wallpaper != "0.0")
            {
                fuseResponse.New(Opcodes.OpcodesOut.SendRoomDecoration);
                fuseResponse.Append<string>("wallpaper");
                fuseResponse.Append<string>(Session.Room.Wallpaper);
                fuseResponse.Send(Session);
            }

            if (Session.Room.Floor != "0.0")
            {
                fuseResponse.New(Opcodes.OpcodesOut.SendRoomDecoration);
                fuseResponse.Append<string>("floor");
                fuseResponse.Append<string>(Session.Room.Floor);
                fuseResponse.Send(Session);
            }

            if (Session.Room.Owner == Session.User.Username)
            {
                fuseResponse.New(Opcodes.OpcodesOut.SendRoomUserRightLevel);
                fuseResponse.Append<int>(4);
                fuseResponse.Send(Session);

                fuseResponse.New(Opcodes.OpcodesOut.SendRoomOwnerShip);
                fuseResponse.Send(Session);
            }
            else
            {
                fuseResponse.New(Opcodes.OpcodesOut.SendRoomUserRightLevel);
                fuseResponse.Append<int>(0);
                fuseResponse.Send(Session);
            }

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomScore);
            fuseResponse.Append<int>(Session.Room.Score);
            fuseResponse.Append<bool>((Session.Room.Owner == Session.User.Username) ? false : Session.User.RatedRooms.Contains(ID) ? false : true);
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomEventData);
            fuseResponse.Append<string>("-1");
            fuseResponse.Send(Session);
        }

        public static void GetRoomModeldata(Message Msg, Session Session)
        {
            Session.Room.Model = new RoomModel(Session.Room.Model_Name);

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomModeldata);
            Session.Room.Model.SerializeFirst(fuseResponse);
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomRelativeModeldata);
            Session.Room.Model.SerializeSecond(fuseResponse);
            fuseResponse.Send(Session);
        }

        public static void GetEndEnterRoom(Message Msg, Session Session)
        {
            Session.User.RoomUser = new RoomUser()
            {
                CoordX = Session.Room.Model.DoorX,
                CoordY = Session.Room.Model.DoorY,
                CoordZ = Session.Room.Model.DoorZ,
                Dance = 0,
                Effect = 0,
                Idle = false
            };

            if (Engine.RoomsLoaded.ContainsKey(Session.Room.ID))
            {
                Engine.RoomsLoaded[Session.Room.ID].Add(Session);
            }
            else
            {
                Engine.RoomsLoaded.Add(Session.Room.ID, new List<Session>());
                Engine.RoomsLoaded[Session.Room.ID].Add(Session);
            }

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomUser);
            fuseResponse.Append<int>(Engine.RoomsLoaded[Session.Room.ID].Count);

            foreach (var User in Engine.RoomsLoaded[Session.Room.ID])
            {
                fuseResponse.Append<int>(User.User.ID);
                fuseResponse.Append<string>(User.User.Username);
                fuseResponse.Append<string>(User.User.Motto);
                fuseResponse.Append<string>(User.User.Figure);
                fuseResponse.Append<int>(User.User.ID);
                fuseResponse.Append<int>(User.User.RoomUser.CoordX);
                fuseResponse.Append<int>(User.User.RoomUser.CoordY);
                fuseResponse.Append<string>(User.User.RoomUser.CoordZ.ToString());
                fuseResponse.Append<int>(2);
                fuseResponse.Append<int>(1);
                fuseResponse.Append<string>(User.User.Gender);
                fuseResponse.Append<int>(0);
                fuseResponse.Append<int>(0);
                fuseResponse.Append<string>("");
                fuseResponse.Append<string>("");
                fuseResponse.Append<int>(0);
            }

            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomUser);
            fuseResponse.Append<int>(1);

            fuseResponse.Append<int>(Session.User.ID);
            fuseResponse.Append<string>(Session.User.Username);
            fuseResponse.Append<string>(Session.User.Motto);
            fuseResponse.Append<string>(Session.User.Figure);
            fuseResponse.Append<int>(Session.User.ID);
            fuseResponse.Append<int>(Session.User.RoomUser.CoordX);
            fuseResponse.Append<int>(Session.User.RoomUser.CoordY);
            fuseResponse.Append<string>(Session.User.RoomUser.CoordZ.ToString());
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>(Session.User.Gender);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<string>("");
            fuseResponse.Append<string>("");
            fuseResponse.Append<int>(0);

            Session.Room.SendData(fuseResponse);

            fuseResponse.New(Opcodes.OpcodesOut.SendUserStatus);
            fuseResponse.Append<int>(Engine.RoomsLoaded[Session.Room.ID].Count);

            foreach (Session S in Engine.RoomsLoaded[Session.Room.ID])
            {
                fuseResponse.Append<int>(S.User.ID);
                fuseResponse.Append<int>(S.User.RoomUser.CoordX);
                fuseResponse.Append<int>(S.User.RoomUser.CoordY);
                fuseResponse.Append<string>(S.User.RoomUser.CoordZ);
                fuseResponse.Append<int>(2);
                fuseResponse.Append<int>(2);
                fuseResponse.Append<string>("//");
            }

            Session.Room.SendData(fuseResponse);

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomStatus);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<uint>(Session.Room.ID);
            fuseResponse.Append<bool>(Session.Room.Owner == Session.User.Username);
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendInRoomDetails);
            fuseResponse.Append<bool>(true);
            FluentRooms.Serialize(fuseResponse, Session.Room);
            fuseResponse.Append<bool>(false);
            fuseResponse.Append<bool>(false);
            fuseResponse.Append<bool>(false);
            fuseResponse.Send(Session);
        }
    }
}
