package org.mydotey.util;

/**
 * @author koqizhao
 *
 * Jul 25, 2018
 */
public class DataBufferConfig extends TimeSequenceCircularBufferConfig {

    private int _bucketCapacity;

    public DataBufferConfig() {

    }

    public int getBucketCapacity() {
        return _bucketCapacity;
    }

    @Override
    public String toString() {
        return String.format("%s { timeWindow: %s, bucketTtl: %s, bucketCount: %s, bucketCapacity: %s }",
                getClass().getSimpleName(), getTimeWindow(), getBucketTtl(), getBucketCount(), _bucketCapacity);
    }

    public static class Builder extends TimeSequenceCircularBufferConfig.AbstractBuilder<Builder> {

        @Override
        protected DataBufferConfig newConfig() {
            return new DataBufferConfig();
        }

        @Override
        protected DataBufferConfig getConfig() {
            return (DataBufferConfig) super.getConfig();
        }

        public Builder setBucketCapacity(int bucketCapacity) {
            getConfig()._bucketCapacity = bucketCapacity;
            return this;
        }

        @Override
        public DataBufferConfig build() {
            if (getConfig()._bucketCapacity <= 0)
                throw new IllegalArgumentException("bucketCapacity not set");

            return (DataBufferConfig) super.build();
        }
    }

}
