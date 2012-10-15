using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Database.Mappings
{
    public class catalogpages
    {
        public virtual int id { get; set; }
        public virtual int category_id { get; set; }
        public virtual string name { get; set; }
        public virtual string layout_images { get; set; }
        public virtual string layout_texts { get; set; }
        public virtual int rank_required { get; set; }
        public virtual int club_required { get; set; }
        public virtual string layout { get; set; }
        public virtual bool has_content { get; set; }
        public virtual int style_color { get; set; }
        public virtual int style_icon { get; set; }
        public virtual bool is_visible { get; set; }
    }
}
