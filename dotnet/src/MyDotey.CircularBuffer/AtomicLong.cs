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
    internal class AtomicLong
    {
        private long _value;

        public long IncrementAndGet()
        {
            return Interlocked.Increment(ref _value);
        }

        public long DecrementAndGet()
        {
            return Interlocked.Decrement(ref _value);
        }

        public long Get()
        {
            return Interlocked.Read(ref _value);
        }
    }
}