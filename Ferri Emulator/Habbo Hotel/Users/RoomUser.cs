using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
