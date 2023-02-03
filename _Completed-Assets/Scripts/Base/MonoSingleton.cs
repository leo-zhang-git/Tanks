using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("instance is null");
                }
                return instance;
            }
        }
        protected MonoSingleton()
        {

        }
        protected virtual void Awake()
        {
            instance = this as T;
        }
    }
}
