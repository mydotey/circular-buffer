package org.mydotey.util;

/**
 * @author koqizhao
 *
 * Jul 25, 2018
 */
public class TimeSequenceCircularBufferConfig implements Cloneable {

    private long _timeWindow;
    private long _bucketTtl;
    private int _bucketCount;

    protected TimeSequenceCircularBufferConfig() {

    }

    public long getTimeWindow() {
        return _timeWindow;
    }

    public long getBucketTtl() {
        return _bucketTtl;
    }

    public int getBucketCount() {
        return _bucketCount;
    }

    @Override
    public TimeSequenceCircularBufferConfig clone() {
        try {
            return (TimeSequenceCircularBufferConfig) super.clone();
        } catch (CloneNotSupportedException e) {
            e.printStackTrace();
            return null;
        }
    }

    @Override
    public String toString() {
        return String.format("%s { timeWindow: %s, bucketTtl: %s, bucketCount: %s }", getClass().getSimpleName(),
                _timeWindow, _bucketTtl, _bucketCount);
    }

    public static class Builder extends AbstractBuilder<Builder> {

    }

    @SuppressWarnings("unchecked")
    public static abstract class AbstractBuilder<B extends AbstractBuilder<B>> {

        private TimeSequenceCircularBufferConfig _config;

        protected AbstractBuilder() {
            _config = newConfig();
        }

        protected TimeSequenceCircularBufferConfig newConfig() {
            return new TimeSequenceCircularBufferConfig();
        }

        protected TimeSequenceCircularBufferConfig getConfig() {
            return _config;
        }

        public B setTimeWindow(long timeWindow) {
            _config._timeWindow = timeWindow;
            return (B) this;
        }

        public B setBucketTtl(long bucketTtl) {
            _config._bucketTtl = bucketTtl;
            return (B) this;
        }

        public TimeSequenceCircularBufferConfig build() {
            if (_config._timeWindow <= 0)
                throw new IllegalArgumentException("timeWindow not set");

            if (_config._bucketTtl <= 0)
                throw new IllegalArgumentException("bucketTtl not set");

            if (_config._timeWindow % _config._bucketTtl != 0)
                throw new IllegalArgumentException(String.format("timeWindow %s cannot be divided by bucketTtl %s.",
                        _config._timeWindow, _config._bucketTtl));

            long bucketCount = _config._timeWindow / _config._bucketTtl;
            if (bucketCount > Integer.MAX_VALUE)
                throw new IllegalArgumentException(String.format("timeWindow %s too large", _config._timeWindow));

            _config._bucketCount = (int) bucketCount;
            return _config.clone();
        }

    }

}
