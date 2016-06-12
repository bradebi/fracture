using System;
using UTK.Utilities;

#if UNITY
    using UnityEngine;
#endif


namespace UTK.Math
{
    public struct Point3f : ISerializable
    {
        public float y;
        public float z;
        public float x;

        public Point3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point3f(float x, float y)
        {
            this.x = x;
            this.y = y;
            z = 0;
        }

        public float Distance()
        {
            throw new NotImplementedException();
        }

        public float Distance(Point3f point)
        {
            throw new NotImplementedException();
        }

        public byte[] ToBytes()
        {
            ByteStream data = new ByteStream();

            data.Serialize(x);
            data.Serialize(y);
            data.Serialize(z);

            return data;
        }

        public void FromBytes(byte[] dataIn)
        {
            ByteStream data = new ByteStream(dataIn);

            data.Deserialize(out x);
            data.Deserialize(out y);
            data.Deserialize(out z);
        }

        #region Operators

        public static bool operator ==(Point3f value1, Point3f value2)
        {
            return value1.x == value2.x
                && value1.y == value2.y
                && value1.z == value2.z;
        }

        public static bool operator !=(Point3f value1, Point3f value2)
        {
            return !(value1 == value2);
        }

        public static Point3f operator +(Point3f value1, Point3f value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            value1.z += value2.z;
            return value1;
        }

        public static Point3f operator -(Point3f value)
        {
            value = new Point3f(-value.x, -value.y, -value.z);
            return value;
        }

        public static Point3f operator -(Point3f value1, Point3f value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            value1.z -= value2.z;
            return value1;
        }

        public static Point3f operator *(Point3f value1, Point3f value2)
        {
            value1.x *= value2.x;
            value1.y *= value2.y;
            value1.z *= value2.z;
            return value1;
        }

        public static Point3f operator *(Point3f value, float scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            value.z *= scaleFactor;
            return value;
        }

        public static Point3f operator *(float scaleFactor, Point3f value)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            value.z *= scaleFactor;
            return value;
        }

        public static Point3f operator /(Point3f value1, Point3f value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            value1.z /= value2.z;
            return value1;
        }

        public static Point3f operator /(Point3f value, float divider)
        {
            float factor = 1 / divider;
            value.x *= factor;
            value.y *= factor;
            value.z *= factor;
            return value;
        }

        #endregion
    }
}