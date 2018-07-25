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
    internal class AtomicInteger
    {
        private volatile int _value;

        public int GetAndIncrement()
        {
            return Interlocked.Increment(ref _value) + 1;
        }

        public int Get()
        {
            return _value;
        }

        public void Set(int value)
        {
            _value = value;
        }
    }
}