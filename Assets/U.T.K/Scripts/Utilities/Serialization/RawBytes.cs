using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNIMPLEMENTED
    namespace Data
    {
        public class RawBytes : ISerializable
        {
            byte [] _data;
            public static implicit operator byte [](RawBytes x)
            {
                return x._data;
            }

            public RawBytes(){}

            public RawBytes(byte [] data)
            {
                _data = data;
            }

            public byte[] ToBytes()
            {
                return _data;
            }

            public void FromBytes(byte [] data)
            {

            }

        }
    }
#endif