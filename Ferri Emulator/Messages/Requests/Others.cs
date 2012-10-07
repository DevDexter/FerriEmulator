using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri.Kernel.Network;
using Ferri_Emulator.SS;

namespace Ferri_Emulator.Messages.Requests
{
    public class Others : Data
    {
        public static void UNK1(Message Message, Session Session)
        {
            bool b = Message.NextBool();
            int i = Message.NextInt32();

            fuseResponse.New(3306);
            fuseResponse.Append<bool>(b);
            fuseResponse.Append<int>(i);
            //fuseResponse.Send(Session);

            fuseResponse.New(3713);
            fuseResponse.Append<string>("30-09-2012 16:17:34");
            fuseResponse.Append<int>(78151);
            //fuseResponse.Send(Session);
        }

        public static void GetHotelview(Message Message, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendHotelView);
            fuseResponse.Append<string>("2012-10-03 10:00,vipParties1;2012-10-08 10:00,vipParties2;2012-10-12 10:00,vipParties3;2012-10-14 10:00,");
            fuseResponse.Append<string>("vipParties1");
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendHotelView);
            fuseResponse.Append<string>("2012-09-28 15:00,jetsetquest1;2012-10-03 10:00,;2012-10-03 18:00,jetsetquest2;2012-10-08 14:00,jetsetquest3;2012-10-11 10:00,");
            fuseResponse.Append<string>("jetsetquest2");
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendHotelView);
            fuseResponse.Append<string>("2012-09-27 12:00,jetset0;2012-09-28 18:00,diamondpromo;2012-10-02 10:00,jetsetaccess;2012-10-12 10:00,");
            fuseResponse.Append<string>("jetsetaccess");
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendHotelView);
            fuseResponse.Append<string>("2012-09-28 18:00,jetsetsubmit1;2012-10-01 13:00,jetsetvote1;2012-10-03 10:00,;2012-10-03 18:00,jetsetsubmit2;2012-10-05 10:00,jetsetvote2;2012-10-08 10:00,;2012-10-08 14:00,jetsetsubmit3;2012-10-10 10:00,jetsetvote3;2012-10-12 10:00,");
            fuseResponse.Append<string>("jetsetvote2");
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendHotelView);
            fuseResponse.Append<string>("2012-09-28 15:00,jetsetvip;2012-10-12 10:00");
            fuseResponse.Append<string>("jetsetvip");
            fuseResponse.Send(Session);
            
            fuseResponse.New(Opcodes.OpcodesOut.SendWelcomeContainer);
            fuseResponse.Append<int>(0);
            fuseResponse.Send(Session);
        }

        public static void GetCampaignWinners(Message Message, Session Session)
        {
            string Cmpgn = Message.NextString();

            fuseResponse.New(Opcodes.OpcodesOut.SendCampaignWinners);
            fuseResponse.Append<string>(Cmpgn);
            fuseResponse.Append<int>(1);

            fuseResponse.Append<int>(Session.User.ID);
            fuseResponse.Append<string>(Session.User.Username);
            fuseResponse.Append<string>(Session.User.Figure);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1337);
            fuseResponse.Send(Session);
        }
    }
}
