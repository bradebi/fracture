using System;
using UTK.Utilities;
#if UNITY
    using UnityEngine;
#endif

namespace UTK.Math
{
    public struct Point2d : ISerializable
    {

        public double x;

        public double y;

        public Point2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point2d zero
        {
            get
            {
                return new Point2d(0, 0);
            }
        }

#if UNITY
        public static explicit operator Point2d(Vector2 vector)
        {
            return new Point2d(vector.x, vector.y);
        }

        public static explicit operator Point2d(Vector3 vector)
        {
            return new Point2d(vector.x, vector.y);
        }

        public static explicit operator Vector2(Point2d vector)
        {
            return new Vector2((float)vector.x, (float)vector.y);
        }

        public static explicit operator Vector3(Point2d vector)
        {
            return new Vector3((float)vector.x, (float)vector.y, 0);
        }
#endif

        public static explicit operator Point2d(Point3f vector)
        {
            return new Point2d(vector.x, vector.y);
        }

        public double Distance()
        {
            throw new NotImplementedException();
        }

        public double Distance(Point<double> point)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "X: " + x + " Y : " + y;
        }

        public byte[] ToBytes()
        {
            ByteStream data = new ByteStream();

            data.Serialize(x);
            data.Serialize(y);

            return data;
        }

        public override bool Equals(object obj)
        {
            
            return Equals((Point2d)obj);
        }

        public bool Equals(Point2d p)
        {
            // Return true if the fields match:
            return Scalar.AreEqual(p.x, x, 0.0001f) && Scalar.AreEqual(p.y, y, 0.0001f);
        }

        public void FromBytes(byte[] dataIn)
        {
            ByteStream data = new ByteStream(dataIn);

            data.Deserialize(out x);
            data.Deserialize(out y);
        }


        #region Operators

        public static bool operator ==(Point2d value1, Point2d value2)
        {
            return value1.x == value2.x
                && value1.y == value2.y;
        }

        public static bool operator !=(Point2d value1, Point2d value2)
        {
            return !(value1 == value2);
        }

        public static Point2d operator +(Point2d value1, Point2d value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static Point2d operator -(Point2d value)
        {
            value = new Point2d(-value.x, -value.y);
            return value;
        }

        public static Point2d operator -(Point2d value1, Point2d value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static Point2d operator *(Point2d value1, Point2d value2)
        {
            value1.x *= value2.x;
            value1.y *= value2.y;
            return value1;
        }

        public static Point2d operator *(Point2d value, double scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }

        public static Point2d operator *(double scaleFactor, Point2d value)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }

        public static Point2d operator /(Point2d value1, Point2d value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }

        public static Point2d operator /(Point2d value, double divider)
        {
            double factor = 1 / divider;
            value.x *= factor;
            value.y *= factor;
            return value;
        }
        #endregion
    }
}
