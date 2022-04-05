using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnumDict<T, o> : ISerializationCallbackReceiver where T : System.Enum {
    [SerializeField] [HideInInspector] private string[] keys;
    [SerializeField] [HideInInspector] private o[] values;

    private Dictionary<T, o> dictionary;

    public o this[T enumValue] {
        get {
            return dictionary[enumValue];
        }
        set {
            dictionary[enumValue] = value;
        }
    }

    public T[] Values() {
        return (T[])System.Enum.GetValues(typeof(T));
    }

    public void OnBeforeSerialize() {
        keys = dictionary.Keys.Select(x => x.ToString()).ToArray();
        values = dictionary.Keys.Select(x => dictionary[x]).ToArray();
    }

    public void OnAfterDeserialize() {
        dictionary = new Dictionary<T, o>();
        var stringMapping = new Dictionary<string, o>();
        for(int i = 0; i < keys.Length; i++) {
            stringMapping[keys[i]] = values[i];
        }
        foreach(var enumValue in System.Enum.GetValues(typeof(T))) {
            var enumName = System.Enum.GetName(typeof(T), enumValue);
            dictionary[(T)enumValue] = stringMapping.ContainsKey(enumName) ? stringMapping[enumName] : default(o);
        }
    }
}
