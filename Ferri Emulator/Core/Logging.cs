using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Core
{
    public class Logging
    {
        public void SetTitle(string Title, params object[] Params)
        {
            Console.Title = string.Format(Title, Params);
        }

        public void WriteLine(string Text, params object[] Params)
        {
            Console.WriteLine(Text, Params);
        }

        public void WriteTagLine(string Tag, string Text, params object[] Params)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("[{0}] ", Tag);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(string.Format("-> {0}", Text), Params);
        }

        public void WriteErrorTagLine(string Tag, string Text, params object[] Params)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[{0}] ", Tag);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(string.Format("-> {0}", Text), Params);
        }
    }
}
