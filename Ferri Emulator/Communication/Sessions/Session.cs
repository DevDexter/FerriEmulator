using System;
using System.Net.Sockets;
using System.Text;
using Ferri_Encryption;
using Ferri_Emulator.Database.Mappings;
using Ferri_Emulator.Messages;

namespace Ferri_Emulator.Communication
{
    public sealed class Session
    {
        public HabboCrypto Crypto;
        private bool _disconnectedCalled;
        public string MachineID = "";
        public int X;
        public users User;

        public rooms Room;

        public string ReleaseBuild;

        public int Y;
        public System.Threading.Thread MoveThread;


        /// <summary>
        /// Initializes a new instance of the Session class.
        /// </summary>
        /// <param name="manager">The manager which created this session.</param>
        public Session(int id, ServerSocket manager)
        {
            Id = id;
            Manager = manager;
        }

        /// <summary>
        /// Unique ID which indentifies this Session. (It may only be used for debugging purposes)
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The manager that was used to create this session.
        /// </summary>
        public ServerSocket Manager { get; private set; }

        /// <summary>
        /// The Socket used for the backend of the communication.
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// Gets the IP Address of this connection session.
        /// </summary>
        public string IpAddress
        {
            get { return Socket.RemoteEndPoint.ToString().Split(':')[0]; }
        }

        /// <summary>
        /// This method is called when data incoming has been received.
        /// </summary>
        /// <param name="bytes">The array of data.</param>
        public void OnReceiveData(byte[] bytes)
        {
            try
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
                    var message = new Message(bytes);

                    Engine.Packethandler.Handle(message, this);

                    bytes = message.BytesRemain;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Forces this player to be disconnected
        /// </summary>
        public void Disconnect()
        {
            if (!_disconnectedCalled)
            {
                _disconnectedCalled = true;
                Socket.Disconnect(true); // This needs improving (DisconnectAsync)
            }
        }

        public void SendPacket(ServerMessage packet)
        {
            SendData(packet.ToByteArray());
        }

        public void SendData(byte[] data)
        {
            Manager.SendData(Socket, data);
        }

        public void SendData(string Data)
        {
            SendData(Encoding.UTF8.GetBytes(Data));
        }

        public void SendAlert(string message)
        {
            var Response = new ServerMessage(Opcodes.OpcodesOut.SendIlluminaAlert);
            Response.Append<string>(message);
            SendPacket(Response);
        }
    }

    public class Data
    {
        protected static ServerMessage fuseResponse = new ServerMessage();
    }
}
