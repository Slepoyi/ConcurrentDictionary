using ConcurrentDictionary.ConcurrentDict;
using System;
using System.Threading;

namespace ConcurrentDictionary.ConcurrentWrite.Classes
{
    class ConcurrentWriter
    {
        private int _sleepDuration;
        private int _maxNum; // = n
        private int _writeParam; // 0 - even, 1 - odd
        private string _name;

        public ConcurrentWriter(int maxNum, int writeParam, string name, int sleepDuration)
        {
            _maxNum = maxNum;
            _writeParam = writeParam;
            _name = name;
            _sleepDuration = sleepDuration;
        }

        public void ConcurrentWriteToDict(ConcurrentDictionaryWrapper concurrentDictionaryWrapper)
        {
            for (int k = 0, i = 2 * k + _writeParam; i <= _maxNum; k++, i = 2 * k + _writeParam)
            {
                if (concurrentDictionaryWrapper.TryAdd(i, string.Format(@"${0} - {1}", _name, DateTime.Now)))
                    Thread.Sleep(_sleepDuration);
            }
        }
    }
}
