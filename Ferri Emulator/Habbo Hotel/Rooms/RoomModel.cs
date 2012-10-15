using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Ferri_Emulator.Messages;
using Ferri.Kernel.Pathfinding;

namespace Ferri_Emulator.Habbo_Hotel.Rooms
{
    public enum SquareState
    {
        OPEN = 0,
        BLOCKED = 1,
        SEAT = 2
    }

    public class RoomModel
    {
        public string Name;

        public int DoorX;
        public int DoorY;
        public double DoorZ;
        public int DoorOrientation;

        public string Heightmap;

        public int MapSizeX = 0;
        public int MapSizeY = 0;

        public string StaticFurniMap;

        public bool ClubOnly;
        private List<string> Lines = new List<string>();
        private string[] tmpHeightmap = new string[9999];

        public Dictionary<string, RoomModel> Models = new Dictionary<string, RoomModel>();
        public TileState[,] mTileState;
        public int[,] mFloorHeight;
        private bool[,] RoomUnit;
        private sbyte[,] LogicalHeightMap;

        public RoomModel() { }

        public void LoadAll()
        {
            var Started = DateTime.Now;
            var mData = Engine.dbManager.ReadTable("SELECT * FROM system_modeldata");

            foreach (DataRow Data in mData.Rows)
            {
                var Model = new RoomModel((string)Data["name"],
          (int)Data["door_x"],
              (int)Data["door_y"],
              (Double)Data["door_z"],
              (int)Data["door_direction"],
              (string)Data["modeldata"],
              "", false);

                Models.Add(Model.Name, Model);
            }

            var Expire = (DateTime.Now - Started);
            Engine.Logging.WriteTagLine("Cache", "Loaded {0} Room Models in {1} s and {2} ms", Models.Count, Expire.Seconds, Expire.Milliseconds);
        }

        public RoomModel(string Name, int DoorX, int DoorY, double DoorZ, int DoorOrientation, string Heightmap, string StaticFurniMap, bool ClubOnly)
        {
            this.Name = Name;

            this.DoorX = DoorX;
            this.DoorY = DoorY;
            this.DoorZ = DoorZ;
            this.DoorOrientation = DoorOrientation;

            this.Heightmap = Heightmap.ToLower();
            this.StaticFurniMap = StaticFurniMap;

            this.ClubOnly = ClubOnly;

            Setup();
        }
        internal void Setup()
        {
            Lines = new List<string>();

            Heightmap = Heightmap.Replace(Convert.ToChar(10).ToString(), "");
            string[] splitRawmap = Heightmap.Split("\r\n".ToCharArray());

            foreach (string s in splitRawmap)
            {
                Lines.Add(s);
            }

            this.MapSizeX = Lines[0].Length;
            this.MapSizeY = Lines.Count;

            this.mTileState = new TileState[MapSizeX, MapSizeY];
            this.mFloorHeight = new int[MapSizeX, MapSizeY];

            for (int y = 0; y < MapSizeY; y++)
            {
                for (int x = 0; x < MapSizeX; x++)
                {
                    string value = Lines[y][x].ToString().ToLower();

                    mTileState[x, y] = (value == "x" ? TileState.Blocked : TileState.Open);
                    mFloorHeight[x, y] = (value == "x" ? 0 : int.Parse(value));
                    RoomUnit = new bool[x, y];
                    LogicalHeightMap = new sbyte[x, y];
                }
            }
        }

        internal void SerializeFirst(ServerMessage Message)
        {
            string Build = "";

            foreach (string s in Lines)
            {
                Build += (s + Convert.ToChar(13));
            }

            Message.Append<string>(Build);
        }

        internal void SerializeSecond(ServerMessage Message)
        {
            string Build = "";

            for (int y = 0; y < MapSizeY; y++)
            {
                for (int x = 0; x < MapSizeX; x++)
                {
                    string Square = Lines[y].Substring(x, 1).Trim().ToLower();

                    if (DoorX == x && DoorY == y)
                    {
                        Square = DoorZ.ToString();
                    }

                    Build += Square;
                }

                Build += Convert.ToChar(13);
            }

            string lol = Build.ToString();
            Message.Append<string>(lol);
        }
    }

}
