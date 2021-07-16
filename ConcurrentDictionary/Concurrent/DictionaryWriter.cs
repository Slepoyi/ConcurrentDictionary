using System;
using System.Threading;

namespace ConcurrentDictionary.Concurrent
{
    class DictionaryWriter
    {
        private int _sleepDuration;
        private int _maxNum; // = n
        private int _writeParam; // 0 - even, 1 - odd
        private string _name;
        private DictionaryWrapper _concurrentDictionaryWrapper;

        public DictionaryWriter(int maxNum, int writeParam, string name, int sleepDuration, DictionaryWrapper concurrentDictionaryWrapper)
        {
            _maxNum = maxNum;
            _writeParam = writeParam;
            _name = name;
            _sleepDuration = sleepDuration;
            _concurrentDictionaryWrapper = concurrentDictionaryWrapper;
        }

        public void ConcurrentWriteToDict()
        {
            for (int k = 0, i = 2 * k + _writeParam; i <= _maxNum; k++, i = 2 * k + _writeParam)
            {
                if (_concurrentDictionaryWrapper.TryAdd(i, string.Format(@"${0} - {1}", _name, DateTime.Now)))
                    Thread.Sleep(_sleepDuration);
            }
        }
    }
}
