using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Messages;

namespace Ferri_Emulator.Habbo_Hotel.Users
{
    public class RoomUser
    {
        public virtual int CoordX { get; set; }
        public virtual int CoordY { get; set; }
        public virtual double CoordZ { get; set; }
        public virtual int Effect { get; set; }
        public virtual int Dance { get; set; }
        public virtual bool Idle { get; set; }
        public virtual int RotationHead { get; set; }
        public virtual int RotationBody { get; set; }
        public virtual int GoalX { get; set; }
        public virtual int GoalY { get; set; }
        public virtual bool RequestWalk { get; set; }
        public virtual bool IsWalking { get; set; }
        public virtual bool CompleteWalk { get; set; }

        public ServerMessage UpdateState(int Id, string Status)
        {
            ServerMessage Message = new ServerMessage(Opcodes.OpcodesOut.SendUserStatus);
            Message.Append<int>(1);
            Message.Append<int>(Id);
            Message.Append<int>(CoordX);
            Message.Append<int>(CoordY);
            Message.Append<string>(CoordZ.ToString());
            Message.Append<int>(RotationHead);
            Message.Append<int>(RotationBody);
            Message.Append<string>("/" + Status + "//");
            return Message;
        }

        internal int CalculateRot(int X1, int Y1, int X2, int Y2)
        {
            int Rotation = 0;

            if (X1 > X2 && Y1 > Y2)
                Rotation = 7;
            else if (X1 < X2 && Y1 < Y2)
                Rotation = 3;
            else if (X1 > X2 && Y1 < Y2)
                Rotation = 5;
            else if (X1 < X2 && Y1 > Y2)
                Rotation = 1;
            else if (X1 > X2)
                Rotation = 6;
            else if (X1 < X2)
                Rotation = 2;
            else if (Y1 < Y2)
                Rotation = 4;
            else if (Y1 > Y2)
                Rotation = 0;

            return Rotation;
        }
    }
}
