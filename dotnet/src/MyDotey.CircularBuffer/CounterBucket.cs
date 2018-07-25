using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MyDotey.Util
{
    /**
     * @author koqizhao
     *
     * Jul 25, 2018
     */
    public class CounterBucket<T> : TimeBucket
    {
        private ConcurrentDictionary<T, AtomicLong> _counters;

        public CounterBucket(long startTime, long ttl)
            : base(startTime, ttl)
        {
            _counters = new ConcurrentDictionary<T, AtomicLong>();
        }

        public long Get(T identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity is null");

            _counters.TryGetValue(identity, out AtomicLong counter);
            return counter == null ? 0 : counter.Get();
        }

        public void Increment(T identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity is null");

            AtomicLong counter = GetCounter(identity);
            counter.IncrementAndGet();
        }

        public void Decrement(T identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity is null");

            AtomicLong counter = GetCounter(identity);
            counter.DecrementAndGet();
        }

        private AtomicLong GetCounter(T identity)
        {
            return _counters.GetOrAdd(identity, k => new AtomicLong());
        }

        public override void Reset(long startTime)
        {
            base.Reset(startTime);
            _counters.Clear();
        }
    }
}