using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using URnd = UnityEngine.Random;

namespace Utility
{
    [Serializable]
    public class Singleton<T> where T : MonoBehaviour
    {
        private T instance;

        public T I
        {
            get => instance;
            set
            {
                if (instance == null) instance = value;
                else if (instance != value) Debug.Log("Error");
            }
        }
        
        public static implicit operator T(Singleton<T> singleton) => singleton.instance;
        public static implicit operator bool(Singleton<T> singleton) => singleton.instance != null;
    }

    public static class ComponentUtil
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component => go.GetComponent<T>() ?? go.AddComponent<T>();
    }

    public static class ListUtil
    {
        public static void Shuffle(this IList list)
        {
            var n = list.Count;
            while (n > 1) {
                n--;
                var k = URnd.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}