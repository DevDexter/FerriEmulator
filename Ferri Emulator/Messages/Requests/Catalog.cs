using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.SS;
using Ferri.Kernel.Network;

namespace Ferri_Emulator.Messages.Requests
{
    class Catalog : Data
    {
        public static void CatalogTabs(Message Msg, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendShopTabs);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(-1);
            fuseResponse.Append<string>("root");
            fuseResponse.Append<string>("");
            fuseResponse.Append<int>(1);

            fuseResponse.Append<bool>(true);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>(" front_page");
            fuseResponse.Append<string>(" Front Page");
            fuseResponse.Append<int>(0);

            fuseResponse.Append<bool>(true);
            fuseResponse.Send(Session);
        }

        public static void GetRecyclerRewards(Message Msg, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendRecyclerRewards);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<int>(10);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("matic_cont_duck");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("s");
            fuseResponse.Append<int>(4415);

            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("matic_walkway_green");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("s");
            fuseResponse.Append<int>(4415);

            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("matic_tree_green");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("s");
            fuseResponse.Append<int>(4415);
            fuseResponse.Send(Session);
        }

        public static void GetShopData(Message Msg, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendShopData);
            fuseResponse.Append<bool>(false);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(10000);
            fuseResponse.Append<int>(48);
            fuseResponse.Append<int>(7);
            fuseResponse.Send(Session);
        }

        public static void GetGiftWrapping(Message Msg, Session Session)
        {
            ServerMessage Packet = new ServerMessage(46);
            Packet.Append<bool>(true);
            Packet.Append<int>(1);
            Packet.Append<int>(10);
            Packet.Append<int>(3144);
            Packet.Append<int>(3145);
            Packet.Append<int>(3146);
            Packet.Append<int>(3147);
            Packet.Append<int>(3148);
            Packet.Append<int>(3149);
            Packet.Append<int>(3150);
            Packet.Append<int>(3151);
            Packet.Append<int>(3152);
            Packet.Append<int>(3153);
            Packet.Append<int>(7);
            Packet.Append<int>(0);
            Packet.Append<int>(1);
            Packet.Append<int>(2);
            Packet.Append<int>(3);
            Packet.Append<int>(4);
            Packet.Append<int>(5);
            Packet.Append<int>(6);
            Packet.Append<int>(11);
            Packet.Append<int>(0);
            Packet.Append<int>(1);
            Packet.Append<int>(2);
            Packet.Append<int>(3);
            Packet.Append<int>(4);
            Packet.Append<int>(5);
            Packet.Append<int>(6);
            Packet.Append<int>(7);
            Packet.Append<int>(8);
            Packet.Append<int>(9);
            Packet.Append<int>(10);
            Packet.Append<int>(7);
            Packet.Append<int>(187);
            Packet.Append<int>(188);
            Packet.Append<int>(189);
            Packet.Append<int>(190);
            Packet.Append<int>(191);
            Packet.Append<int>(192);
            Packet.Append<int>(193);
            Packet.Send(Session);
        }

        public static void GetBundleDiscount(Message Msg, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendBundleDiscount);
            fuseResponse.Append<int>(100);
            fuseResponse.Append<int>(6);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(40);
            fuseResponse.Append<int>(99);
            fuseResponse.Send(Session);
        }

        public static void GetShopPage(Message Msg, Session Session)
        {
            int ID = Msg.NextInt32();

            fuseResponse.New(Opcodes.OpcodesOut.SendShopPage);
            fuseResponse.Append<int>(ID);
            fuseResponse.Append<string>("frontpage3");
            fuseResponse.Append<int>(3);
            fuseResponse.Append<string>("catalog_frontpage_headline_shop_GENERAL");
            fuseResponse.Append<string>("Bundles_ts");
            fuseResponse.Append<string>("frontpage_sms4");
            fuseResponse.Append<int>(11);
            fuseResponse.Append<string>("");
            fuseResponse.Append<string>("");
            fuseResponse.Append<string>("Ã?stÃ¼ne hopla");
            fuseResponse.Append<string>("NasÄ±l Habbo Kredisi alÄ±nÄ±r");
            fuseResponse.Append<string>("KontÃ¶rlÃ¼ Kartlar, Ev Telefonu, Kredi KartÄ±, Cep Telefonu, teklifleri tamamlama ve daha birÃ§ok yÃ¶ntemle Habbo Kredisi alabilirsin!");
            fuseResponse.Append<string>("Burada bir fiÅ? kodunu bozdur:");
            fuseResponse.Append<string>("");
            fuseResponse.Append<string>("");
            fuseResponse.Append<string>("");
            fuseResponse.Append<string>("-TÃ¼m seÃ§enekleri ister misin? Buraya tÄ±kla!");
            fuseResponse.Append<string>("kredi");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(-1);
            fuseResponse.Append<bool>(false);
            fuseResponse.Send(Session);
        }

        public static void GetVIPBuyDialog(Message Message, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendVIPBuyDialog);
            fuseResponse.Append<int>(2);

            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("HABBO_CLUB_VIP_1_MONTH");
            fuseResponse.Append<int>(25);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(31);
            fuseResponse.Append<int>(31);
            fuseResponse.Append<int>(2012);
            fuseResponse.Append<int>(11);
            fuseResponse.Append<int>(7);

            fuseResponse.Append<int>(2);
            fuseResponse.Append<string>("HABBO_CLUB_VIP_3_MONTHS");
            fuseResponse.Append<int>(25);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(31);
            fuseResponse.Append<int>(31);
            fuseResponse.Append<int>(2012);
            fuseResponse.Append<int>(11);
            fuseResponse.Append<int>(7);

            fuseResponse.Append<int>(Message.NextInt32());
            fuseResponse.Send(Session);
        }
    }
}
