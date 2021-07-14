using ConcurrentDictionary.ConcurrentDict;
using System.Threading;

namespace ConcurrentDictionary.ConcurrentWrite.Classes
{
    class Reader
    {
        private int _maxNum; // = n
        private int _divisionFactor; // 2 * 5
        private int _numberOfInsertsByMain; // [n/(2*5)]
        private int _sleepDuration;

        public delegate void InsertHandler(int signalKey, ConcurrentDictionaryWrapper concurrentDictionaryWrapper);
        public event InsertHandler Notify;

        public Reader(int maxNum, int divisionFactor, int sleepDuration)
        {
            _maxNum = maxNum;
            _divisionFactor = 2 * divisionFactor;
            _sleepDuration = sleepDuration;
            _numberOfInsertsByMain = NumberOfInsertsByMain();
        }

        public void Read(ConcurrentDictionaryWrapper concurrentDictionaryWrapper)
        {
            int i = 1;

            while (true)
            {
                int signalKey = i * _divisionFactor - 1;

                if (signalKey > _maxNum + _numberOfInsertsByMain)
                    return;

                if (concurrentDictionaryWrapper.ContainsKey(signalKey))
                {
                    Notify?.Invoke(signalKey, concurrentDictionaryWrapper);

                    i += 1;
                }
                Thread.Sleep(_sleepDuration);

            }
        }

        private int NumberOfInsertsByMain()
        {
            return _maxNum / _divisionFactor;
        }
    }
}
