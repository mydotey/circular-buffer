package org.mydotey.util;

import java.util.concurrent.atomic.AtomicLong;

/**
 * @author koqizhao
 *
 * Jul 25, 2018
 */
public class CounterBuffer<T> extends TimeSequenceCircularBuffer<CounterBucket<T>> {

    public CounterBuffer(TimeSequenceCircularBufferConfig bufferConfig) {
        super(bufferConfig);
    }

    @Override
    protected CounterBucket<T> createBucket(long startTime, long ttl) {
        return new CounterBucket<T>(startTime, ttl);
    }

    public long get(final T identity) {
        final AtomicLong count = new AtomicLong();
        forEach(bucket -> count.addAndGet(bucket.get(identity)));
        return count.get();
    }

    public void increment(T identity) {
        getCurrentBucket().increment(identity);
    }

    public void decrement(T identity) {
        getCurrentBucket().decrement(identity);
    }

}
