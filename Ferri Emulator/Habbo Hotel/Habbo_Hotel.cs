using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Habbo_Hotel.Navigator;
using Ferri_Emulator.Habbo_Hotel.Catalogue;
using Ferri_Emulator.Habbo_Hotel.Items;
using Ferri_Emulator.Habbo_Hotel.Rooms;
using Ferri_Emulator.Habbo_Hotel.Groups;
using Ferri_Emulator.Habbo_Hotel.Achievements;

namespace Ferri_Emulator.Habbo_Hotel
{
    public class Habbo_Hotel
    {
        public FluentFrontpage getFrontpage = new FluentFrontpage();
        public CatalogManager getCatalogueManager = new CatalogManager();
        public ItemDefinition getItemDefinitions = new ItemDefinition();
        public RoomModel getRoomModels = new RoomModel();
        public FluentGroups getGroups = new FluentGroups();
        public AchievementManager getAchievementManager = new AchievementManager();

        public void LoadHH()
        {
            getFrontpage.CacheFrontpage();
            getCatalogueManager.CachePages();
            getCatalogueManager.CacheItems();
            getItemDefinitions.CacheDefinitions();
            getRoomModels.LoadAll();
            getGroups.CacheGroups();
            getAchievementManager.CacheAchievements();
        }
    }
}
