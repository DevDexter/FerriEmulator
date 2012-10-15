using Ferri.Kernel.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferri.Kernel.Pathfinding
{
    public class Pathfinder
    {
        private Point[] AvaliablePoints;

        private int MapSizeX = 0;
        private int MapSizeY = 0;

        public TileState[,] Squares;

        private Session Session;

        public int GoX;
        public int GoY;

        public Pathfinder(Session Session)
        {
            AvaliablePoints = new Point[]
				{ 
				new Point(0, -1),
				new Point(0, 1),
				new Point(1, 0),
				new Point(-1, 0),
				new Point(1, -1),
				new Point(-1, 1),
				new Point(1, 1),
				new Point(-1, -1)
				};

            this.Session = Session;

            MapSizeX = Session.Room.Model.MapSizeX;
            MapSizeY = Session.Room.Model.MapSizeY;
            Squares = Session.Room.Model.mTileState;
        }

        public List<Coord> PathCollection()
        {
            List<Coord> CoordSquares = new List<Coord>();

            int UserX = Session.User.RoomUser.CoordX;
            int UserY = Session.User.RoomUser.CoordY;

            GoX = Session.User.RoomUser.GoalX;
            GoY = Session.User.RoomUser.GoalY;

            Coord LastCoord = new Coord(-200, -200);
            int Trys = 0;

            while (true)
            {
                Trys++;
                int StepsToWalk = 10000;
                foreach (Point MovePoint in AvaliablePoints)
                {
                    int newX = MovePoint.X + UserX;
                    int newY = MovePoint.Y + UserY;

                    if (newX >= 0 && newY >= 0 && MapSizeX > newX && MapSizeY > newY && Squares[newX, newY] == TileState.Open/* && !User.getRoomUser().getCurrentRoom().CheckUserCoordinates(User, newX, newY) && !CheckFurniCoordinates(newX, newY)*/)
                    {
                        Coord pCoord = new Coord(newX, newY);
                        pCoord.PositionDistance = DistanceBetween(newX, newY, GoX, GoY);
                        pCoord.ReversedPositionDistance = DistanceBetween(GoX, GoY, newX, newY);

                        if (StepsToWalk > pCoord.PositionDistance)
                        {
                            StepsToWalk = pCoord.PositionDistance;
                            LastCoord = pCoord;
                        }
                    }
                }
                if (Trys >= 200)
                    return null;

                else if (LastCoord.X == -200 && LastCoord.Y == -200)
                    return null;

                UserX = LastCoord.X;
                UserY = LastCoord.Y;
                CoordSquares.Add(LastCoord);

                if (UserX == GoX && UserY == GoY)
                    break;
            }
            return CoordSquares;
        }

        public Boolean CheckFurniCoordinates(int X, int Y)
        {
            return false;
        }
        private int DistanceBetween(int currentX, int currentY, int goX, int goY)
        {
            return Math.Abs(currentX - goX) + Math.Abs(currentY - goY);
        }
    }
}
