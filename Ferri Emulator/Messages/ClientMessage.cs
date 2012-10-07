using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Utilities;

namespace Ferri_Emulator.Messages
{
    public class ByteUtil
    {
        #region Variables
        protected List<byte> Bytes;
        private int Pointer;
        #endregion

        #region Getters
        public int Length
        {
            get
            {
                return Bytes.Count;
            }
        }

        public int BytesRemain
        {
            get
            {
                return Length - Pointer;
            }
        }
        #endregion

        #region Constructors
        public ByteUtil(byte[] _Bytes)
        {
            this.Bytes = _Bytes.ToList();
            this.Pointer = 0;
        }

        public ByteUtil()
        {
            this.Bytes = new List<byte>();
            this.Pointer = 4;
        }
        #endregion

        #region Readers
        public byte[] ReadBytes(int _Length, bool _Reverse = false)
        {
            if (_Length > BytesRemain)
            {
                _Length = BytesRemain;
            }

            List<byte> Result = new List<byte>(_Length);

            for (int i = 0; i < _Length; i++)
            {
                Result.Add(Bytes[Pointer++]);
            }

            if (_Reverse)
            {
                Result.Reverse();
            }

            return Result.ToArray();
        }

        public byte[] ReadBytes(int _StartOff, int _Length, bool _Reverse = false)
        {
            if (_Length > BytesRemain)
            {
                _Length = BytesRemain;
            }

            List<byte> Result = new List<byte>(_Length);

            for (int i = 0, j = _StartOff; i < _Length; i++, j++)
            {
                Result.Add(Bytes[j]);
            }

            if (_Reverse)
            {
                Result.Reverse();
            }

            return Result.ToArray();
        }
        #endregion

        #region Setters
        public void WriteBytes(byte[] _Bytes, bool _Reverse = false)
        {
            List<byte> TBytes = _Bytes.ToList();

            if (_Reverse)
            {
                TBytes.Reverse();
            }

            foreach (byte Byte in TBytes)
            {
                Bytes.Add(Byte);
            }
        }
        #endregion
    }

    public class Message : ByteUtil
    {
        #region Variables

        private int _pPacketLength;
        public new byte[] BytesRemain { get; private set; }

        public short HeaderId { get; private set; }

        public int PacketLength
        {
            get
            {
                if (IsServerMessage)
                {
                    return base.Bytes.Count;
                }

                return _pPacketLength;
            }
            set { _pPacketLength = value; }
        }

        public bool IsServerMessage { get; private set; }

        #endregion

        #region Getters

        public byte[] GetBytes
        {
            get
            {
                if (IsServerMessage)
                {
                    return ParsedServerBytes();
                }

                return base.Bytes.ToArray();
            }
        }

        #endregion

        #region Constructors

        public Message(byte[] bytes)
            : base(bytes)
        {
            BytesRemain = null;
            IsServerMessage = false;
            PacketLength = NextInt32();
            HeaderId = NextInt16();

            FixLength();
        }

        public Message(short headerid)
        {
            BytesRemain = null;
            IsServerMessage = true;
            HeaderId = headerid;

            WriteInt16((short)headerid);
        }

        #endregion

        #region Readers

        public Int16 NextInt16()
        {
            return BitConverter.ToInt16(base.ReadBytes(2, true), 0);
        }

        public Int32 NextInt32()
        {
            return BitConverter.ToInt32(base.ReadBytes(4, true), 0);
        }

        public string NextString()
        {
            byte[] Bytes = base.ReadBytes(NextInt16());

            return Encoding.Default.GetString(Bytes);
        }

        public bool NextBool()
        {
            return BitConverter.ToBoolean(base.ReadBytes(1), 0);
        }

        public T Read<T>()
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)NextInt32();
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)NextString();
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)NextBool();
            }

            if (typeof(T) == typeof(short))
            {
                return (T)(object)NextInt16();
            }

            if (typeof(T) == typeof(uint))
            {
                return (T)(object)(uint)NextInt32();
            }

            return default(T);
        }

        #endregion

        #region Setters

        public void WriteInt16(Int16 data)
        {
            base.WriteBytes(BitConverter.GetBytes(data), true);
        }

        public void WriteInt32(Int32 data)
        {
            base.WriteBytes(BitConverter.GetBytes(data), true);
        }

        public void WriteString(string data)
        {
            WriteInt16((short)data.Length);

            base.WriteBytes(Encoding.Default.GetBytes(data));
        }

        public void WriteBool(bool data)
        {
            base.WriteBytes(BitConverter.GetBytes(data));
        }

        #endregion

        #region Parser

        private byte[] ParsedServerBytes()
        {
            byte[] Length = BitConverter.GetBytes(PacketLength);
            var Result = new List<byte>(base.Bytes);

            foreach (byte Byte in Length)
            {
                Result.Insert(0, Byte);
            }

            return Result.ToArray();
        }

        private void FixLength()
        {
            try
            {
                int SourceIndex = PacketLength + 4;
                int Length = base.Length - SourceIndex;

                if (SourceIndex == base.Length && Length == 0)
                {
                    return;
                }

                BytesRemain = new byte[Length];
                Array.Copy(GetBytes, SourceIndex, BytesRemain, 0, Length);

                var Result = new byte[SourceIndex];
                Array.Copy(GetBytes, Result, SourceIndex);

                base.Bytes = Result.ToList();
            }
            catch
            {

            }
        }

        #endregion
    }
}
