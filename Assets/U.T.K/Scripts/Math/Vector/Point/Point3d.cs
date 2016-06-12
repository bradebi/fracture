using UTK.Utilities;

#if UNITY
using UnityEngine;
#endif

namespace UTK.Math
{

    public struct Point3d : ISerializable
    {
        public double y;
        public double z;
        public double x;

        public Point3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

#if UNITY

        public Point3d(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public static explicit operator Point3d(Vector2 vector)
        {
            return new Point3d(vector.x, vector.y, 0);
        }

        public static explicit operator Point3d(Vector3 vector)
        {
            return new Point3d(vector.x, vector.y, vector.z);
        }

        public static explicit operator Vector2(Point3d vector)
        {
            return new Vector2((float)vector.x, (float)vector.y);
        }

        public static explicit operator Vector3(Point3d vector)
        {
            return new Vector3((float)vector.x, (float)vector.y, (float)vector.z);
        }
#endif

        public override bool Equals(object obj)
        {

            return Equals((Point3d)obj);
        }

        public bool Equals(Point3d p)
        {
            // Return true if the fields match:
            return Scalar.AreEqual(p.x, x, 0.0001f) && Scalar.AreEqual(p.y, y, 0.0001f) && Scalar.AreEqual(p.z, y, 0.0001f);
        }

        public static Point3d zero
        {
            get
            {
                return new Point3d(0, 0, 0);
            }
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

        public override string ToString()
        {
            return ("(" + x + ", " + y + ", " + z + ")");
        }

        #region Operators

        public static bool operator ==(Point3d value1, Point3d value2)
        {
            return value1.x == value2.x
                && value1.y == value2.y
                && value1.z == value2.z;
        }

        public static bool operator !=(Point3d value1, Point3d value2)
        {
            return !(value1 == value2);
        }

        public static Point3d operator +(Point3d value1, Point3d value2)
        {
            return new Point3d(value1.x + value2.x, value1.y + value2.y, value1.z + value2.z);
        }

        public static Point3d operator -(Point3d value)
        {
            return new Point3d(-value.x, -value.y, -value.z); ;
        }

        public static Point3d operator -(Point3d value1, Point3d value2)
        {
            return new Point3d(value1.x - value2.x, value1.y - value2.y, value1.z - value2.z);
        }

        public static Point3d operator *(Point3d value1, Point3d value2)
        {
            return new Point3d(value1.x * value2.x, value1.y * value2.y, value1.z * value2.z);
        }

        public static Point3d operator *(Point3d value, double scaleFactor)
        {
            return new Point3d(value.x * scaleFactor, value.y * scaleFactor, value.z * scaleFactor);
        }

        public static Point3d operator *(double scaleFactor, Point3d value)
        {
            return new Point3d(value.x * scaleFactor, value.y * scaleFactor, value.z * scaleFactor);
        }

        public static Point3d operator /(Point3d value1, Point3d value2)
        {
            return new Point3d(value1.x / value2.x, value1.y / value2.y, value1.z / value2.z);
        }

        public static Point3d operator /(Point3d value, double divider)
        {
            double factor = 1 / divider;
            return new Point3d(value.x * factor, value.y * factor, value.z * factor);
        }

        #endregion

    }
}
