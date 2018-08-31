//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//
//[Serializable]
// public class BetterDictionary<TKey, TValue> : IDictionary<TKey, TValue>
// {
//     [SerializeField]
//     protected List<TKey> keys;
// 
//     [SerializeField]
//     protected List<TValue> values;
// 
//     public List<TKey> Keys { get { return keys; } }
// 
//     public List<TValue> Values { get { return values; } }
// 
//     public int Count { get { return keys.Count; } }
// 
//     public BetterDictionary() : this(new List<TKey>(), new List<TValue>())
//     {}
// 
//     public BetterDictionary(List<TKey> keys, List<TValue> values) {
//         this.keys = keys; 
//         this.values = values;
//     }
// 
//     public TValue this[TKey key] {
//         get {
//             int index;
//             if (!TryGetIndex(key, out index)) {
//                 throw new KeyNotFoundException(key.ToString());
//             }
//             return values[index];
//         }
//         set {
//             int index;
//             if (!TryGetIndex(key, out index))
//                 Add(key, value);
//             else values[index] = value;
//         }
//     }
// 
//     public void SetKeyAt(int i, TKey value)
//     {
//         AssertIndexInBounds(i);
//         if (value != null && !value.Equals(keys[i]))
//             AssertUniqueKey(value);
//         keys[i] = value;
//     }
// 
//     public TKey GetKeyAt(int i)
//     {
//         AssertIndexInBounds(i);
//         return keys[i];
//     }
// 
//     public void SetValueAt(int i, TValue value)
//     {
//         AssertIndexInBounds(i);
//         values[i] = value;
//     }
// 
//     public TValue GetValueAt(int i)
//     {
//         AssertIndexInBounds(i);
//         return values[i];
//     }
// 
//     public KeyValuePair<TKey, TValue> GetPairAt(int i)
//     {
//         AssertIndexInBounds(i);
//         return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
//     }
// 
//     private void AssertIndexInBounds(int i)
//     {
//         if (!keys.InBounds(i))
//             throw new IndexOutOfRangeException("i");
//     }
// 
//     public void Clear()
//     {
//         keys.Clear();
//         values.Clear();
//     }
// 
//     public void Insert(int i, TKey key, TValue value)
//     {
//         AssertUniqueKey(key);
//         if (key == null)
//            Debug.LogError(key + " - Dictionary key cannot be null");
//         keys.Insert(i, key);
//         values.Insert(i, value);
//     }
// 
//     private void AssertUniqueKey(TKey key)
//     {
//         if (ContainsKey(key))
//             throw new ArgumentException(string.Format("There's already a key `{0}` defined in the dictionary", key.ToString()));
//     }
// 
//     public void Insert(TKey key, TValue value)
//     {
//         Insert(0, key, value);
//     }
// 
//     public void Add(TKey key, TValue value)
//     {
//         Insert(Count, key, value);
//     }
// 
//     public bool Remove(TKey key)
//     {
//         int index;
//         if (TryGetIndex(key, out index)) {
//             keys.RemoveAt(index);
//             values.RemoveAt(index);
//             return true;
//         }
//         return false;
//     }
// 
//     public void RemoveAt(int i)
//     {
//         AssertIndexInBounds(i);
//         keys.RemoveAt(i);
//         values.RemoveAt(i);
//     }
// 
//     public void RemoveLast()
//     {
//         RemoveAt(Count - 1);
//     }
// 
//     public void RemoveFirst()
//     {
//         RemoveAt(0);
//     }
// 
//     public bool TryGetValue(TKey key, out TValue result) 
//	 {
//         int index;
//         if (!TryGetIndex(key, out index)) {
//             result = default(TValue);
//             return false;
//         }
//         result = values[index];
//         return true;
//     }
// 
//     public bool ContainsValue(TValue value) 
//	 {
//         return values.Contains(value);
//     }
// 
//     public bool ContainsKey(TKey key) 
//	 {
//         return keys.Contains(key);
//     }
// 
//     private bool TryGetIndex(TKey key, out int index) 
//	 {
//         return (index = keys.IndexOf(key)) != -1;
//     }
// 
//     public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() 
//	 {
//         for (int i = 0; i < Count; i++)
//             yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
//     }
// 
//     IEnumerator IEnumerable.GetEnumerator() 
//	 {
//         return GetEnumerator();
//     }
// 
//     ICollection<TKey> IDictionary<TKey, TValue>.Keys {
//         get { return keys; }
//     }
// 
//     bool IDictionary<TKey, TValue>.Remove(TKey key) 
//	 {
//         return Remove(key);
//     }
// 
//     ICollection<TValue> IDictionary<TKey, TValue>.Values {
//         get { return values; }
//     }
// 
//     public void Add(KeyValuePair<TKey, TValue> item) 
//	 {
//         keys.Add(item.Key);
//         values.Add(item.Value);
//     }
// 
//     public bool Contains(KeyValuePair<TKey, TValue> item) 
//	 {
//         return ContainsKey(item.Key);
//     }
// 
//     public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) 
//	 {
//         for (int i = arrayIndex; i < array.Length; i++)
//             array[i] = new KeyValuePair<TKey, TValue>(keys[i], values[i]);
//     }
// 
//     public bool IsReadOnly {
//         get { return false; }
//     }
// 
//     public bool Remove(KeyValuePair<TKey, TValue> item) 
//	 {
//         return Remove(item.Key);
//     }
// }
// 
// public static class BetterDictionaryExtensions {
//     public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this BetterDictionary<TKey, TValue> d) 
//	 {
//         return CreateDictionary(d.Keys, d.Values);
//     }
// 
//     public static BetterDictionary<TKey, TValue> ToKVPList<TKey, TValue>(this IDictionary<TKey, TValue> d) 
//	 {
//         return new BetterDictionary<TKey, TValue>(d.Keys.ToList(), d.Values.ToList());
//     }
//
//    /// <summary> Creates a dictionary out of the passed keys and values </summary>
//    public static Dictionary<T, U> CreateDictionary<T, U>(IEnumerable<T> keys, IList<U> values) 
//	{
//        return keys.Select((k, i) => new { k, v = values[i] }).ToDictionary(x => x.k, x => x.v);
//    }
//
//    public static bool InBounds<T>(this IList<T> list, int i)
//    {
//        return i > -1 && i < list.Count;
//    }
// }
//
//
