package org.mydotey.util;

/**
 * @author koqizhao
 *
 * Jul 25, 2018
 */
public abstract class TimeBucket {

    private volatile long _startTime;
    private long _ttl;

    protected TimeBucket(long startTime, long ttl) {
        _startTime = startTime;
        _ttl = ttl;
    }

    public long getStartTime() {
        return _startTime;
    }

    public long getTtl() {
        return _ttl;
    }

    public void reset(long startTime) {
        _startTime = startTime;
    }

    public boolean isStale() {
        return System.currentTimeMillis() - _startTime > _ttl;
    }

}
