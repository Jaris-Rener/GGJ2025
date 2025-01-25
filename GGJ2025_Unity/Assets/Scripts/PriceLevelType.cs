using System.Collections.Generic;
using UnityEngine;

public static class IntToFloatMapper
{
    private static Dictionary<int, float> intToFloatMap;

    public static float GetMultiplier(int marketForce)
    {
        return intToFloatMap[marketForce];
    }
    
    static IntToFloatMapper()
    {
        // Initialize the map
        intToFloatMap = new Dictionary<int, float>
        {
            { -2, 0.5f },
            { -1, 0.75f },
            { 0, 1.0f },
            { 1, 1.5f },
            { 2, 2.0f }
        };

        // Example usage
        int input = -1;
        if (intToFloatMap.TryGetValue(input, out float mappedValue))
        {
            Debug.Log($"Mapped {input} to {mappedValue}");
        }
        else
        {
            Debug.Log($"No mapping found for {input}");
        }
    }
}