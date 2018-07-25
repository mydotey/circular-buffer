package org.mydotey.util;

import java.util.concurrent.atomic.AtomicInteger;

/**
 * @author koqizhao
 *
 * Jul 25, 2018
 */
public class DataBucket<T> extends TimeBucket {

    private final Object[] _data;
    private final AtomicInteger _count;

    public DataBucket(long startTime, long ttl, int capacity) {
        super(startTime, ttl);

        _data = new Object[capacity];
        _count = new AtomicInteger();
    }

    public int count() {
        return Math.min(_count.get(), _data.length);
    }

    public void add(T value) {
        if (_data.length == 0)
            return;

        if (value == null)
            return;

        int index = _count.getAndIncrement() % _data.length;
        _data[index] = value;
    }

    @SuppressWarnings("unchecked")
    public T get(int index) {
        return (T) _data[index];
    }

    @Override
    public void reset(long startTime) {
        super.reset(startTime);
        for (int i = 0; i < _data.length; i++)
            _data[i] = null;
        _count.set(0);
    }

}
