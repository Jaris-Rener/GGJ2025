using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void ClearObjects<T>(this List<T> list) where T : Object
    {
        for (var i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            Object.Destroy(item);
        }
        
        list.Clear();
    }
}