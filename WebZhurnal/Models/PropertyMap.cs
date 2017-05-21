using System;
using System.Collections;
using System.Collections.Generic;

namespace WebZhurnal.Models
{
    public class PropertyMap : IDictionary<string, int>
    {

        private Dictionary<string, int> _map;

        public PropertyMap()
        {
            _map = new Dictionary<string, int>();
        }

        public PropertyMap(IDictionary<string, int> map)
        {
            _map = new Dictionary<string, int>(map);
        }


        public int this[string key] { get => _map[key]; set => _map[key] = value; }

        public ICollection<string> Keys => _map.Keys;

        public ICollection<int> Values => _map.Values;

        public int Count => _map.Count;

        public bool IsReadOnly => false;

        public void Add(string key, int value)
        {
            if (_map.ContainsValue(value)) throw new ArgumentException($"Значение {value} уже присутствует! Дублирование не допускается.");
            if (_map.ContainsKey(key)) throw new ArgumentException($"Значение {value} уже присутствует! Дублирование не допускается.");
            _map.Add(key, value);
        }

        public void Add(KeyValuePair<string, int> item)
        {
            if (_map.ContainsValue(item.Value)) throw new ArgumentException($"Значение {item.Value} уже присутствует! Дублирование не допускается.");
            if (_map.ContainsKey(item.Key)) throw new ArgumentException($"Ключ {item.Key} уже присутствует! Дублирование не допускается.");
            _map.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _map.Clear();
        }

        public bool Contains(KeyValuePair<string, int> item)
        {
            return _map.ContainsKey(item.Key) && _map.ContainsValue(item.Value);
        }

        public bool ContainsKey(string key)
        {
            return _map.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, int>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, int> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out int value)
        {
            return _map.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            /*yield*/ return _map.GetEnumerator();
        }
    }
}