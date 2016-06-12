using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTK.Utilities
{
    public partial class ByteStream
    {
        byte[] stream;
        int index;

        private bool _isWriting;

        public int Count
        {
            get
            {
                if (stream != null)
                    return stream.Length;
                else
                    return byteList.Count;
            }
        }

        public bool IsWriting
        {
            get
            {
                return _isWriting;
            }

            set
            {
                _isWriting = value;
            }
        }

        public bool IsReading
        {
            get
            {
                return !_isWriting;
            }
            set
            {
                _isWriting = !value;
            }
        }

        private List<byte> byteList;

        public static implicit operator byte [](ByteStream x)
        {
            if (x.IsWriting)
                return x.byteList.ToArray();
            else
                return x.stream;
        }

        public ByteStream(byte [] data)
        {
            stream = data;
            byteList = new List<byte>();
            index = 0;
        }

        public ByteStream()
        {
            byteList = new List<byte>();
            index = 0;
        }

        public void WriteToStream(byte[] data)
        {
            byteList.AddRange(data);
            
        }

        public byte [] RemainingData
        {
            get
            {
                int startPoint = index;
                int endPoint = (stream.Length - 1);

                byte[] remaining = new byte[endPoint - startPoint + 1];
                Array.Copy(stream, startPoint, remaining, 0, endPoint - startPoint + 1);

                return remaining;
            }
        }
    }
}