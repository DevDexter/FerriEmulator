using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ferri_Encryption;
using Ferri.Kernel.Network;
using Ferri_Emulator.SS;
using Ferri_Emulator.Habbo_Hotel.Users;

namespace Ferri_Emulator.Messages.Requests
{
    public class Handshake : Data
    {
        public static void ReadRelease(Message Msg, Session Session)
        {
        }

        public static void InitCrypto(Message Msg, Session Session)
        {
            String Token = new BigInteger(DiffieHellman.GenerateRandomHexString(15), 16).ToString();

            Engine.BannerTokenValues.Add(Token, Session.Crypto.GetPrime.ToString() + ":" + Session.Crypto.GetGenerator.ToString());

            fuseResponse.New(Opcodes.OpcodesOut.SendToken);
            fuseResponse.Append<string>(Token);
            fuseResponse.Append<bool>(false);
            Session.WriteComposer(fuseResponse);
        }

        public static void InitRC4(Message Msg, Session Session)
        {
            string Key = Msg.Read<string>();

            if (!Session.Crypto.InitializeRC4(Key))
                return;

            fuseResponse.New(Opcodes.OpcodesOut.SendPublicKey);
            fuseResponse.Append<string>(Session.Crypto.GetPublicKey.ToString());
            Session.WriteComposer(fuseResponse);
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

                Others.GetCitizenship(Msg, Session);

                fuseResponse.New(Opcodes.OpcodesOut.SendMinimailCount);
                fuseResponse.Append<int>(1);
                fuseResponse.Send(Session);

                fuseResponse.New(Opcodes.OpcodesOut.SendHomeRoom);
                fuseResponse.Append<int>(100);
                fuseResponse.Append<int>(0);
                fuseResponse.Send(Session);

                Session.SendBroadcast("Welcome to Ferri, " + Session.User.Username);

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
            }
            catch
            {
            }
        }
    }
}
