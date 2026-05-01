using UnityEngine;

namespace Utils
{
    public class EverlastingSingleton<T> : MonoBehaviour where T : Component
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
                    Debug.LogWarning($"[EverlastingSingleton] Instance of {typeof(T)} requested during application quit — returning null.");
                    return null;
                }

                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();
                    if (_instance == null)
                    {
                        GameObject newInstance = new();
                        _instance = newInstance.AddComponent<T>();
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
            // incorrectly treat ourselves as a duplicate and Destroy this component.
            if (_instance == null || _instance == this)
            {
                _instance = this as T;
                // Must pass gameObject; passing a component on a parented object logs a Unity warning.
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
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
