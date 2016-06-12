using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY
using UnityEngine;
#endif
//! \todo: Add support for null objects :: Done?

namespace UTK.Utilities
{
    public partial class ByteStream
    {
        public void Serialize(List<int> valueList)
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Count;

            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                int value = valueList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<int> valueList)
        {

            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<int>();

            if (listLength == 0)
            {
                valueList = null;
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                int value = 0;
                Deserialize(out value);
                valueList.Add(value);
            }
            index += sizeof(int) * listLength;
        }

        public void Serialize(List<string> valueList)
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Count;

            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                string value = valueList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<string> valueList)
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<string>();

            if (listLength == 0)
            {
                valueList = null;
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                string value;
                Deserialize(out value);
                valueList.Add(value);
            }
        }

        public void Serialize(List<float> valueList)
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Count;

            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                float value = valueList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<float> valueList)
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<float>();

            if (listLength == 0)
            {
                valueList = null;
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                float value = 0;
                Deserialize(out value);
                valueList.Add(value);
            }
            index += sizeof(float) * listLength;
        }

        public void Serialize(List<double> valueList)
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Count;

            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                double value = valueList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<double> valueList)
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<double>();

            if (listLength == 0)
            {
                valueList = null;
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                double value = 0;
                Deserialize(out value);
                valueList.Add(value);
            }
            index += sizeof(double) * listLength;
        }

        public void Serialize(List<short> valueList)
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Count;

            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                short value = valueList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<short> valueList)
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<short>();

            if (listLength == 0)
            {
                valueList = null;
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                short value = 0;
                Deserialize(out value);
                valueList.Add(value);
            }
            index += sizeof(short) * listLength;
        }

        public void Serialize(List<byte> valueList)
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Count;

            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                byte value = valueList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<byte> valueList)
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<byte>();

            if (listLength == 0)
            {
                valueList = null;
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                byte value = 0;
                Deserialize(out value);
                valueList.Add(value);
            }
            index += sizeof(byte) * listLength;
        }

        public void Serialize(byte [] valueList)
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Length;

            byteList.AddRange(BitConverter.GetBytes(listLength));

            if(listLength > 0)
                byteList.AddRange(valueList);
        }

        public void Deserialize(out byte[] valueList)
        {
            int arrayLength = BitConverter.ToInt32(stream, index);
            index += sizeof(int);

            valueList = new byte[arrayLength];

            if (arrayLength == 0)
            {
                valueList = null;
                return;
            }

            Array.Copy(stream, index, valueList, 0, arrayLength);
            index += sizeof(byte) * arrayLength;
        }

        public void Serialize(List<bool> valueList)
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Count;

            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                bool value = valueList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<bool> valueList)
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<bool>();

            if (listLength == 0)
            {
                valueList = null;
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                bool value = false;
                Deserialize(out value);
                valueList.Add(value);
            }
            index += sizeof(bool) * listLength;
        }

        public void Serialize(List<char> valueList)
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Count;

            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                char value = valueList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<char> valueList)
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<Char>();
            if (listLength == 0)
            {
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                char value = '0';
                Deserialize(out value);
                valueList.Add(value);
            }
            index += sizeof(char) * listLength;
        }
#if UNITY
        public void Serialize(List<Vector3> valueList)
        {
            int listLength = valueList.Count;
            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                Vector3 value = valueList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<Vector3> valueList)
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<Vector3>();

            if (listLength == 0)
            {
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                Vector3 value = new Vector3();
                Deserialize(out value);
                valueList.Add(value);
            }
        }

        public void Serialize(List<Quaternion> serialList)
        {
            int listLength = serialList.Count;
            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                Quaternion value = serialList[i];
                Serialize(value);
            }
        }

        public void Deserialize(out List<Quaternion> valueList)
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);
            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<Quaternion>();

            if (listLength == 0)
            {
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                Quaternion value = Quaternion.identity;
                Deserialize(out value);
                valueList.Add(value);
            }
        }
#endif

        public void Serialize<T>(List<T> valueList)
            where T : ISerializable, new() 
        {
            int listLength = 0;

            if (valueList != null)
                listLength = valueList.Count;

            //Add the length of the list to the first position
            byteList.AddRange(BitConverter.GetBytes(listLength));

            //Serialize all values in list
            for (int i = 0; i < listLength; i++)
            {
                Serialize(valueList[i]);
            }
        }

        public void Deserialize<T>(out List<T> valueList)
            where T : ISerializable, new()
        {
            //Get the length of the list
            int listLength = BitConverter.ToInt32(stream, index);

            //Increment the byte pointer by the size of one int
            index += sizeof(int);

            valueList = new List<T>();

            if (listLength == 0)
            {
                valueList = null;
                return;
            }

            //get all values for the next 'i' count data and add it to the list
            //The serialize calls take care of the index pointer
            for (int i = 0; i < listLength; i++)
            {
                T value = new T();
                Deserialize<T>(out value);
                valueList.Add(value);
            }
        }
    }
}