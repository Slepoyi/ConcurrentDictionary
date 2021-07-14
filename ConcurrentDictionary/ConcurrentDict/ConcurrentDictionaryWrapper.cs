using System.Collections.Generic;

namespace ConcurrentDictionary.ConcurrentDict
{
    class ConcurrentDictionaryWrapper
    {
        private readonly object writeLock = new object();

        private Dictionary<int, string> dictionary;

        public ConcurrentDictionaryWrapper()
        {
            dictionary = new Dictionary<int, string>();
        }

        public int Count()
        {
            int count = 0;

            lock (writeLock)
            {
                count = dictionary.Count;
            }

            return count;
        }

        public bool TryAdd(int key, string value)
        {
            lock (writeLock)
            {
                if (dictionary.ContainsKey(key))
                    return false;

                if (value == null)
                    return false;

                dictionary.Add(key, value);
                return true;
            }
        }

        public bool ContainsKey(int key)
        {
            return dictionary.ContainsKey(key);
        }
    }
}
