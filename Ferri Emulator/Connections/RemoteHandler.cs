using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Connections
{
    class RemoteHandler
    {
        private System.Net.Sockets.Socket AcceptSock;
        private byte[] buffer = new byte[1024];

        public RemoteHandler(System.Net.Sockets.Socket AcceptSock)
        {
            // TODO: Complete member initialization
            this.AcceptSock = AcceptSock;
            this.AcceptSock.BeginReceive(buffer, 0, buffer.Length, 0, Receive, null);
        }

        public void Receive(IAsyncResult res)
        {
            int Bits = this.AcceptSock.EndReceive(res);
            string Data = Encoding.Default.GetString(buffer, 0, Bits);
            string[] Args = Data.Split(Convert.ToChar(1));
            string Header = Args[0];
            string Extras = Args[1];

            switch (Header)
            {
                case "0":
                    {
                        string Token = Extras;
                        string Tosend = "";

                        Engine.BannerTokenValues.TryGetValue(Token, out Tosend);

                        this.AcceptSock.Send(Encoding.Default.GetBytes(Tosend));
                        break;
                    }
            }
        }
    }
}
