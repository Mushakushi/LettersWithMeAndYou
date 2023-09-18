using Framework.SceneManagement.Runtime.ScriptableObjects.Channels;
using UnityEngine;

namespace Framework.Initialization.Runtime
{
    /// <summary>
    /// Script that allows the game to be started from any scene. 
    /// </summary>
    public class EditorColdStartup 
#if UNITY_EDITOR
        : InitializationBase
#endif
    {
#if UNITY_EDITOR
        /// <summary>
        /// The <see cref="SceneEventChannel"/>.
        /// </summary>
        [Header("Listening to Channel"), Tooltip("The SceneLoadEventChannel"), SerializeField]
        private SceneEventChannel sceneEventChannel;

        private void Start()
        {
            if (PersistentManagersScene.LoadedScene.isLoaded) return;
            LoadPersistentManagersScene(sceneEventChannel.RaiseOnAsyncOperationsCompleted);
        }
#endif
    }
}