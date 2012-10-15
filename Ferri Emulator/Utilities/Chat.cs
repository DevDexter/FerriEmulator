using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Utilities
{
    public class Chat
    {
        public static int GetChatEmoticon(string input)
        {
            string[] Happy = new string[] { ":)", ";)", ":D", ":d", ";D", ";d", "^^", ":-)", ";-)" };
            string[] Sad = new string[] { ":(", ";(", ":-(", ";-(", ":'(", ";'(" };
            string[] Angry = new string[] { ":@", ">:(" };
            string[] Surprised = new string[] { ":o", ":O", ";o", ";O", ":|" };

            if (Happy.Any(input.Contains))
            {
                return 1;
            }

            if (Angry.Any(input.Contains))
            {
                return 2;
            }

            if (Surprised.Any(input.Contains))
            {
                return 3;
            }

            if (Sad.Any(input.Contains))
            {
                return 4;
            }

            return 0;
        }
    }
}
