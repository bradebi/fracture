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

        public void Deserialize<T>(out T value)
            where T : ISerializable, new ()
        {
            IsReading = true;

            int length = BitConverter.ToInt32(stream, index);
            index += sizeof(int);

            value = new T();

            if (length == 0)
            {
                value = default(T);
                return;
            }

            value.FromBytes((stream.Skip(index).Take(length)).ToArray());
            index += length;
        }

        public void Deserialize(out string value)
        {
            IsReading = true;
            
            int length = BitConverter.ToInt32(stream, index);
            index += sizeof(int);
            value = Encoding.ASCII.GetString(stream, index, length);
            index += length;
        }

        //Integer serialization
        public void Deserialize(out int value)
        {
            IsReading = true;

            value = BitConverter.ToInt32(stream, index);
            index += sizeof(int);
        }

        //Boolean serialization
        public void Deserialize(out bool value)
        {
            IsReading = true;

            value = Convert.ToBoolean(stream[index]);
            index += sizeof(bool);
        }

        //Short serialization
        public void Deserialize(out short value)
        {
            IsReading = true;

            value = BitConverter.ToInt16(stream, index);
            index += sizeof(short);
        }

        //Byte serialization
        public void Deserialize(out byte value)
        {
            IsReading = true;

            value = stream[index];
            index += sizeof(byte);
        }

        //Char serialization
        public void Deserialize(out char value)
        {
            IsReading = true;

            value = Convert.ToChar(stream[index]);
            index += sizeof(char);
        }

        //Float serialization
        public void Deserialize(out float value)
        {
            IsReading = true;

            value = BitConverter.ToSingle(stream, index);
            index += sizeof(float);
        }

        //Double serialization
        public void Deserialize(out double value)
        {
            IsReading = true;

            value = BitConverter.ToDouble(stream, index);
            index += sizeof(double);
        }

        //DateTime serialization
        public void Deserialize(out DateTime value)
        {
            IsReading = true;

            string temp;
            Deserialize(out temp);
            value = DateTime.Parse(temp);
        }

        // Long Serialization
        public void Deserialize(out long value)
        {
            IsReading = true;

            value = BitConverter.ToInt64(stream, index);
            index += sizeof(long);
        }

#if UNITY
        //Quaternion serialization
        public void Deserialize(out Quaternion value)
        {
            IsReading = true;

            value.w = BitConverter.ToSingle(stream, index);
            index += sizeof(float);
            value.x = BitConverter.ToSingle(stream, index);
            index += sizeof(float);
            value.y = BitConverter.ToSingle(stream, index);
            index += sizeof(float);
            value.z = BitConverter.ToSingle(stream, index);
            index += sizeof(float);
        }

        //Vector3 serialization
        public void Deserialize(out Vector3 value)
        {
            IsReading = true;

            value.x = BitConverter.ToSingle(stream, index);
            index += sizeof(float);
            value.y = BitConverter.ToSingle(stream, index);
            index += sizeof(float);
            value.z = BitConverter.ToSingle(stream, index);
            index += sizeof(float);
        }
#endif
    }
}
