using System;
using System.Threading;

namespace ConcurrentDictionary.Concurrent
{ 
    class DictionaryReader
    {
        private int _maxNum; // = n
        private int _divisionFactor; // 2 * 5
        private int _sleepDuration;
        private Tuple<int, string> _firstElement;
        private Tuple<int, string> _secondElement;
        private DictionaryWrapper _concurrentDictionaryWrapper;
        private int _wsHappenedCounter = 0;

        public delegate void InsertHandler(int firstPairKey, int secondPairKey, int count, DictionaryWrapper concurrentDictionaryWrapper);
        public event InsertHandler InsertElement;

        public DictionaryReader(int maxNum, int divisionFactor, int sleepDuration, DictionaryWrapper concurrentDictionaryWrapper)
        {
            _maxNum = maxNum;
            _divisionFactor = 2 * divisionFactor;
            _sleepDuration = sleepDuration;
            _concurrentDictionaryWrapper = concurrentDictionaryWrapper;
        }

        public void Read()
        {
            _concurrentDictionaryWrapper.ValueWasWritten += EventCounter;

            while (true)
            {
                if (_firstElement == null || _secondElement == null)
                {
                    continue;
                }

                if (_firstElement.Item1 == _maxNum || _secondElement.Item1 == _maxNum)
                {
                    if (_wsHappenedCounter % _divisionFactor == 0)
                        InsertElement.Invoke(_firstElement.Item1, _secondElement.Item1, _concurrentDictionaryWrapper.Count(), 
                            _concurrentDictionaryWrapper);

                    break;
                }

                if (_wsHappenedCounter > 0 && _wsHappenedCounter % _divisionFactor == 0)
                {
                    InsertElement.Invoke(_firstElement.Item1, _secondElement.Item1, _concurrentDictionaryWrapper.Count(), 
                        _concurrentDictionaryWrapper);
                    _wsHappenedCounter = 0;
                }

                Thread.Sleep(_sleepDuration / 100);
            }
        }

        private void EventCounter(int key, string value)
        {
            if (value.Contains("W"))
            {
                if (_wsHappenedCounter == 0)
                {
                    _firstElement = Tuple.Create(key, value);
                }

                _secondElement = Tuple.Create(_firstElement.Item1, _firstElement.Item2);
                _firstElement = Tuple.Create(key, value);

                _wsHappenedCounter += 1;
            }
            Console.WriteLine(key + " " + value);
        }
    }
}
