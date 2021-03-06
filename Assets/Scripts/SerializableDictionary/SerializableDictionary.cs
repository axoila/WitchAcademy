using System.Collections.Generic;
 
using UnityEngine;
 
abstract public class SerializableDictionary<K, V> : ISerializationCallbackReceiver {
    [SerializeField]
    private K[] keys;
    [SerializeField]
    private V[] values;
 
    public Dictionary<K, V> dictionary = new Dictionary<K, V>();
 
    /*static public T New<T>() where T : SerializableDictionary<K, V>, new() {
        var result = new T();
        result.dictionary 
        return result;
    }*/
 
    public void OnAfterDeserialize() {
        var c = keys.Length;
        dictionary = new Dictionary<K, V>(c);
        for (int i = 0; i < c; i++) {
            dictionary[keys[i]] = values[i];
        }
        keys = null;
        values = null;
    }
 
    public void OnBeforeSerialize() {
        var c = dictionary.Count;
        keys = new K[c];
        values = new V[c];
        int i = 0;
        using (var e = dictionary.GetEnumerator())
        while (e.MoveNext()) {
            var kvp = e.Current;
            keys[i] = kvp.Key;
            values[i] = kvp.Value;
            i++;
        }
    }
}
