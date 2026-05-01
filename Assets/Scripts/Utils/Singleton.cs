using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        // ReSharper disable once InconsistentNaming
        protected static T _instance;

        // Guards against creating a new instance while the app is shutting down.
        private static bool _applicationIsQuitting;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance of {typeof(T)} requested during application quit — returning null.");
                    return null;
                }

                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();
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

            // _instance == this handles the case where the Instance getter ran FindAnyObjectByType
            // before this Awake fired and pre-assigned _instance to us; without this check we would
            // incorrectly treat ourselves as a duplicate and Destroy our own GameObject.
            if (_instance == null || _instance == this)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }
    }
}
