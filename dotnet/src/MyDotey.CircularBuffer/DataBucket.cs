using System;
using System.Threading;

namespace MyDotey.Util
{
    /**
     * @author koqizhao
     *
     * Jul 25, 2018
     */
    public class DataBucket<T> : TimeBucket
    {
        private T[] _data;
        private AtomicInteger _count;

        public DataBucket(long startTime, long ttl, int capacity)
            : base(startTime, ttl)
        {

            _data = new T[capacity];
            _count = new AtomicInteger();
        }

        public int Count
        {
            get { return Math.Min(_count.Get(), _data.Length); }
        }

        public void Add(T value)
        {
            if (_data.Length == 0)
                return;

            if (value == null)
                return;

            int index = _count.GetAndIncrement() % _data.Length;
            _data[index] = value;
        }

        public T Get(int index)
        {
            return _data[index];
        }

        public override void Reset(long startTime)
        {
            base.Reset(startTime);
            for (int i = 0; i < _data.Length; i++)
                _data[i] = default(T);
            _count.Set(0);
        }
    }
}