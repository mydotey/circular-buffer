using System;
using System.Threading;
using Xunit;

namespace MyDotey.Util
{
    /**
     * @author koqizhao
     *
     * Jul 25, 2018
     */
    public class CircularBufferTest
    {
        [Fact]
        public void TestDemo()
        {
            int timeWindow = 10 * 1000;
            int bucketTtl = 1 * 1000;
            int bucketCapacity = 1 * 1000;

            int sleepTime = 100;
            int times = 20 * 1000 / sleepTime;

            TimeSequenceCircularBufferConfig counterBufferConfig = new TimeSequenceCircularBufferConfig.Builder()
                    .SetTimeWindow(timeWindow).SetBucketTtl(bucketTtl).Build();
            CounterBuffer<String> counterBuffer = new CounterBuffer<String>(counterBufferConfig);
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(sleepTime);
                counterBuffer.Increment("key1");
                counterBuffer.Increment("key2");
            }
            Console.WriteLine("key1 count: {0}, key2 count: {1}", counterBuffer.Get("key1"), counterBuffer.Get("key2"));

            DataBufferConfig dataBufferConfig = new DataBufferConfig.Builder().SetTimeWindow(timeWindow)
                    .SetBucketTtl(bucketTtl).SetBucketCapacity(bucketCapacity).Build();
            DataBuffer<int> dataBuffer = new DataBuffer<int>(dataBufferConfig);
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(sleepTime);
                dataBuffer.Add(i);
            }
            int sum = 0;
            dataBuffer.Consume(i => sum += i);
            Console.WriteLine("sum: {0}", sum);
        }
    }
}