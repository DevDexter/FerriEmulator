using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Ferri_Emulator.Communication
{
    internal sealed class SocketAsyncEventArgsPool
    {
        private readonly ConcurrentStack<SocketAsyncEventArgs> pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            pool = new ConcurrentStack<SocketAsyncEventArgs>();
        }

        public bool TryPop(out SocketAsyncEventArgs args)
        {
            return pool.TryPop(out args);
        }

        public void Push(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
            }

            pool.Push(args);
        }

        public void Dispose()
        {
            SocketAsyncEventArgs eventArgs;

            while (pool.Count > 0)
            {
                if (pool.TryPop(out eventArgs))
                {
                    eventArgs.Dispose();
                }
            }
        }
    }
}