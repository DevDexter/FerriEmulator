using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Habbo_Hotel.Navigator;

namespace Ferri_Emulator.Habbo_Hotel
{
    public class Habbo_Hotel
    {
        public FluentFrontpage getFrontpage = new FluentFrontpage();

        public void LoadHH()
        {
            getFrontpage = new FluentFrontpage();
            getFrontpage.CacheFrontpage();
        }
    }
}
