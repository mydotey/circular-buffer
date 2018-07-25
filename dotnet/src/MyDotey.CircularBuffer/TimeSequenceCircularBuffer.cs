using System;
using System.Threading;

namespace MyDotey.Util
{
    /**
     * @author koqizhao
     *
     * Jul 25, 2018
     */
    public abstract class TimeSequenceCircularBuffer<T>
        where T : TimeBucket
    {
        private T[] _buckets;
        private TimeSequenceCircularBufferConfig _bufferConfig;

        private object _addBucketLock = new object();
        private volatile int _bufferEnd;
        private volatile T _spareBucket;

        protected TimeSequenceCircularBuffer(TimeSequenceCircularBufferConfig bufferConfig)
        {
            if (bufferConfig == null)
                throw new ArgumentNullException("bufferConfig is null");

            _bufferConfig = bufferConfig;
            _buckets = new T[_bufferConfig.BucketCount + 1];

            for (int i = 0; i < _buckets.Length; i++)
            {
                _buckets[i] = NewBucket(0, _bufferConfig.TimeWindow);
            }

            _spareBucket = NewBucket(0, _bufferConfig.TimeWindow);
        }

        public TimeSequenceCircularBufferConfig Config
        {
            get { return _bufferConfig; }
        }

        protected abstract T NewBucket(long startTime, long ttl);

        protected void ForEach(Action<T> consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer is null");

            foreach (T bucket in _buckets)
            {
                if (bucket.IsStale)
                    continue;

                consumer(bucket);
            }
        }

        protected T CurrentBucket
        {
            get
            {
                long currentBucketStartTime = CurrentBucketStartTime;
                if (!NeedRefreshCurrentBucket(currentBucketStartTime))
                    return _buckets[_bufferEnd];

                if (!Monitor.TryEnter(_addBucketLock))
                    return _buckets[_bufferEnd];

                try
                {
                    currentBucketStartTime = CurrentBucketStartTime;
                    if (!NeedRefreshCurrentBucket(currentBucketStartTime))
                        return _buckets[_bufferEnd];

                    int newBufferEnd = (_bufferEnd + 1) % _buckets.Length;
                    T oldBucket = _buckets[newBufferEnd];
                    _spareBucket.Reset(currentBucketStartTime);
                    _buckets[newBufferEnd] = _spareBucket;
                    _bufferEnd = newBufferEnd;
                    oldBucket.Reset(0);
                    _spareBucket = oldBucket;
                    return _buckets[_bufferEnd];
                }
                finally
                {
                    Monitor.Exit(_addBucketLock);
                }
            }
        }

        private long CurrentBucketStartTime
        {
            get
            {
                long currentTime = TimeBucket.CurrentTimeMillis;
                return currentTime - currentTime % _bufferConfig.BucketTtl;
            }
        }

        private bool NeedRefreshCurrentBucket(long currentBucketStartTime)
        {
            return _buckets[_bufferEnd].StartTime + _bufferConfig.BucketTtl <= currentBucketStartTime;
        }
    }
}