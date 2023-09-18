// modified from: https://github.com/UnityTechnologies/InputSystem_Warriors/blob/master/InputSystem_Warriors_Project/Assets/Scripts/Utilities/Singleton.cs

using UnityEngine;

namespace Mushakushi.Singletons.Runtime
{
    /// <summary>
    /// A generic MonoBehaviour Singleton.
    /// </summary>
    /// <remarks>
    /// Use ScriptableObjects instead wherever possible.
    /// Be aware this will not prevent a non singleton constructor such as <code>T myT = new T();</code>
    /// To prevent that, add <code>protected T () {}</code> to your singleton class.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
    {
        /// <summary>
        /// Whether or not the application is quitting. 
        /// </summary>
        /// <seealso cref="Application.quitting"/>
        // ReSharper disable once StaticMemberInGenericType
        private static bool applicationIsQuitting;

        /// <summary>
        /// Prevents multi-access to this instance. 
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new Object();
        
        /// <summary>
        /// The singleton instance of type <see cref="T"/>. 
        /// </summary>
        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning($"Singleton instance of type {typeof(T)} was already destroyed on application quit. Returning null.");
                    return null;
                }

                lock (Lock)
                {
                    if (instance != null) return instance;
                    instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError($"More than one singleton of instance {typeof(T)} was found. Try reopening the scene.");
                        return instance;
                    }

                    if (instance == null)
                    {
                        var _ = new GameObject($"[Singleton] {typeof(T)}");
                        instance.gameObject.AddComponent<T>();

                        Debug.Log($"Creating singleton instance of type {typeof(T)}");
                    }
                    else
                    {
                        Debug.LogWarning($"Singleton instance of type {typeof(T)} already exists.");
                    }
                }

                return instance;
            }
        }
        private static T instance;
        
        /// <summary>
        /// Whether or not this <see cref="MonoBehaviour"/> is marked as do not destroy. 
        /// </summary>
        private static bool IsDontDestroyOnLoad
        {
            get
            {
                if (instance == null) return false;
                
                // Object exists independent of Scene lifecycle, assume that means it has DontDestroyOnLoad set
                return (instance.gameObject.hideFlags & HideFlags.DontSave) == HideFlags.DontSave; 
            }
        }

        /// <summary>
        /// Removes access to this <see cref="instance"/> on destroy. 
        /// </summary>
        /// <remarks>
        /// When Unity quits, it destroys objects in a random order. Scripts that access this <see cref="Instance"/> after its destruction,
        /// will create a buggy ghost object that will remain in the Editor even after the application has quit. 
        /// </remarks>
        public void OnDestroy()
        {
            if (IsDontDestroyOnLoad) applicationIsQuitting = true; 
        }
    }
}