using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ferri_Encryption;
using Ferri_Emulator.Habbo_Hotel.Users;
using Ferri_Emulator.Communication;

namespace Ferri_Emulator.Messages.Requests
{
    public class Handshake : Data
    {
        public static void ReadRelease(Message Msg, Session Session)
        {
        }

        public static void InitCrypto(Message Msg, Session Session)
        {
            Session.Crypto = new HabboCrypto(Engine.N, Engine.E, Engine.D);
            String Token = new BigInteger(DiffieHellman.GenerateRandomHexString(15), 16).ToString();

            Engine.BannerTokenValues.Add(Token, Session.Crypto.GetPrime.ToString() + ":" + Session.Crypto.GetGenerator.ToString());

            fuseResponse.New(Opcodes.OpcodesOut.SendToken);
            fuseResponse.Append<string>(Token);
            fuseResponse.Append<bool>(false);
            Session.SendPacket(fuseResponse);
        }

        public static void InitRC4(Message Msg, Session Session)
        {
            string Key = Msg.Read<string>();

            if (!Session.Crypto.InitializeRC4(Key))
                return;

            fuseResponse.New(Opcodes.OpcodesOut.SendPublicKey);
            fuseResponse.Append<string>(Session.Crypto.GetPublicKey.ToString());
            Session.SendPacket(fuseResponse);
        }

        public static void AuthenticateUser(Message Msg, Session Session)
        {
            string SSO = Msg.Read<string>();

            try
            {
                Session.User = FluentUsers.AuthenticateUser(SSO);

                fuseResponse.New(Opcodes.OpcodesOut.SendInterface);
                fuseResponse.Send(Session);

                fuseResponse.New(Opcodes.OpcodesOut.SendFriends);
                fuseResponse.Append<int>(300);
                fuseResponse.Append<int>(800);
                fuseResponse.Append<int>(1100);
                fuseResponse.Append<int>(1100);
                fuseResponse.Append<int>(0);
                fuseResponse.Append<int>(0);
                fuseResponse.Append<int>(100);
                fuseResponse.Append<int>(0);
                fuseResponse.Send(Session);

                fuseResponse.New(Opcodes.OpcodesOut.SendMinimailCount);
                fuseResponse.Append<int>(1);
                fuseResponse.Send(Session);

                fuseResponse.New(Opcodes.OpcodesOut.SendHomeRoom);
                fuseResponse.Append<int>(100);
                fuseResponse.Append<int>(0);
                fuseResponse.Send(Session);

                Session.SendAlert("Welcome to Ferri, " + Session.User.Username);

                fuseResponse.New(2367);
                fuseResponse.Append<int>(-1);
                fuseResponse.Append<int>(0);
                fuseResponse.Append<int>(0);
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<int>(0);
                fuseResponse.Send(Session);

                fuseResponse.New(108);
                fuseResponse.Append<int>(9); // count? wtf?
                fuseResponse.Append<string>("CITIZEN");
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<string>("");
                fuseResponse.Append<string>("VOTE_IN_COMPETITIONS");
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<string>("");
                fuseResponse.Append<string>("JUDGE_CHAT_REVIEWS");
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<string>("");
                fuseResponse.Append<string>("FULL_CHAT");
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<string>("");
                fuseResponse.Append<string>("CALL_ON_HELPERS");
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<string>("");
                fuseResponse.Append<string>("TRADE");
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<string>("");
                fuseResponse.Append<string>("USE_GUIDE_TOOL");
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<string>("");
                fuseResponse.Append<string>("SAFE_CHAT");
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<string>("");
                fuseResponse.Append<string>("SAFE_CHAT");
                fuseResponse.Append<bool>(true);
                fuseResponse.Append<string>("");
                fuseResponse.Send(Session);

                Session.User.Emblems = new Habbo_Hotel.Users.Badges.EmblemsManager();
                Session.User.Emblems.LoadBadges(Session.User.ID);
                Session.User.MessengerComponent = new Habbo_Hotel.Users.Messenger.MessengerComponent(Session.User.ID);

                fuseResponse.New(Opcodes.OpcodesOut.SendFuserights);
                fuseResponse.Append<int>(2);
                fuseResponse.Append<int>(7);
                fuseResponse.Send(Session);
            }
            catch
            {
            }
        }
    }
}
