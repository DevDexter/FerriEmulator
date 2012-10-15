using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Database.Mappings;
using System.Data;

namespace Ferri_Emulator.Habbo_Hotel.Items
{
    public class ItemDefinition
    {
        public Dictionary<uint, furniture> Definitions;

        public void CacheDefinitions()
        {
            Definitions = new Dictionary<uint, furniture>();
            DateTime Start = DateTime.Now;

            DataTable Data = Engine.dbManager.ReadTable("SELECT * FROM furniture");
            var RowEnum = Data.Rows.GetEnumerator();

            while (RowEnum.MoveNext())
            {
                DataRow Row = (DataRow)RowEnum.Current;

                furniture furni = new furniture()
                {
                    id = (uint)Row["id"],
                    sprite_id = (int)Row["sprite_id"],
                    type = Row["type"].ToString()
                };

                Definitions.Add(furni.id, furni);
            }

            DateTime End = DateTime.Now;
            TimeSpan Expired = (End - Start);
            Engine.Logging.WriteTagLine("Cache", "Loaded {0} Item Definitions in {1} s and {2} ms", Definitions.Count, Expired.Seconds, Expired.Milliseconds);
        }
    }
}
