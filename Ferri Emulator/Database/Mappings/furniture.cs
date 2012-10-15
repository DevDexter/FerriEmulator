using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Database.Mappings
{
    public class furniture
    {
        public virtual uint id { get; set; }
        public virtual int sprite_id { get; set; }
        public virtual string type { get; set; }
    }
}
