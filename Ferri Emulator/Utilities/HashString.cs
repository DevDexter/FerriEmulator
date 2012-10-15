using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Utilities
{
    public class HashString
    {
        public string s;

        public static implicit operator string(HashString hash)
        {
            string ToHash = hash.s;
            string Output = "";

            for (int i = 0; i < ToHash.ToCharArray().Length; i++)
            {
                if (i < (ToHash.Length / 1.50))
                {
                    Output += (i / ToHash.Length + 1.50);
                    Output += ToHash[i];
                }
                else
                {
                    Output += (i + ToHash.Length - ToHash.Max());
                    Output += ToHash[i];
                    Output += ((i ^ ToHash.Length * new Random().Next()) + Convert.ToInt32(new Random().NextDouble())); 
                }
            }

            return Output.Replace("-", "").Replace(",", "");
        }

        public static implicit operator HashString(string String)
        {
            return new HashString()
            {
                s = String
            };
        }
    }
}
