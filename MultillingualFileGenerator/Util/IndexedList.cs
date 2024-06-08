using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Util
{
    /// <summary>
    /// List with fast searching on string Key for performance reasons
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IndexedList<T> : IEnumerable<T>
    {
        private List<T> _list;
        private Func<T, string> _getKey;
        private Dictionary<string, T> _dictionary;

        public IndexedList(Func<T, string> getKey)
        {
            _list = new List<T>();
            _getKey = getKey;
            _dictionary = new Dictionary<string, T>();
        }

        public void Add(T item) 
        { 
            var key = _getKey(item);

            if (_dictionary.ContainsKey(key))
                throw new Exception($"List already contains key {key}, duplicate error");

            _dictionary.Add(key, item);
            _list.Add(item);
        }

        public bool ContainsKey(string key)
            => _dictionary.ContainsKey(key);

        public void Remove(string key)
        {
            if (!_dictionary.ContainsKey(key))
                throw new Exception($"Key {key} does not exist");

            var item = _dictionary[key];

            _dictionary.Remove(key);
            _list.Remove(item);
        }

        public T this[string key] => _dictionary[key];

        public IEnumerator<T> GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();
    }
}
