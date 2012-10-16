using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Habbo_Hotel.Rooms;
using Ferri_Emulator.Habbo_Hotel.Users;
using System.Threading;
using Ferri.Kernel.Pathfinding;
using Ferri_Emulator.Utilities;
using Ferri_Emulator.Communication;

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

        public static void GetGroupData(Message Msg, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendRoomGroupBadge);
            fuseResponse.Append<int>((Session.User.favouritegroup > 0) ? 1 : 0);

            if (Session.User.favouritegroup > 0)
            {
                var Group = Engine.GetHabboHotel.getGroups.Groups[Session.User.favouritegroup];
                fuseResponse.Append<int>(Group.ID);
                fuseResponse.Append<string>(Group.Badges);
            }

            fuseResponse.Send(Session);
        }

        public static void GetRoomModeldata(Message Msg, Session Session)
        {
            Session.Room.Model = Engine.GetHabboHotel.getRoomModels.Models[Session.Room.Model_Name];

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomModeldata);
            Session.Room.Model.SerializeFirst(fuseResponse);
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomRelativeModeldata);
            Session.Room.Model.SerializeSecond(fuseResponse);
            fuseResponse.Send(Session);
        }

        public static void GetEndEnterRoom(Message Msg, Session Session)
        {
            fuseResponse.New(537);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("lol");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<bool>(true);
            fuseResponse.Send(Session);

            fuseResponse.New(632);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("lol");
            fuseResponse.Append<string>("uhaveshittyballs");
            fuseResponse.Append<string>("");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("lolroom");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<bool>(false);
            fuseResponse.Append<string>("now");
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<string>("tehowner");
            fuseResponse.Append<bool>(false);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<int>(0);
            fuseResponse.Send(Session);

            Session.User.RoomUser = new RoomUser()
            {
                CoordX = Session.Room.Model.DoorX,
                CoordY = Session.Room.Model.DoorY,
                CoordZ = Session.Room.Model.DoorZ,
                Dance = 0,
                Effect = 0,
                Idle = false,
                RotationBody = 2,
                RotationHead = 2
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
                fuseResponse.Append<int>(User.User.favouritegroup);
                fuseResponse.Append<int>(3); // ??
                fuseResponse.Append<string>(Engine.GetHabboHotel.getGroups.Groups[User.User.favouritegroup].Name);
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
            fuseResponse.Append<int>(Session.User.favouritegroup);
            fuseResponse.Append<int>(3); // ??
            fuseResponse.Append<string>(Engine.GetHabboHotel.getGroups.Groups[Session.User.favouritegroup].Name);
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

        public static void Move(Message Msg, Session Session)
        {
            if (Session.User.RoomUser.IsWalking)
            {
                Session.MoveThread.Abort();
                Session.MoveThread = null;
                Session.User.RoomUser.IsWalking = false;
            }

            int X = Msg.NextInt32();
            int Y = Msg.NextInt32();

            if (X == Session.Room.Model.DoorX && Y == Session.Room.Model.DoorY)
            {
                fuseResponse.New(Opcodes.OpcodesOut.SendUserOutofRoom);
                Session.Room.SendData(fuseResponse);

                Session.MoveThread.Abort();
                Session.MoveThread = null;
                return;
            }

            Session.User.RoomUser.GoalX = X;
            Session.User.RoomUser.GoalY = Y;

            Session.MoveThread = new Thread(new ParameterizedThreadStart(OnWalk));
            Session.MoveThread.Start(Session);
        }

        public static void OnWalk(object session)
        {
            var Session = session as Session;

            Pathfinder Path = new Pathfinder(Session);

            if (Path == null)
            {
                return;
            }

            foreach (var Coord in Path.PathCollection())
            {
                /*
                 * Clear existing statuses, updates iswalking
                 */
                Session.User.RoomUser.IsWalking = true;

                /*
                 * Set rotation
                 */
                int Rot = Session.User.RoomUser.CalculateRot(Session.User.RoomUser.CoordX, Session.User.RoomUser.CoordY, Coord.X, Coord.Y);
                Session.User.RoomUser.RotationBody = Rot;
                Session.User.RoomUser.RotationHead = Rot;

                /*
                 * Update walk status
                 */

                Session.Room.SendData(Session.User.RoomUser.UpdateState(Session.User.ID, "mv " + Coord.X + "," + Coord.Y + ",0.0"));

                /*
                 * Set coordinates you moved to, to your current cord
                 */
                Session.User.RoomUser.CoordX = Coord.X;
                Session.User.RoomUser.CoordY = Coord.Y;

                /*
                 * Set height 
                 */
                // User.Z = Double.toString(Session.Room.Model.getSquareHeight()[next.X][next.Y]);
                //User.Walking = true;

                /*
                 * Make thread sleep.
                 */
                Thread.Sleep(500);

                /*
                 * When user reaches the end we cancel the thread.
                 */
                if (Session.User.RoomUser.GoalX == Coord.X && Session.User.RoomUser.GoalY == Coord.Y)
                {
                    Session.Room.SendData(Session.User.RoomUser.UpdateState(Session.User.ID, ""));
                    Session.User.RoomUser.IsWalking = false;
                    Session.MoveThread.Abort();
                    break;
                }
            }
        }

        public static void Talk(Message Msg, Session Session)
        {
            string Message = Msg.NextString();
            int Type = Msg.NextInt32(); // Never knew you could do bot things
            int Color = Msg.NextInt32();

            if (Message.StartsWith("b: "))
            {
                Type = 2;
                Message = Message.Replace("b: ", "");
            }
            else if (Message.StartsWith("n: "))
            {
                Type = 1;
                Message = Message.Replace("n: ", "");
            }

            //[0][0][0]v[0][0][0][0][0][3]hey[0][0][0][0][0][0][0][0][0][0][0][0][0][0][0][0]
            fuseResponse.New(Opcodes.OpcodesOut.SendRoomChat);
            fuseResponse.Append<int>(Session.User.ID);
            fuseResponse.Append<string>(Message);            
            fuseResponse.Append<int>(Chat.GetChatEmoticon(Message));
            fuseResponse.Append<int>(Type); // Type (0 = normal, 1 = notification, 2 = BOT, 3 = colored)
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(Color); // ?
            Session.Room.SendData(fuseResponse);
        }

        public static void GetInventoryBots(Message Msg, Session Session)
        {//66646
            fuseResponse.New(1259);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("hi");
            fuseResponse.Append<string>("M");
            fuseResponse.Append<string>(Session.User.Figure);
            fuseResponse.Send(Session);
        }

        public static void GetRoomUserTags(Message Message, Session Session)
        {
            int UserID = Message.NextInt32();

            var user = (from i in Engine.RoomsLoaded[Session.Room.ID] where i.User.ID == UserID select i);
            var Ses = user.First();
            //??

        }

        public static void GetRoomUserBadges(Message Message, Session Session)
        {
            int UserID = Message.NextInt32();

            var user = (from i in Engine.RoomsLoaded[Session.Room.ID] where i.User.ID == UserID select i);
            var Ses = user.First();
            var Wearing = (from i in Ses.User.Emblems.getEmblems() where i.Value.SlotID > 0 select i.Value);

            fuseResponse.New(Opcodes.OpcodesOut.SendRoomUserBadges);
            fuseResponse.Append<int>(UserID);

            fuseResponse.Append<int>(Wearing.Count());

            foreach (var Wear in Wearing)
            {
                fuseResponse.Append<int>(Wear.SlotID);
                fuseResponse.Append<string>(Wear.Badge);
            }

            fuseResponse.Send(Session);
        }
    }
}
