using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Ferri_Emulator.Messages;

namespace Ferri_Emulator.Habbo_Hotel.Rooms
{
    public class RoomModel
    {
        internal readonly string Id;
        internal string RawMap;

        internal List<string> Lines;

        internal int MaxX = new int();
        internal int MaxY = new int();

        internal int DoorX = new int();
        internal int DoorY = new int();
        internal double DoorZ = new double();
        internal int DoorRot = new int();

        internal RoomModel(string Model, bool SQL = true)
        {
            try
            {
                if (SQL)
                {
                    DataRow Row = Engine.dbManager.ReadRow("SELECT * FROM system_modeldata WHERE name = '" + Model + "'");

                    this.Id = Row["name"].ToString();
                    this.RawMap = Row["modeldata"].ToString();
                    this.DoorX = (int)Row["door_x"];
                    this.DoorY = (int)Row["door_y"];
                    this.DoorZ = (double)Row["door_z"];
                    this.DoorRot = (int)Row["door_direction"];

                    Setup();
                }
                else
                {
                    this.Id = "model_a";
                    this.RawMap = "xxxxxxxxxxxx" + Convert.ToChar(13) +
                                  "xxxx00000000" + Convert.ToChar(13) +
                                  "xxxx00000000" + Convert.ToChar(13) +
                                  "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxx00000000" + Convert.ToChar(13) +
                                   "xxxxxxxxxxxx" + Convert.ToChar(13) +
                                   "xxxxxxxxxxxx" + Convert.ToChar(13);

                    this.DoorX = 3;
                    this.DoorY = 5;
                    this.DoorZ = 0;
                    this.DoorRot = 2;

                    Setup();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        internal void Setup()
        {
            Lines = new List<string>();

            RawMap = RawMap.Replace(Convert.ToChar(10).ToString(), "");
            string[] splitRawmap = RawMap.Split("\r\n".ToCharArray());

            foreach (string s in splitRawmap)
            {
                Lines.Add(s);
            }

            this.MaxX = Lines[0].Length;
            this.MaxY = Lines.Count;
        }

        internal void SerializeFirst(ServerMessage Message)
        {
            StringBuilder Build = new StringBuilder();

            for (int y = 0; y < MaxY; y++)
            {
                Build.Append(Lines[y]);
                Build.Append(Convert.ToChar(13).ToString());
            }

            Message.Append<string>(Build.ToString());
        }

        internal void SerializeSecond(ServerMessage Message)
        {
            StringBuilder Build = new StringBuilder();

            for (int y = 0; y < MaxY; y++)
            {
                for (int x = 0; x < MaxX; x++)
                {
                    string Square = Lines[y].Substring(x, 1).Trim().ToLower();

                    if (DoorX == x && DoorY == y)
                    {
                        Square = DoorZ.ToString();
                    }

                    Build.Append(Square);
                }

                Build.Append(Convert.ToChar(13).ToString());
            }

            Message.Append<string>(Build.ToString());
        }
    }

}
