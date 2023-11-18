using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions {
    public static IEnumerable<Enum> GetFlags(this Enum flags) {
        return Enum.GetValues(flags.GetType()).Cast<Enum>().Where(flags.HasFlag);
    }

    public static List<DictKeyValue<TKey, TValue>> ToSerialisableList<TKey, TValue>(this Dictionary<TKey, TValue> dict) {
        return new List<DictKeyValue<TKey, TValue>>(
            dict.Keys.ToList().ConvertAll((TKey key) => new DictKeyValue<TKey, TValue>(key, dict[key]))
        );
    }
     
    /// <summary>
    /// Gets, or adds if doesn't contain a component
    /// </summary>
    /// <typeparam name="T">Component Type</typeparam>
    /// <param name="gameObject">GameObject to get component from</param>
    /// <returns>Component</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
        T component = gameObject.GetComponent<T>();
        if (!component) {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }
    
    /// <summary>
    /// Returns true if a GameObject has a component of type T
    /// </summary>
    /// <typeparam name="T">Component Type</typeparam>
    /// <param name="gameObject">GameObject to check for component on</param>
    /// <returns>If the component is present</returns>
    public static bool Has<T>(this GameObject gameObject) where T : Component { 
        return gameObject.GetComponent<T>() != null; 
    }

    public static void ClampToNormalised(this Vector3 vector) {
        if (vector.magnitude > 1f) {
            vector.Normalize();
        }
    }

    public static Vector2 FromDegrees(this Vector2 _, float radians) {
        return new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
    }

    public static void FlashColour(this SpriteRenderer spriteRenderer, Color colour, float time, MonoBehaviour monoBehaviour) {        
        monoBehaviour.StartCoroutine(Flash(spriteRenderer, colour, time));
    }

    private static IEnumerator Flash(SpriteRenderer spriteRenderer, Color colour, float time) { 
        Color original = spriteRenderer.material.color;
        spriteRenderer.material.color = colour;
        yield return new WaitForSeconds(time);
        spriteRenderer.material.color = original;
    }

    [Serializable]
    public struct DictKeyValue<TKey, TValue> {
        public TKey key;
        public TValue value;

        public DictKeyValue(TKey key, TValue value) {
            this.key = key;
            this.value = value;
        }
    }
}