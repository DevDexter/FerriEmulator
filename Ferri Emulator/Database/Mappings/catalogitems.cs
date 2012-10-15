using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Messages;

namespace Ferri_Emulator.Database.Mappings
{
    public class catalogitems
    {
        public virtual int id { get; set; }
        public virtual string productname { get; set; }
        public virtual int credits { get; set; }
        public virtual int activitypoints { get; set; }
        public virtual int activitypointtype { get; set; }
        public virtual string[] amounts { get; set; }
        public virtual string[] items { get; set; }
        public virtual int vipneed { get; set; }
        public virtual string extradata { get; set; }
        public virtual int pageid { get; set; }

        internal void Serialize(ServerMessage fuseResponse)
        {
            fuseResponse.Append<int>(id);
            fuseResponse.Append<string>(productname);
            fuseResponse.Append<int>(credits);
            fuseResponse.Append<int>(activitypoints);
            fuseResponse.Append<int>(activitypointtype);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<int>(items.Count());

            for(int i = 0; i < items.Count(); i++)
            {
                furniture furni = Engine.GetHabboHotel.getItemDefinitions.Definitions[uint.Parse(items[i])];

                fuseResponse.Append<string>(furni.type);
                fuseResponse.Append<int>(furni.sprite_id);
                fuseResponse.Append<string>("");
                fuseResponse.Append<int>(int.Parse(amounts[i]));
                fuseResponse.Append<int>(-1);
                fuseResponse.Append<bool>(false);
            }

            fuseResponse.Append<int>(vipneed);
            fuseResponse.Append<bool>((items.Count() < 2));
        }
    }
}
