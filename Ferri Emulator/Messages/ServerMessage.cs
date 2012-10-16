using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Communication;

namespace Ferri_Emulator.Messages
{
    public class ServerMessage
    {
        private List<byte> Message;

        public ServerMessage() { }

        public ServerMessage(short ID)
        {
            Message = new List<byte>();
            AppendInt16(ID);
        }

        public ServerMessage New(short ID)
        {
            Message = new List<byte>();
            AppendInt16(ID);
            return this;
        }

        private void AppendInt32(int Int32)
        {
            AddBytes(BitConverter.GetBytes(Int32), true);
        }

        private void AppendUInt32(uint UInt32)
        {
            AddBytes(BitConverter.GetBytes(UInt32), true);
        }

        private void AppendInt16(short Short)
        {
            AddBytes(BitConverter.GetBytes(Short), true);
        }

        private void AppendString(string String)
        {
            AppendInt16((short)String.Length);
            AddBytes(Encoding.Default.GetBytes(String), false);
        }

        private void AppendBool(bool Bool)
        {
            AddBytes(new byte[] { (Bool) ? (byte)1 : (byte)0 }, false);
        }

        private void AppendByte(byte b)
        {
            AddBytes(new byte[] { b }, false);
        }

        private void AddBytes(byte[] Bytes, bool IsInt)
        {
            if (IsInt)
            {
                for (int i = Bytes.Length - 1; i > -1; i--)
                {
                    this.Message.Add(Bytes[i]);
                }
            }
            else
            {
                this.Message.AddRange(Bytes);
            }
        }

        public void inte(int i)
        {
            Append<int>(i);
        }

        public ServerMessage Append<T>(object obj)
        {
            if (typeof(T) == typeof(int))
            {
                AppendInt32((int)obj);
                return this;
            }

            if (typeof(T) == typeof(string))
            {
                AppendString(obj.ToString());
                return this;
            }

            if (typeof(T) == typeof(bool))
            {
                AppendBool((bool)obj);
                return this;
            }

            if (typeof(T) == typeof(short))
            {
                AppendInt16((short)obj);
                return this;
            }

            if (typeof(T) == typeof(uint))
            {
                AppendUInt32((uint)obj);
                return this;
            }

            if (typeof(T) == typeof(string[]))
            {
                for (int i = 0; i < ((string[])obj).Length; i++)
                    AppendString(((string[])obj)[i]);
            }

            if (typeof(T) == typeof(byte))
            {
                AppendByte((byte)obj);
            }

            return this;
        }

        public byte[] ToByteArray()
        {
            List<byte> NewMsg = new List<byte>();

            NewMsg.AddRange(BitConverter.GetBytes(Message.Count));
            NewMsg.Reverse();
            NewMsg.AddRange(Message);

            return NewMsg.ToArray();
        }

        public void Send(Session Session)
        {
            Session.SendPacket(this);
        }

        public int GetBitLen()
        {
            return Message.Count;
        }
    }
}