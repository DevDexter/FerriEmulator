using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Database.Mappings
{
    public class frontpage
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int BannerLength { get; set; }
        public virtual string Image { get; set; }
        public virtual int Type { get; set; }
        public virtual int Category_ID { get; set; }
        public virtual bool Recommended { get; set; }
    }
}
