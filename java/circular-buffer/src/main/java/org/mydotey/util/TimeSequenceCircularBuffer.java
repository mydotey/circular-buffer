package org.mydotey.util;

import java.util.Objects;
import java.util.concurrent.locks.ReentrantLock;
import java.util.function.Consumer;

/**
 * @author koqizhao
 *
 * Jul 25, 2018
 */
@SuppressWarnings("unchecked")
public abstract class TimeSequenceCircularBuffer<T extends TimeBucket> {

    private Object[] _buckets;
    private TimeSequenceCircularBufferConfig _bufferConfig;

    private ReentrantLock _addBucketLock = new ReentrantLock();
    private volatile int _bufferEnd;
    private volatile T _spareBucket;

    protected TimeSequenceCircularBuffer(TimeSequenceCircularBufferConfig bufferConfig) {
        Objects.requireNonNull(bufferConfig, "bufferConfig is null");

        _bufferConfig = bufferConfig;
        _buckets = new Object[_bufferConfig.getBucketCount() + 1];

        for (int i = 0; i < _buckets.length; i++) {
            _buckets[i] = createBucket(0, _bufferConfig.getTimeWindow());
        }

        _spareBucket = createBucket(0, _bufferConfig.getTimeWindow());
    }

    public TimeSequenceCircularBufferConfig getConfig() {
        return _bufferConfig;
    }

    protected abstract T createBucket(long startTime, long ttl);

    protected void forEach(Consumer<T> consumer) {
        Objects.requireNonNull(consumer, "consumer is null");

        for (Object item : _buckets) {
            T bucket = (T) item;
            if (bucket.isStale())
                continue;

            consumer.accept(bucket);
        }
    }

    protected T getCurrentBucket() {
        long currentBucketStartTime = getCurrentBucketStartTime();
        if (!needRefreshCurrentBucket(currentBucketStartTime))
            return (T) _buckets[_bufferEnd];

        if (!_addBucketLock.tryLock())
            return (T) _buckets[_bufferEnd];

        try {
            currentBucketStartTime = getCurrentBucketStartTime();
            if (!needRefreshCurrentBucket(currentBucketStartTime))
                return (T) _buckets[_bufferEnd];

            int newBufferEnd = (_bufferEnd + 1) % _buckets.length;
            T oldBucket = (T) _buckets[newBufferEnd];
            _spareBucket.reset(currentBucketStartTime);
            _buckets[newBufferEnd] = _spareBucket;
            _bufferEnd = newBufferEnd;
            oldBucket.reset(0);
            _spareBucket = oldBucket;
            return (T) _buckets[_bufferEnd];
        } finally {
            _addBucketLock.unlock();
        }
    }

    private long getCurrentBucketStartTime() {
        long currentTime = System.currentTimeMillis();
        return currentTime - currentTime % _bufferConfig.getBucketTtl();
    }

    private boolean needRefreshCurrentBucket(long currentBucketStartTime) {
        return ((T) _buckets[_bufferEnd]).getStartTime() + _bufferConfig.getBucketTtl() <= currentBucketStartTime;
    }

}
