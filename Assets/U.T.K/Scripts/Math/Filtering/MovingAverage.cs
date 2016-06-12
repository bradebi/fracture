using System.Collections.Generic;
namespace UTK.Math.Filtering
{
    public class MovingAverageFilter
    {
        Queue<float> filteringData;
        float summation = 0;
        int _length = 0;

        float _average = 0;

        public float Average
        {
            get
            {
                return _average;
            }
        }

        public MovingAverageFilter(int length)
        {
            _length = length < 1 ? 1 : length;

            filteringData = new Queue<float>();
        }

        public float Insert(float data)
        {
            filteringData.Enqueue(data);

            summation += data;

            //If we exceed the buffer size, then dequeue the value
            if (filteringData.Count > _length)
            {
                summation -= filteringData.Dequeue();
            }

            _average = summation / filteringData.Count;

            return _average;
        }

    }
}
