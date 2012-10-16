using System;

namespace Ferri_Emulator
{
    internal sealed class SendDataToken
    {
        private byte[] _dataToSend;
        private int _sendBytesRemainingCount;

        public int SendBytesRemainingCount
        {
            get { return _sendBytesRemainingCount; }
            set { _sendBytesRemainingCount = value; }
        }

        public int BytesSentAlreadyCount { get; set; }

        public byte[] DataToSend
        {
            get { return _dataToSend; }
            set { _dataToSend = value; }
        }

        public void Reset()
        {
            _sendBytesRemainingCount = 0;
            BytesSentAlreadyCount = 0;
            Array.Clear(DataToSend, 0, DataToSend.Length);
            _dataToSend = null;
        }
    }
}