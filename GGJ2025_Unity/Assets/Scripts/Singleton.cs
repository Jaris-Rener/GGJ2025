using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else
        {
            Debug.LogWarning($"Existing instance of {typeof(T)} found, destroying this.");
            Destroy(gameObject);
        }
    }
}