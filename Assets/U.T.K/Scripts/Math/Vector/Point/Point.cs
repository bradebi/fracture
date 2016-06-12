using System;
using UTK.Utilities;
namespace UTK.Math
{
    public abstract class Point<T>
    {
        protected T _x = default(T);
        public T x
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        protected T _y = default(T);
        public T y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }

        public abstract T Distance();

        public abstract T Distance(Point<T> point);

        public abstract void FromBytes(byte[] data);

        public abstract byte[] ToBytes();
    }
}