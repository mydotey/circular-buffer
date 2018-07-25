# Circular Buffer

## NuGet package

```sh
dotnet add package MyDotey.CircularBuffer -v 1.0.0
```

## Usage

```cs
using MyDotey.Collections.Generic;

namespace MyDotey.NameCacheTest
{
    public class CascadedKeyDictionaryTest
    {
        [Fact]
        public void TestDemo()
        {
            CascadedKeyDictionary<String, String> map = new CascadedKeyDictionary<String, String>();

            map.Put("ok", "key1", "key2", "key3");
            String value = map.Get("key1", "key2", "key3");
            Assert.Equal("ok", value);

            map.Put("ok2", "key1", "key2", "key3", "key4");
            value = map.Get("key1", "key2", "key3", "key4");
            Assert.Equal("ok2", value);

            value = map.GetOrAdd(cascadedKeys => "ok3", "key1", "key2");
            Assert.Equal("ok3", value);

            map.Remove("key1", "key2");

            Assert.Equal(2, map.Count);

            foreach (CascadedKeyValuePair<String, String> pair in map)
            {
                Console.WriteLine("cascadedKeys: [ {0} ], value: {1}\n", String.Join(", ", pair.CascadedKeys), pair.Value);
            }
        }
    }
}
```