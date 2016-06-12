using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY
using UnityEngine;
#endif

namespace UTK.Utilities
{
    public partial class ByteStream
    {

        //Serialize any class that has inherited from Serializable
        public void Serialize(ISerializable value)
        {
            IsWriting = true;

            int length = 0;

            if(value == null)
            {
                byteList.AddRange(BitConverter.GetBytes(length));
                return;
            }
                
            //Convert the class to bytes
            byte [] data = value.ToBytes();

            //Add the length of the serializable type to the stream
            length = data.Length;
            byteList.AddRange(BitConverter.GetBytes(length));

            //Add the data to the byte list
            byteList.AddRange(data);

        }

        //String Serialization 
        public void Serialize(string value)
        {
            IsWriting = true;

            byte[] data = Encoding.ASCII.GetBytes(value);
            int length = data.Length;
            //Add length of the following string set so we know how to extract later
            byteList.AddRange(BitConverter.GetBytes(length));
            //Add the string to the byte list
            byteList.AddRange(data);
        }

        //Integer serialization
        public void Serialize(int value)
        {
            IsWriting = true;
            byteList.AddRange(BitConverter.GetBytes(value));
        }

        //Boolean serialization
        public void Serialize(bool value)
        {
            IsWriting = true;
            byteList.Add(Convert.ToByte(value));
        }

        //Short serialization
        public void Serialize(short value)
        {
            IsWriting = true;
            byteList.AddRange(BitConverter.GetBytes(value));
        }

        //Byte serialization
        public void Serialize(byte value)
        {
            IsWriting = true;
            byteList.Add(value);
        }

        //Char serialization
        public void Serialize(char value)
        {
            IsWriting = true;
            byteList.Add(Convert.ToByte(value));
        }

        //Float serialization
        public void Serialize(float value)
        {
            IsWriting = true;
            byteList.AddRange(BitConverter.GetBytes(value));
        }

        //Double serialization
        public void Serialize(double value)
        {
            IsWriting = true;
            byteList.AddRange(BitConverter.GetBytes(value));
        }

        //DateTime serialization
        public void Serialize(DateTime value)
        {
            IsWriting = true;
            Serialize(value.ToString());
        }

        //Double serialization
        public void Serialize(long value)
        {
            IsWriting = true;
            byteList.AddRange(BitConverter.GetBytes(value));
        }

#if UNITY
        //Quaternion serialization
        public void Serialize(Quaternion value)
        {
            IsWriting = true;
            byteList.AddRange(BitConverter.GetBytes(value.w));
            byteList.AddRange(BitConverter.GetBytes(value.x));
            byteList.AddRange(BitConverter.GetBytes(value.y));
            byteList.AddRange(BitConverter.GetBytes(value.z));
        }

        //Vector3 serialization
        public void Serialize(Vector3 value)
        {
            IsWriting = true;
            byteList.AddRange(BitConverter.GetBytes(value.x));
            byteList.AddRange(BitConverter.GetBytes(value.y));
            byteList.AddRange(BitConverter.GetBytes(value.z));
        }
#endif
    }
}
