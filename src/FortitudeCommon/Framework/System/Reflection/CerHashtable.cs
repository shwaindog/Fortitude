using System.Diagnostics;

namespace FortitudeCommon.Framework.System.Reflection
{
    // Reliable hashtable thread safe for multiple readers and single writer. Note that the reliability goes together with thread
    // safety. Thread safety for multiple readers requires atomic update of the state that also makes the table
    // reliable in the presence of asynchronous exceptions.
    internal struct CerHashtable<K, V> where K : class
    {
        private sealed class Table
        {
            // Note that m_keys and m_values arrays are immutable to allow lock-free reads. A new instance
            // of CerHashtable has to be allocated to grow the size of the hashtable.
            internal K[] m_keys;
            internal V[] m_values;
            internal int m_count;

            internal Table(int size)
            {
                size = GetPrime(size);
                m_keys = new K[size];
                m_values = new V[size];
            }

            internal void Insert(K key, V value)
            {
                int hashcode = GetHashCodeHelper(key);
                if (hashcode < 0)
                    hashcode = ~hashcode;

                K[] keys = m_keys;
                int index = hashcode % keys.Length;

                while (true)
                {
                    K hit = keys[index];

                    if (hit == null)
                    {
                        m_count++;
                        m_values[index] = value;

                        // This volatile write has to be last. It is going to publish the result atomically.
                        //
                        // Note that incrementing the count or setting the value does not do any harm without setting the key. The inconsistency will be ignored
                        // and it will go away completely during next rehash.
                        Volatile.Write(ref keys[index], key);

                        break;
                    }
                    else
                    {
                        Debug.Assert(!hit.Equals(key), "Key was already in CerHashtable!  Potential race condition (or bug) in the Reflection cache?");

                        index++;
                        if (index >= keys.Length)
                            index -= keys.Length;
                    }
                }
            }

            public const int HashPrime = 101;

            private static ReadOnlySpan<int> Primes => new int[]
            {
                3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
                1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
                17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
                187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
                1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
            };
            

            public static int GetPrime(int min)
            {
                if (min < 0)
                    throw new ArgumentException("Capacity overflow");

                foreach (int prime in Primes)
                {
                    if (prime >= min)
                        return prime;
                }

                // Outside of our predefined table. Compute the hard way.
                for (int i = (min | 1); i < int.MaxValue; i += 2)
                {
                    if (IsPrime(i) && ((i - 1) % HashPrime != 0))
                        return i;
                }
                return min;
            }
            

            public static bool IsPrime(int candidate)
            {
                if ((candidate & 1) != 0)
                {
                    int limit = (int)Math.Sqrt(candidate);
                    for (int divisor = 3; divisor <= limit; divisor += 2)
                    {
                        if ((candidate % divisor) == 0)
                            return false;
                    }
                    return true;
                }
                return candidate == 2;
            }
        }

        private Table m_Table;

        private const int MinSize = 7;

        private static int GetHashCodeHelper(K key)
        {
            // For strings we don't want the key to differ across domains as CerHashtable might be shared.
            return key.GetHashCode();
        }

        private void Rehash(int newSize)
        {
            Table newTable = new Table(newSize);

            Table oldTable = m_Table;
            if (oldTable != null)
            {
                K[] keys = oldTable.m_keys;
                V[] values = oldTable.m_values;

                for (int i = 0; i < keys.Length; i++)
                {
                    K key = keys[i];

                    if (key != null)
                    {
                        newTable.Insert(key, values[i]);
                    }
                }
            }

            // Publish the new table atomically
            Volatile.Write(ref m_Table, newTable);
        }

        internal V this[K key]
        {
            get
            {
                Table table = Volatile.Read(ref m_Table);
                if (table == null)
                    return default!;

                int hashcode = GetHashCodeHelper(key);
                if (hashcode < 0)
                    hashcode = ~hashcode;

                K[] keys = table.m_keys;
                int index = hashcode % keys.Length;

                while (true)
                {
                    // This volatile read has to be first. It is reading the atomically published result.
                    K hit = Volatile.Read(ref keys[index]);

                    if (hit != null)
                    {
                        if (hit.Equals(key))
                            return table.m_values[index];

                        index++;
                        if (index >= keys.Length)
                            index -= keys.Length;
                    }
                    else
                    {
                        return default!;
                    }
                }
            }
            set
            {
                Table table = m_Table;

                if (table != null)
                {
                    int requiredSize = 2 * (table.m_count + 1);
                    if (requiredSize >= table.m_keys.Length)
                        Rehash(requiredSize);
                }
                else
                {
                    Rehash(MinSize);
                }

                m_Table.Insert(key, value);
            }
        }
    }
}
