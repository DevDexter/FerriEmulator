using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Database.Mappings
{
    public class groups
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Badges { get; set; }
        public virtual string[] Members { get; set; }
        public virtual string Owner { get; set; }
        public virtual string Created { get; set; }
        public virtual string[] Admins { get; set; }
    }
}
