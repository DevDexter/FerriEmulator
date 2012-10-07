using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Database.Mappings;
using NHibernate.Criterion;
using System.Data;

namespace Ferri_Emulator.Habbo_Hotel.Users
{
    public class FluentUsers
    {
        public static users AuthenticateUser(string SSO)
        {
            try
            {
                DataRow Row = Engine.dbManager.ReadRow("SELECT * FROM members WHERE ssoticket = '" + SSO + "'");

                var User = new users()
                {
                    ID = (int)Row["id"],
                    Username = Row["username"].ToString(),
                    Coins = (int)Row["coins"],
                    Email = Row["email"].ToString(),
                    Figure = Row["figure"].ToString(),
                    Gender = Row["gender"].ToString(),
                    Online = Row["online"].ToString(),
                    Respect = (int)Row["respect"],
                    SsoTicket = SSO,
                    Tags = Row["tags"].ToString(),
                    Pixels = (int)Row["pixels"],
                    OtherCurrency = (int)Row["othercurrency"]
                };

                return User;
            }
            catch
            {
                return null;
            }
        }
    }
}
