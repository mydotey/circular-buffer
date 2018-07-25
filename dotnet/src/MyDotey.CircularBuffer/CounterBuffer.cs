using System;

namespace MyDotey.Util
{
    /**
     * @author koqizhao
     *
     * Jul 25, 2018
     */
    public class CounterBuffer<T> : TimeSequenceCircularBuffer<CounterBucket<T>>
    {
        public CounterBuffer(TimeSequenceCircularBufferConfig bufferConfig)
            : base(bufferConfig)
        {
        }

        protected override CounterBucket<T> NewBucket(long startTime, long ttl)
        {
            return new CounterBucket<T>(startTime, ttl);
        }

        public long Get(T identity)
        {
            long count = 0;
            ForEach(bucket => count += bucket.Get(identity));
            return count;
        }

        public void Increment(T identity)
        {
            CurrentBucket.Increment(identity);
        }

        public void Decrement(T identity)
        {
            CurrentBucket.Decrement(identity);
        }
    }
}