using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Communication;

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

        public static void GetCitizenship(Message Message, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendCitizenship);
            fuseResponse.Append<string>("citizenship");
            fuseResponse.Append<int>(5);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(125);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("ACH_SafetyQuizGraduate1");
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("A1 KUMIANKKA");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(4);
            fuseResponse.Append<int>(6);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("ACH_AvatarLooks1");
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(18);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("ACH_RespectGiven1");
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(19);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("ACH_AllTimeHotelPresence1");
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(30);
            fuseResponse.Append<int>(30);
            fuseResponse.Append<int>(8);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("ACH_RoomEntry1");
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(5);
            fuseResponse.Append<int>(5);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("A1 KUMIANKKA");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(4);
            fuseResponse.Append<int>(145);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("ACH_GuideAdvertisementReader1");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(11);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("ACH_RegistrationDuration1");
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(19);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<string>("ACH_AllTimeHotelPresence2");
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(60);
            fuseResponse.Append<int>(60);
            fuseResponse.Append<int>(8);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<string>("ACH_RoomEntry2");
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(20);
            fuseResponse.Append<int>(20);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("A1 KUMIANKKA");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(4);
            fuseResponse.Append<int>(11);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<string>("ACH_RegistrationDuration2");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<int>(94);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("ACH_HabboWayGraduate1");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<int>(19);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<string>("ACH_AllTimeHotelPresence3");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(120);
            fuseResponse.Append<int>(142);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("ACH_FriendListSize1");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("TRADE");
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("A1 KUMIANKKA");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(4);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(1);
            fuseResponse.Append<string>("CITIZEN");
            fuseResponse.Append<int>(2);
            fuseResponse.Append<string>("A1 KUMIANKKA");
            fuseResponse.Append<int>(0);
            fuseResponse.Append<string>("HABBO_CLUB_CITIZENSHIP_VIP_REWARD");
            fuseResponse.Append<int>(7);
            fuseResponse.Send(Session);
        }
    }
}
