namespace Utils
{
    public static class UnityHelper
    {
#if UNITY_EDITOR
        public static bool IsInUnityEditor = true;
        public static bool IsNotInUnityEditor = !IsInUnityEditor;
#else
        public static bool IsInUnityEditor = false;
        public static bool IsNotInUnityEditor = !IsInUnityEditor;
#endif
#if DEBUG
        public static bool IsInDebug = true;
#else
        public static bool IsInDebug = false;
#endif
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        public static bool DoEnableCloudServicesAnalytics = true;
#else
        public static bool DoEnableCloudServicesAnalytics = false;
#endif
/// Mobile checks.
#if UNITY_ANDROID
        public static bool IsAndroid = true;
#else
        public static bool IsAndroid = false;
#endif
#if UNITY_IOS
        public static bool IsIOS = true;
#else
        public static bool IsIOS = false;
#endif
        public static bool IsMobile = IsAndroid || IsIOS;
// Other platform checks.
#if UNITY_WEBGL
        public static bool IsWebGL = true;
#else
        public static bool IsWebGL = false;
#endif
    }
}