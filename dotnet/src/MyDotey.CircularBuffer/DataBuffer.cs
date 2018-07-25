using System;

namespace MyDotey.Util
{
    /**
     * @author koqizhao
     *
     * Jul 25, 2018
     */
    public class DataBuffer<T> : TimeSequenceCircularBuffer<DataBucket<T>>
    {
        public DataBuffer(DataBufferConfig bufferConfig)
            : base(bufferConfig)
        {
        }

        public new DataBufferConfig Config
        {
            get { return (DataBufferConfig)base.Config; }
        }

        protected override DataBucket<T> NewBucket(long startTime, long ttl)
        {
            return new DataBucket<T>(startTime, ttl, Config.BucketCapacity);
        }

        public void Add(T data)
        {
            CurrentBucket.Add(data);
        }

        public void Consume(Action<T> consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer is null");

            ForEach(bucket =>
            {
                for (int i = 0; i < bucket.Count; i++)
                {
                    T item = bucket.Get(i);
                    if (item == null)
                        continue;
                    consumer(item);
                }
            });
        }
    }
}