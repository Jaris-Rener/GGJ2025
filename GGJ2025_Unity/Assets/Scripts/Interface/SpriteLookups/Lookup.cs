using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Lookup<TKey, TValue> : ScriptableObject
{
    [SerializeField] private List<SpriteLookupEntry> _lookup = new();
    
    public TValue Get(TKey key)
    {
        var entry = _lookup.Find(x => Compare(x.Key, key));
        return entry.Value;
    }
    
    public bool Compare<T>(T x, T y)
    {
        return EqualityComparer<T>.Default.Equals(x, y);
    }
    
    [Serializable]
    private struct SpriteLookupEntry
    {
        public TKey Key;
        public TValue Value;
    }
}