package org.mydotey.util;

import java.util.Objects;
import java.util.function.Consumer;

/**
 * @author koqizhao
 *
 * Jul 25, 2018
 */
public class DataBuffer<T> extends TimeSequenceCircularBuffer<DataBucket<T>> {

    public DataBuffer(DataBufferConfig bufferConfig) {
        super(bufferConfig);
    }

    @Override
    public DataBufferConfig getConfig() {
        return (DataBufferConfig) super.getConfig();
    }

    @Override
    protected DataBucket<T> newBucket(long startTime, long ttl) {
        return new DataBucket<T>(startTime, ttl, getConfig().getBucketCapacity());
    }

    public void add(T data) {
        getCurrentBucket().add(data);
    }

    public void consume(Consumer<T> consumer) {
        Objects.requireNonNull(consumer, "consumer is null");

        forEach(bucket -> {
            for (int i = 0; i < bucket.count(); i++) {
                T item = bucket.get(i);
                if (item == null)
                    continue;
                consumer.accept(item);
            }
        });
    }

}
