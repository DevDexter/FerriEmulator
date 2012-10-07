using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.SS;
using Ferri.Kernel.Network;

namespace Ferri_Emulator.Messages.Requests
{
    public class Users : Data
    {
        public static void GetPurse(Message Message, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendCreditBalance);
            fuseResponse.Append<string>(string.Format("{0}.0", Session.User.Coins));
            fuseResponse.Send(Session);

            fuseResponse.New(Opcodes.OpcodesOut.SendActivityPoints);
            fuseResponse.Append<int>(2);
            fuseResponse.Append<int>(0);
            fuseResponse.Append<int>(Session.User.Pixels);
            fuseResponse.Append<int>(105);
            fuseResponse.Append<int>(Session.User.OtherCurrency);
            fuseResponse.Send(Session);
        }

        public static void GetUser(Message Msg, Session Session)
        {
            fuseResponse.New(Opcodes.OpcodesOut.SendUserInformation);
            fuseResponse.Append<int>(Session.User.ID);
            fuseResponse.Append<string>(Session.User.Username);
            fuseResponse.Append<string>(Session.User.Figure);
            fuseResponse.Append<string>(Session.User.Gender);
            fuseResponse.Append<string>("");
            fuseResponse.Append<string>(Session.User.Email.Split('@')[0]);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<int>(Session.User.Respect);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<int>(3);
            fuseResponse.Append<bool>(true);
            fuseResponse.Append<string>("30-09-2012 16:17:34");
            fuseResponse.Append<string>("");
            fuseResponse.Send(Session);
        }
    }
}
