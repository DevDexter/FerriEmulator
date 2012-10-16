using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Ferri_Emulator.Communication
{
    public sealed class ServerSocket
    {
        /// <summary>
        /// The buffer manager for allocation a buffer block to a SocketAsyncEventArgs.
        /// </summary>
        private readonly BufferManager _bufferManager;

        /// <summary>
        /// The semaphore used for controlling the max socket accept operations at a time.
        /// </summary>
        private readonly SemaphoreSlim _maxAcceptOpsEnforcer;

        /// <summary>
        /// The semaphore used for controlling the max connections to the server.
        /// </summary>
        private readonly SemaphoreSlim _maxConnectionsEnforcer;

        /// <summary>
        /// The semaphore used for controlling the max SocketAsyncEventArgs to be used for send operations at a time.
        /// </summary>
        private readonly SemaphoreSlim _maxSaeaSendEnforcer;

        /// <summary>
        /// The pool of re-usable SocketAsyncEventArgs for accept operations.
        /// </summary>
        private readonly SocketAsyncEventArgsPool _poolOfAcceptEventArgs;

        /// <summary>
        /// The pool of re-usable SocketAsyncEventArgs for receiving data.
        /// </summary>
        private readonly SocketAsyncEventArgsPool _poolOfRecEventArgs;

        /// <summary>
        /// The pool of re-usable SocketAsyncEventArgs for sending data.
        /// </summary>
        private readonly SocketAsyncEventArgsPool _poolOfSendEventArgs;

        /// <summary>
        /// The settings to use with this ServerSocket.
        /// </summary>
        private readonly ServerSocketSettings _settings;

        /// <summary>
        /// The socket used for listening for incoming connections.
        /// </summary>
        private Socket _listenSocket;

        /// <summary>
        /// Initializes a new instance of the ServerSocket.
        /// </summary>
        /// <param name="settings">The settings to use with this ServerSocket.</param>
        public ServerSocket(ServerSocketSettings settings)
        {
            _settings = settings;

            _bufferManager =
                new BufferManager(
                    (_settings.BufferSize*_settings.NumOfSaeaForRec) + (_settings.BufferSize*_settings.NumOfSaeaForSend),
                    _settings.BufferSize);
            _poolOfAcceptEventArgs = new SocketAsyncEventArgsPool(_settings.MaxSimultaneousAcceptOps);
            _poolOfRecEventArgs = new SocketAsyncEventArgsPool(_settings.NumOfSaeaForRec);
            _poolOfSendEventArgs = new SocketAsyncEventArgsPool(_settings.NumOfSaeaForSend);

            _maxConnectionsEnforcer = new SemaphoreSlim(_settings.MaxConnections, _settings.MaxConnections);
            _maxSaeaSendEnforcer = new SemaphoreSlim(_settings.NumOfSaeaForSend, _settings.NumOfSaeaForSend);
            _maxAcceptOpsEnforcer = new SemaphoreSlim(_settings.MaxSimultaneousAcceptOps,
                                                     _settings.MaxSimultaneousAcceptOps);
        }

        public void Init()
        {
            _bufferManager.InitBuffer();

            for (int i = 0; i < _settings.MaxSimultaneousAcceptOps; i++)
            {
                var acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed +=
                    AcceptEventArgCompleted;

                _poolOfAcceptEventArgs.Push(acceptEventArg);
            }

            // receive objects
            for (int i = 0; i < _settings.NumOfSaeaForRec; i++)
            {
                var eventArgObjectForPool = new SocketAsyncEventArgs();
                _bufferManager.SetBuffer(eventArgObjectForPool);

                eventArgObjectForPool.Completed +=
                    IoReceiveCompleted;
                eventArgObjectForPool.UserToken = new Session(i, this);
                _poolOfRecEventArgs.Push(eventArgObjectForPool);
            }

            // send objects
            for (int i = 0; i < _settings.NumOfSaeaForSend; i++)
            {
                var eventArgObjectForPool = new SocketAsyncEventArgs();
                _bufferManager.SetBuffer(eventArgObjectForPool);

                eventArgObjectForPool.Completed +=
                    IoSendCompleted;
                eventArgObjectForPool.UserToken = new SendDataToken();
                _poolOfSendEventArgs.Push(eventArgObjectForPool);
            }
        }

        public void StartListen()
        {
            _listenSocket = new Socket(_settings.Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(_settings.Endpoint);
            _listenSocket.Listen(_settings.Backlog);

            Engine.Logging.WriteTagLine("NETWORK", string.Format("Game Socket started on: {0}:{1}", _settings.Endpoint.Address,
                                                        _settings.Endpoint.Port));

            StartAccept();
        }

        private void StartAccept()
        {
            SocketAsyncEventArgs acceptEventArgs;

            _maxAcceptOpsEnforcer.Wait();

            if (_poolOfAcceptEventArgs.TryPop(out acceptEventArgs))
            {
                _maxConnectionsEnforcer.Wait();
                bool willRaiseEvent = _listenSocket.AcceptAsync(acceptEventArgs);

                if (!willRaiseEvent)
                {
                    ProcessAccept(acceptEventArgs);
                }
            }
        }

        private void AcceptEventArgCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            StartAccept();

            if (acceptEventArgs.SocketError != SocketError.Success)
            {
                HandleBadAccept(acceptEventArgs);
                _maxAcceptOpsEnforcer.Release();
                return;
            }

            SocketAsyncEventArgs recEventArgs;

            if (_poolOfRecEventArgs.TryPop(out recEventArgs))
            {
                ((Session) recEventArgs.UserToken).Socket = acceptEventArgs.AcceptSocket;

                acceptEventArgs.AcceptSocket = null;
                _poolOfAcceptEventArgs.Push(acceptEventArgs);
                _maxAcceptOpsEnforcer.Release();

                Console.WriteLine("<Session " + ((Session) recEventArgs.UserToken).Id + "> is now in use.");

                StartReceive(recEventArgs);
            }
            else
            {
                HandleBadAccept(acceptEventArgs);
                Console.WriteLine("Cannot handle this session, there are no more receive objects available for us.");
            }
        }

        private void IoSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation != SocketAsyncOperation.Send)
            {
                throw new InvalidOperationException(
                    "Tried to pass a send operation but the operation expected was not a send.");
            }

            ProcessSend(e);
        }

        private void IoReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation != SocketAsyncOperation.Receive)
            {
                throw new InvalidOperationException(
                    "Tried to pass a receive operation but the operation expected was not a receive.");
            }

            ProcessReceive(e);
        }

        private void StartReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            var token = (Session) receiveEventArgs.UserToken;

            bool willRaiseEvent = token.Socket.ReceiveAsync(receiveEventArgs);

            if (!willRaiseEvent)
            {
                ProcessReceive(receiveEventArgs);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            var token = (Session) receiveEventArgs.UserToken;

            if (receiveEventArgs.BytesTransferred > 0 && receiveEventArgs.SocketError == SocketError.Success)
            {
                var dataReceived = new byte[receiveEventArgs.BytesTransferred];
                Buffer.BlockCopy(receiveEventArgs.Buffer, receiveEventArgs.Offset, dataReceived, 0,
                                 receiveEventArgs.BytesTransferred);

                if (dataReceived[0] == 60)
                {
                    token.SendData("<?xml version=\"1.0\"?> " +
                                   "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\"> " +
                                   "<cross-domain-policy> " +
                                   "<allow-access-from domain=\"*\" to-ports=\"*\" /> " +
                                   "</cross-domain-policy>\x0");
                }
                else
                {
                    token.OnReceiveData(dataReceived);
                }

                StartReceive(receiveEventArgs);
            }
            else
            {
                CloseClientSocket(receiveEventArgs);
                ReturnReceiveSaea(receiveEventArgs);
            }
        }

        public static string PacketFiltered(string data)
        {
            var e = data;

            foreach (char c in e)
            {
                if (c < 13)
                    e = e.Replace(c.ToString(CultureInfo.InvariantCulture), "[" + (int) c + "]");
                else
                    continue;
            }

            return e;
        }

        public void SendData(Socket socket, byte[] data)
        {
            _maxSaeaSendEnforcer.Wait();
            SocketAsyncEventArgs sendEventArgs;
            _poolOfSendEventArgs.TryPop(out sendEventArgs);

            var token = (SendDataToken) sendEventArgs.UserToken;
            token.DataToSend = data;
            token.SendBytesRemainingCount = data.Length;

            sendEventArgs.AcceptSocket = socket;
            StartSend(sendEventArgs);

            //Console.WriteLine(PacketFiltered(Encoding.Default.GetString(data)));
        }

        private void StartSend(SocketAsyncEventArgs sendEventArgs)
        {
            var token = (SendDataToken) sendEventArgs.UserToken;

            if (token.SendBytesRemainingCount <= _settings.BufferSize)
            {
                sendEventArgs.SetBuffer(sendEventArgs.Offset, token.SendBytesRemainingCount);
                Buffer.BlockCopy(token.DataToSend, token.BytesSentAlreadyCount, sendEventArgs.Buffer,
                                 sendEventArgs.Offset, token.SendBytesRemainingCount);
            }
            else
            {
                sendEventArgs.SetBuffer(sendEventArgs.Offset, _settings.BufferSize);
                Buffer.BlockCopy(token.DataToSend, token.BytesSentAlreadyCount, sendEventArgs.Buffer,
                                 sendEventArgs.Offset, _settings.BufferSize);
            }

            bool willRaiseEvent = sendEventArgs.AcceptSocket.SendAsync(sendEventArgs);

            if (!willRaiseEvent)
            {
                ProcessSend(sendEventArgs);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs sendEventArgs)
        {
            var token = (SendDataToken) sendEventArgs.UserToken;

            if (sendEventArgs.SocketError == SocketError.Success)
            {
                token.SendBytesRemainingCount = token.SendBytesRemainingCount - sendEventArgs.BytesTransferred;

                if (token.SendBytesRemainingCount == 0)
                {
                    token.Reset();
                    ReturnSendSaea(sendEventArgs);
                }
                else
                {
                    token.BytesSentAlreadyCount += sendEventArgs.BytesTransferred;
                    StartSend(sendEventArgs);
                }
            }
            else
            {
                token.Reset();
                CloseClientSocket(sendEventArgs);
                ReturnSendSaea(sendEventArgs);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs args)
        {
            var con = (Session) args.UserToken;

            try
            {
                con.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException)
            {
            }

            con.Socket.Close();

            Console.WriteLine("<Session " + con.Id + "> is no longer in use.");
        }

        private void ReturnReceiveSaea(SocketAsyncEventArgs args)
        {
            _poolOfRecEventArgs.Push(args);
            _maxConnectionsEnforcer.Release();
        }

        private void ReturnSendSaea(SocketAsyncEventArgs args)
        {
            _poolOfSendEventArgs.Push(args);
            _maxSaeaSendEnforcer.Release();
        }

        private void HandleBadAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            acceptEventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
            acceptEventArgs.AcceptSocket.Close();
            _poolOfAcceptEventArgs.Push(acceptEventArgs);
        }

        [Obsolete]
        public void Shutdown()
        {
            _listenSocket.Shutdown(SocketShutdown.Both);
            _listenSocket.Close();

            DisposeAllSaeaObjects();
        }

        private void DisposeAllSaeaObjects()
        {
            _poolOfAcceptEventArgs.Dispose();
            _poolOfSendEventArgs.Dispose();
            _poolOfRecEventArgs.Dispose();
        }
    }
}