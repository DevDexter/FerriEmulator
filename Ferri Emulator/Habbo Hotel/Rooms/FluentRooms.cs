using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Database.Mappings;
using System.Data;
using Ferri_Emulator.Messages;

namespace Ferri_Emulator.Habbo_Hotel.Rooms
{
    public class FluentRooms
    {
        public static List<rooms> GetByOwner(string Owner)
        {
            List<rooms> Rooms = new List<rooms>();
            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM rooms WHERE owner = '" + Owner + "'");
            var RowEnum = Data.Rows.GetEnumerator();

            while (RowEnum.MoveNext())
            {
                DataRow Row = (DataRow)RowEnum.Current;

                Rooms.Add(new rooms()
                {
                    ID = (uint)Row["id"],
                    Caption = Row["caption"].ToString(),
                    Description = Row["description"].ToString(),
                    Owner = Owner,
                    Category = (int)Row["category"],
                    Password = Row["password"].ToString(),
                    Model_Name = Row["model_name"].ToString(),
                    Floor = Row["floor"].ToString(),
                    Users_Max = (int)Row["users_max"],
                    Users_Now = (int)Row["users_now"],
                    Score = (int)Row["score"],
                    Tags = Row["tags"].ToString().Split(';'),
                    State = GetState(Row["state"].ToString()),
                    Allow_Hidewall = (Row["allow_hidewall"].ToString() == "1"),
                    Allow_Pets = (Row["allow_pets"].ToString() == "1"),
                    Allow_Pets_Eat = (Row["allow_pets_eat"].ToString() == "1"),
                    Allow_Rightsoverride = (Row["allow_rightsoverride"].ToString() == "1"),
                    Allow_Walkthrough = (Row["allow_walkthrough"].ToString() == "1"),
                    Floorthickness = (int)Row["floorthickness"],
                    Wallpaper = Row["wallpaper"].ToString(),
                    Wallthickness = (int)Row["wallthickness"],
                    Landscape = Row["landscape"].ToString()
                });
            }

            return Rooms;
        }

        private static int GetState(string p)
        {
            switch (p)
            {
                case "open":
                    return 0;
                case "locked":
                    return 1;
                case "password":
                    return 2;
                default:
                    return 0;
            }
        }

        internal static void Serialize(ServerMessage Msg, rooms Room)
        {
            Msg.Append<uint>(Room.ID);
            Msg.Append<bool>(false);
            Msg.Append<string>(Room.Caption);
            Msg.Append<bool>(true);
            Msg.Append<int>(Engine.dbManager.ReadInt("SELECT id FROM members WHERE username = '" + Room.Owner + "'"));
            Msg.Append<string>(Room.Owner);
            Msg.Append<int>(Room.State);
            Msg.Append<int>(Room.Users_Now);
            Msg.Append<int>(Room.Users_Max);
            Msg.Append<string>(Room.Description);
            Msg.Append<int>(0);
            Msg.Append<int>(0);
            Msg.Append<int>(Room.Score);
            Msg.Append<int>(Room.Category);
            Msg.Append<int>(0);
            Msg.Append<int>(0);
            Msg.Append<string>("");
            Msg.Append<int>(Room.Tags.Length);
            Msg.Append<string[]>(Room.Tags);
            Msg.Append<int>(0);
            Msg.Append<int>(0);
            Msg.Append<int>(0);
            Msg.Append<bool>(true);
            Msg.Append<bool>(true);
        }
    }
}
