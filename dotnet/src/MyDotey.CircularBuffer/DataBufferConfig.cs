using System;

namespace MyDotey.Util
{
    /**
     * @author koqizhao
     *
     * Jul 25, 2018
     */
    public class DataBufferConfig : TimeSequenceCircularBufferConfig
    {
        private int _bucketCapacity;

        public DataBufferConfig()
        {

        }

        public int BucketCapacity
        {
            get { return _bucketCapacity; }
        }

        public override String ToString()
        {
            return String.Format("{0} {{ timeWindow: {1}, bucketTtl: {2}, bucketCount: {3}, bucketCapacity: {4} }}",
                    GetType().Name, TimeWindow, BucketTtl, BucketCount, BucketCapacity);
        }

        public new class Builder : TimeSequenceCircularBufferConfig.AbstractBuilder<Builder, DataBufferConfig>
        {
            protected override TimeSequenceCircularBufferConfig NewConfig()
            {
                return new DataBufferConfig();
            }

            protected new DataBufferConfig Config
            {
                get { return (DataBufferConfig)base.Config; }
            }

            public Builder SetBucketCapacity(int bucketCapacity)
            {
                Config._bucketCapacity = bucketCapacity;
                return this;
            }

            public override DataBufferConfig Build()
            {
                if (Config._bucketCapacity <= 0)
                    throw new ArgumentNullException("bucketCapacity not set");

                return (DataBufferConfig)base.Build();
            }
        }
    }
}