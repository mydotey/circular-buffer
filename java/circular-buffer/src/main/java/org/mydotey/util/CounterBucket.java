package org.mydotey.util;

import java.util.Objects;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicLong;
import java.util.function.Function;

/**
 * @author koqizhao
 *
 * Jul 25, 2018
 */
public class CounterBucket<T> extends TimeBucket {

    protected static final Function<Object, AtomicLong> COUNTER_CREATOR = k -> new AtomicLong();

    private final ConcurrentHashMap<T, AtomicLong> _counters;

    public CounterBucket(long startTime, long ttl) {
        super(startTime, ttl);
        _counters = new ConcurrentHashMap<T, AtomicLong>();
    }

    public long get(T identity) {
        Objects.requireNonNull(identity, "identity is null");
        AtomicLong counter = _counters.get(identity);
        return counter == null ? 0 : counter.get();
    }

    public void increment(T identity) {
        Objects.requireNonNull(identity, "identity is null");
        AtomicLong counter = getCounter(identity);
        counter.incrementAndGet();
    }

    public void decrement(T identity) {
        Objects.requireNonNull(identity, "identity is null");
        AtomicLong counter = getCounter(identity);
        counter.decrementAndGet();
    }

    private AtomicLong getCounter(T identity) {
        return _counters.computeIfAbsent(identity, COUNTER_CREATOR);
    }

    @Override
    public void reset(long startTime) {
        super.reset(startTime);
        _counters.clear();
    }

}
