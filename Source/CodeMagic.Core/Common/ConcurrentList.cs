using System.Collections;
using System.Collections.Generic;

namespace CodeMagic.Core.Common
{
    public class ConcurrentList<T> : IList<T>
    {
        private readonly List<T> impl;

        public ConcurrentList()
        {
            impl = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (impl)
            {
                return impl.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            lock (impl)
            {
                impl.Add(item);
            }
        }

        public void Clear()
        {
            lock (impl)
            {
                impl.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (impl)
            {
                return impl.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (impl)
            {
                impl.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(T item)
        {
            lock (impl)
            {
                return impl.Remove(item);
            }
        }

        public int Count
        {
            get
            {
                lock (impl)
                {
                    return impl.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            lock (impl)
            {
                return impl.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (impl)
            {
                impl.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (impl)
            {
                impl.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                lock (impl)
                {
                    return impl[index];
                }
            }
            set
            {
                lock (impl)
                {
                    impl[index] = value;
                }
            }
        }
    }
}