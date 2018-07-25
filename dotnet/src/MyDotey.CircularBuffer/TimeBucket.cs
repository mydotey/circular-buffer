using System;
using System.Threading;

namespace MyDotey.Util
{
    /**
     * @author koqizhao
     *
     * Jul 25, 2018
     */
    public abstract class TimeBucket
    {
        protected internal static long CurrentTimeMillis { get { return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; } }

        private long _startTime;
        private long _ttl;

        protected TimeBucket(long startTime, long ttl)
        {
            _startTime = startTime;
            _ttl = ttl;
        }

        public long StartTime
        {
            get { return _startTime; }
        }

        public long Ttl
        {
            get { return _ttl; }
        }

        public virtual void Reset(long startTime)
        {
            Interlocked.Exchange(ref _startTime, startTime);
        }

        public bool IsStale
        {
            get { return CurrentTimeMillis - _startTime > _ttl; }
        }
    }
}