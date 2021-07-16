using System.Collections.Generic;

namespace ConcurrentDictionary.Concurrent
{
    class DictionaryWrapper
    {
        private readonly object _writeLock = new object();

        private Dictionary<int, string> _dictionary;

        public delegate void WriteHandler(int key, string value);
        public event WriteHandler ValueWasWritten;

        public DictionaryWrapper()
        {
            _dictionary = new Dictionary<int, string>();
        }

        public int Count()
        {
            int count = 0;

            lock (_writeLock)
            {
                count = _dictionary.Count;
            }

            return count;
        }

        public bool TryAdd(int key, string value)
        {
            lock (_writeLock)
            {
                if (_dictionary.ContainsKey(key))
                    return false;

                if (value == null)
                    return false;

                _dictionary.Add(key, value);
                ValueWasWritten.Invoke(key, value);

                return true;
            }
        }

        public Dictionary<int, string> GetDictionary()
        {
            return _dictionary;
        }
    }
}
