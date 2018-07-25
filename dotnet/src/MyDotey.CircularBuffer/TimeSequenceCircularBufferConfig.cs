using System;

namespace MyDotey.Util
{
    /**
     * @author koqizhao
     *
     * Jul 25, 2018
     */
    public class TimeSequenceCircularBufferConfig : ICloneable
    {
        private long _timeWindow;
        private long _bucketTtl;
        private int _bucketCount;

        protected TimeSequenceCircularBufferConfig()
        {

        }

        public long TimeWindow
        {
            get { return _timeWindow; }
        }

        public long BucketTtl
        {
            get { return _bucketTtl; }
        }

        public int BucketCount
        {
            get { return _bucketCount; }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override String ToString()
        {
            return String.Format("{0} {{ timeWindow: {1}, bucketTtl: {2}, bucketCount: {3} }}", GetType().Name,
                    _timeWindow, _bucketTtl, _bucketCount);
        }

        public class Builder : AbstractBuilder<Builder, TimeSequenceCircularBufferConfig>
        {

        }

        public abstract class AbstractBuilder<B, C>
            where B : AbstractBuilder<B, C>
        {
            private TimeSequenceCircularBufferConfig _config;

            protected AbstractBuilder()
            {
                _config = NewConfig();
            }

            protected virtual TimeSequenceCircularBufferConfig NewConfig()
            {
                return new TimeSequenceCircularBufferConfig();
            }

            protected TimeSequenceCircularBufferConfig Config
            {
                get { return _config; }
            }

            public B SetTimeWindow(long timeWindow)
            {
                _config._timeWindow = timeWindow;
                return (B)this;
            }

            public B SetBucketTtl(long bucketTtl)
            {
                _config._bucketTtl = bucketTtl;
                return (B)this;
            }

            public virtual C Build()
            {
                if (_config._timeWindow <= 0)
                    throw new ArgumentNullException("timeWindow not set");

                if (_config._bucketTtl <= 0)
                    throw new ArgumentNullException("bucketTtl not set");

                if (_config._timeWindow % _config._bucketTtl != 0)
                    throw new ArgumentException(String.Format("timeWindow {0} cannot be divided by bucketTtl {1}.",
                            _config._timeWindow, _config._bucketTtl));

                long bucketCount = _config._timeWindow / _config._bucketTtl;
                if (bucketCount > int.MaxValue)
                    throw new ArgumentException(String.Format("timeWindow {0} too large", _config._timeWindow));

                _config._bucketCount = (int)bucketCount;
                return (C)_config.Clone();
            }
        }
    }
}