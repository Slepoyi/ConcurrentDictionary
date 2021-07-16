using ConcurrentDictionary.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcurrentDictionary.Validation
{
    class DictionaryValidator
    {
        int _msInserts = 0;

        public bool ValidateDictionary(DictionaryWrapper _concurrentDictionaryWrapper, int n, int divisionFactor)
        {
            bool validated = true;

            Dictionary<int, string> dictionary = _concurrentDictionaryWrapper.GetDictionary();

            Dictionary<int, int> keyValuesFrom1ToN = new Dictionary<int, int>();
            for (int i = 0; i < n + 1; i++)
                keyValuesFrom1ToN.Add(i, 0);

            foreach (KeyValuePair<int, string> kvp in dictionary)
            {
                if (kvp.Key < n + 1)
                    keyValuesFrom1ToN[kvp.Key] = 1;

                validated = OddEvenCheck("WS1", 1, kvp) && OddEvenCheck("WS2", 0, kvp) && MainStreamInsertCheck(divisionFactor, kvp);

                if (!validated)
                    break;

                Console.WriteLine(string.Format(@"Key: {0}; value: {1}", kvp.Key, kvp.Value));
            }

            if (keyValuesFrom1ToN.Values.Where(v => v == 0).Count() > 0)
                validated = false;

            return validated;
        }

        private int GetVolumeBeforeInsert(KeyValuePair<int, string> kvp)
        {
            string[] msValue = kvp.Value.Split(' ');
            int firstElemKey = Int32.Parse(msValue[2]);
            int secondElemKey = Int32.Parse(msValue[4]);

            return kvp.Key - firstElemKey - secondElemKey;
        }

        private bool OddEvenCheck(string wsName, int divisionParam, KeyValuePair<int, string> kvp)
        {
            if (kvp.Value.Contains(wsName))
            {
                if (kvp.Key % 2 != divisionParam)
                {
                    return false;
                }
            }

            return true;
        }

        private bool MainStreamInsertCheck(int divisionFactor, KeyValuePair<int, string> kvp)
        {
            if (kvp.Value.Contains("M"))
            {
                int volumeBeforeInsert = (_msInserts + 1) * 2 * divisionFactor + _msInserts;

                if (volumeBeforeInsert != GetVolumeBeforeInsert(kvp))
                {
                    return false;
                }

                _msInserts += 1;
            }

            return true;
        }
    }
}
