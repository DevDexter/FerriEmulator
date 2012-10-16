using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Communication;

namespace Ferri_Emulator.Database.Mappings
{
    public class rooms
    {
        public Habbo_Hotel.Rooms.RoomModel Model;
        public virtual uint ID { get; set; }
        public virtual string Caption { get; set; }
        public virtual string Owner { get; set; }
        public virtual string Description { get; set; }
        public virtual int Category { get; set; }
        public virtual int State { get; set; }
        public virtual int Users_Now { get; set; }
        public virtual int Users_Max { get; set; }
        public virtual string Model_Name { get; set; }
        public virtual int Score { get; set; }
        public virtual string[] Tags { get; set; }
        public virtual string Password { get; set; }
        public virtual string Wallpaper { get; set; }
        public virtual string Floor { get; set; }
        public virtual string Landscape { get; set; }
        public virtual bool Allow_Pets { get; set; }
        public virtual bool Allow_Pets_Eat { get; set; }
        public virtual bool Allow_Walkthrough { get; set; }
        public virtual bool Allow_Hidewall { get; set; }
        public virtual bool Allow_Rightsoverride { get; set; }
        public virtual int Floorthickness { get; set; }
        public virtual int Wallthickness { get; set; }
        public virtual int GroupID { get; set; }

        internal void SendData(Messages.ServerMessage fuseResponse)
        {
            foreach (Session Session in Engine.RoomsLoaded[ID])
            {
                Session.SendPacket(fuseResponse);
            }
        }
    }
}
