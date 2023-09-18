using System.Collections.Generic;
using Eflatun.SceneReference;
using Framework.SceneManagement.Runtime.ScriptableObjects.DataContainers;
using MEC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Initialization.Runtime
{
    /// <summary>
    /// Scene whose only purpose is to load the persistent managers scene. 
    /// </summary>
    public class Initialization : InitializationBase
    {
        /// <summary>
        /// The title screen <see cref="SceneData"/>.
        /// </summary>
        [Tooltip("The title screen data."), SerializeField]
        private SceneData titleScreen;

        private void Start()
        {
            LoadPersistentManagersScene(() => SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex));
            SceneManager.LoadSceneAsync(titleScreen.SceneReference.BuildIndex, LoadSceneMode.Additive);
        }
    }
}
