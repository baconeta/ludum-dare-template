using System;
using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// <summary>
        /// Singleton pattern
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject newInstance = new();
                        _instance = newInstance.AddComponent<T>();
                        _instance.name = typeof(T).ToString();
                        Debug.LogWarning(
                            $"Static Instance was not found for {_instance.name} - A new object has been created.");
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}