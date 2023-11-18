using UnityEngine;
using System.Reflection;

public class Singleton<T> : MonoBehaviour where T : Component {
    protected static T instance;
    public static bool HasInstance => instance != null;
    public static T TryGetInstance() => HasInstance ? instance : null;
    public static T Current => instance;

    public static T Instance {
        get {
            if (instance == null) {
                instance = FindFirstObjectByType<T>();
                if (instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = $"{typeof(T).Name} - AutoCreated";
                    instance = obj.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake() => InitialiseSingleton();

    protected virtual void InitialiseSingleton() {
        if (!Application.isPlaying) {
            return;
        }
        instance = this as T;
    }
}

public class PersistentSingleton<T> : Singleton<T> where T : Component {
    
    public static new T Instance {
        get {
            if (instance == null) {
                instance = FindFirstObjectByType<T>();
                if (instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = $"{typeof(T).Name} - AutoCreated";
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
            }

            return instance;
        }
    }

    protected override void InitialiseSingleton() {
        if (!Application.isPlaying) {
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this as T;
    }
}