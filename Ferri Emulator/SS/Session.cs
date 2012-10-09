using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Ferri_Encryption;
using Ferri_Emulator.Messages;
using Ferri_Emulator;

namespace Ferri.Kernel.Network
{
    public class Session
    {
        public Socket Socket
        {
            get;
            set;
        }

        public SocketAsyncEventArgs ReceiveEventArgs
        {
            get;
            set;
        }

        public int SendBytesRemainingCount
        {
            get;
            set;
        }

        public int BytesSentAlreadyCount
        {
            get;
            set;
        }

        public byte[] DataToSend
        {
            get;
            set;
        }

        public void OnConnectionClose()
        {

        }

        public HabboCrypto Crypto = new HabboCrypto(Engine.N, Engine.E, Engine.D);
        public Ferri_Emulator.Database.Mappings.users User;
        public Ferri_Emulator.Database.Mappings.rooms Room;

        public void WriteComposer(ServerMessage Msg)
        {
            Socket.Send(Msg.ToByteArray());
        }

        public ServerMessage Response
        {
            get
            {
                return new ServerMessage();
            }
        }

        public void SendBroadcast(string Message)
        {
            ServerMessage Response = new ServerMessage(Opcodes.OpcodesOut.SendIlluminaAlert);
            Response.Append<string>(Message);
            WriteComposer(Response);
        }

        public void SendMOTD(string[] Messages)
        {
            ServerMessage Response = new ServerMessage(Opcodes.OpcodesOut.SendMessageOfTheDay);
            Response.Append<int>(Messages.Length);
            foreach(string Message in Messages)
                Response.Append<string>(Message);
            WriteComposer(Response);
        }

        public void SendMOTD(string Message)
        {
            SendMOTD(new string[] { Message });
        }

        public void SendMOTD(List<string> Messages)
        {
            SendMOTD(Messages.ToArray());
        }

        internal void ReceiveData(byte[] bytes)
        {
            if (Crypto != null)
            {
                if (Crypto.Initialized)
                {
                    bytes = Crypto.Parse(bytes);
                }
            }

            while (bytes != null)
            {
                Message msg = new Message(bytes);

                Engine.Packethandler.Handle(msg, this);

                bytes = msg.BytesRemain;
            }
        }
    }
}

namespace Ferri_Emulator.SS
{
    public class Data
    {
        protected static ServerMessage fuseResponse = new ServerMessage();
    }
}
