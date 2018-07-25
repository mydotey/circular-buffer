# Circular Buffer

## maven dependency

```xml
<dependency>
    <groupId>org.mydotey.circularbuffer</groupId>
    <artifactId>circular-buffer</artifactId>
    <version>1.0.0</version>
</dependency>
```

## Usage

```java
@Test
public void testDemo() throws InterruptedException {
    int timeWindow = 10 * 1000;
    int bucketTtl = 1 * 1000;
    int bucketCapacity = 1 * 1000;

    int sleepTime = 100;
    int times = 20 * 1000 / sleepTime;

    TimeSequenceCircularBufferConfig counterBufferConfig = new TimeSequenceCircularBufferConfig.Builder()
            .setTimeWindow(timeWindow).setBucketTtl(bucketTtl).build();
    CounterBuffer<String> counterBuffer = new CounterBuffer<>(counterBufferConfig);
    for (int i = 0; i < times; i++) {
        Thread.sleep(sleepTime);
        counterBuffer.increment("key1");
        counterBuffer.increment("key2");
    }
    System.out.printf("key1 count: %s, key2 count: %s\n", counterBuffer.get("key1"), counterBuffer.get("key2"));

    DataBufferConfig dataBufferConfig = new DataBufferConfig.Builder().setTimeWindow(timeWindow)
            .setBucketTtl(bucketTtl).setBucketCapacity(bucketCapacity).build();
    DataBuffer<Integer> dataBuffer = new DataBuffer<>(dataBufferConfig);
    for (int i = 0; i < times; i++) {
        Thread.sleep(sleepTime);
        dataBuffer.add(i);
    }
    AtomicLong sum = new AtomicLong();
    dataBuffer.consume(sum::addAndGet);
    System.out.printf("sum: %s\n", sum);
}
```
