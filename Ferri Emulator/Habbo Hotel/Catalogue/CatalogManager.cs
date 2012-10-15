using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Database.Mappings;
using System.Data;

namespace Ferri_Emulator.Habbo_Hotel.Catalogue
{
    public class CatalogManager
    {
        public Dictionary<int, catalogpages> Pages;
        public Dictionary<int, catalogitems> Items;

        public void CachePages()
        {
            Pages = new Dictionary<int, catalogpages>();
            DateTime Start = DateTime.Now;

            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM catalogue_pages");
            var RowEnum = Data.Rows.GetEnumerator();

            while (RowEnum.MoveNext())
            {
                DataRow Row = (DataRow)RowEnum.Current;

                catalogpages Page = new catalogpages()
                {
                    id = (int)Row["id"],
                    category_id = (int)Row["category_id"],
                    name = Row["name"].ToString(),
                    layout_images = Row["layout_images"].ToString(),
                    layout_texts = Row["layout_texts"].ToString(),
                    layout = Row["layout"].ToString(),
                    club_required = (int)Row["club_required"],
                    has_content = (Row["has_content"].ToString() == "1"),
                    is_visible = (Row["is_visible"].ToString() == "1"),
                    rank_required = (int)Row["rank_required"],
                    style_color = (int)Row["style_color"],
                    style_icon = (int)Row["style_icon"]
                };

                Pages.Add(Page.id, Page);
            }

            DateTime End = DateTime.Now;
            TimeSpan Expired = (End - Start);
            Engine.Logging.WriteTagLine("Cache", "Loaded {0} Catalogue Pages in {1} s and {2} ms", Pages.Count, Expired.Seconds, Expired.Milliseconds);
        }

        public void CacheItems()
        {
            Items = new Dictionary<int, catalogitems>();
            DateTime Start = DateTime.Now;

            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM catalogue_items");
            var RowEnum = Data.Rows.GetEnumerator();

            while (RowEnum.MoveNext())
            {
                DataRow Row = (DataRow)RowEnum.Current;

                catalogitems Item = new catalogitems()
                {
                    id = (int)Row["id"],
                    productname = Row["productname"].ToString(),
                    credits = (int)Row["credits"],
                    activitypoints = (int)Row["activitypoints"],
                    activitypointtype = int.Parse(Row["activitypointtype"].ToString()),
                    amounts = Row["amounts"].ToString().Split(';'),
                    items = Row["items"].ToString().Split(';'),
                    extradata = Row["extradata"].ToString(),
                    vipneed = int.Parse(Row["vipneed"].ToString()),
                    pageid = (int)Row["pageid"]
                };

                Items.Add(Item.id, Item);
            }

            DateTime End = DateTime.Now;
            TimeSpan Expired = (End - Start);
            Engine.Logging.WriteTagLine("Cache", "Loaded {0} Catalogue Items in {1} s and {2} ms", Items.Count, Expired.Seconds, Expired.Milliseconds);
        }
    }
}
