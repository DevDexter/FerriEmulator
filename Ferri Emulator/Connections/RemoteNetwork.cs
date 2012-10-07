using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Ferri_Emulator.Connections
{
    public class RemoteNetwork : Socket
    {
        public RemoteNetwork(int Port) : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            base.Bind(new IPEndPoint(IPAddress.Any, Port));
            base.Listen(15);
            base.BeginAccept(Accept, null);

            Engine.Logging.WriteTagLine("Remote", "Bound RemoteNetwork to {0}", base.LocalEndPoint);
        }

        public void Accept(IAsyncResult res)
        {
            Socket AcceptSock = base.EndAccept(res);
            RemoteHandler Handler = new RemoteHandler(AcceptSock);

            base.BeginAccept(Accept, null);
        }
    }
}
