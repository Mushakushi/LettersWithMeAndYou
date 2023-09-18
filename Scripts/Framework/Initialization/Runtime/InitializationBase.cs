using System;
using System.Collections.Generic;
using Eflatun.SceneReference;
using MEC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Initialization.Runtime
{
    /// <summary>
    /// Class that provides functions common to all initializers. 
    /// </summary>
    public class InitializationBase : MonoBehaviour
    {
        /// <summary>
        /// The persistent managers scene. 
        /// </summary>
        [field: Header("Scene References"), Tooltip("The persistent managers scene."), SerializeField]
        protected SceneReference PersistentManagersScene { get; private set; }

        /// <summary>
        /// Loads the <see cref="PersistentManagersScene"/>, then performs a callback. 
        /// </summary>
        protected void LoadPersistentManagersScene(Action callback)
        {
            // todo - loading game at different times because async
            Timing.RunCoroutine(_LoadPersistentManagersScene(callback));
        }

        private IEnumerator<float> _LoadPersistentManagersScene(Action callback)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(PersistentManagersScene.BuildIndex, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                yield return Timing.WaitForOneFrame;
            }
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(PersistentManagersScene.BuildIndex));
            callback();
        }
    }
}