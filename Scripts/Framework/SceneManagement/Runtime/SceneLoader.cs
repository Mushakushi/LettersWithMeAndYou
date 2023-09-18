using System.Collections.Generic;
using Eflatun.SceneReference;
using Framework.SceneManagement.Runtime.ScriptableObjects.Channels;
using Framework.SceneManagement.Runtime.ScriptableObjects.DataContainers;
using MEC;
using Mushakushi.InternalDebug.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.SceneManagement.Runtime
{
    /// <summary>
    /// Handles scene and level loading. 
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="SceneReference"/> of the scene that displays the loading status. 
        /// </summary>
        [Tooltip("The loading scene."), SerializeField]
        private SceneReference loadingScene;
        
        /// <summary>
        /// The <see cref="SceneEventChannel"/>.
        /// </summary>
        [Header("Listening to Channel"), Tooltip("The SceneLoadEventChannel"), SerializeField]
        private SceneEventChannel sceneEventChannel;

        /// <summary>
        /// The <see cref="SceneData"/> to load.
        /// </summary>
        private SceneData targetSceneData;

        /// <summary>
        /// The current <see cref="SceneData"/>.
        /// </summary>
        private SceneData currentSceneData;

        /// <summary>
        /// How many async operations are currently running.  
        /// </summary>
        private int activeAsyncOperations; 

        private void OnEnable()
        {
            sceneEventChannel.OnLoadRequested += CacheSceneData;
            sceneEventChannel.OnLoadingScreenDisplayed += LoadSceneContextOnly;
            sceneEventChannel.OnRestartRequested += LoadLoadingScene;
        }

        private void OnDisable()
        {
            sceneEventChannel.OnLoadRequested -= CacheSceneData;
            sceneEventChannel.OnLoadingScreenDisplayed -= LoadSceneContextOnly;
            sceneEventChannel.OnRestartRequested -= LoadLoadingScene;
        }

        /// <summary>
        /// Saves the <see cref="targetSceneData"/> and shows a loading screen if <see cref="SceneData.ShowLoadingScreen"/>. 
        /// </summary>
        /// <param name="sceneData">The <see cref="SceneData"/>.</param>
        private void CacheSceneData(SceneData sceneData)
        {
            if (sceneData.ShowLoadingScreen) LoadLoadingScene();
            else sceneEventChannel.RaiseOnLoadingScreenShown();
            targetSceneData = sceneData;
        }

        /// <summary>
        /// Loads the loading scene. 
        /// </summary>
        private void LoadLoadingScene()
        {
            SceneManager.LoadSceneAsync(loadingScene.BuildIndex, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Loads and initializes the <see cref="targetSceneData"/>. 
        /// </summary>
        private void LoadSceneContextOnly()
        {
            // Resources.UnloadUnusedAssets(); // todo - move to Addressable system
            activeAsyncOperations = 0;
            if (currentSceneData != null && currentSceneData.SceneReference.IsSafeToUse)
            {
                RunCoroutine(_UnloadSceneAsync(currentSceneData.SceneReference.BuildIndex));
            }
            else
            {
                InternalDebug.LogWarning("Current scene context not found. " +
                                 $"Unloading all other scenes other than {gameObject.scene.name} and {loadingScene.Name} (if it was loaded).");
                var scenes = SceneManager.sceneCount;
                for (var i = 0; i < scenes; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    if (scene.name == gameObject.scene.name || scene.name == loadingScene.Name) continue;
                    RunCoroutine(_UnloadSceneAsync(scene.buildIndex));
                }
            }
            
            RunCoroutine(_LoadSceneAsync(targetSceneData));
            currentSceneData = targetSceneData;
        }

        /// <summary>
        /// Wrapper class that runs coroutine through the <see cref="_RunCoroutineHandler"/>. 
        /// </summary>
        /// <param name="coroutine"></param>
        private void RunCoroutine(IEnumerator<float> coroutine)
        {
            Timing.RunCoroutine(_RunCoroutineHandler(coroutine)); 
        }

        /// <summary>
        /// Runs a coroutine while tracking how much <see cref="activeAsyncOperations"/> there are. 
        /// </summary>
        /// <param name="coroutine">The <see cref="IEnumerator{T}"/> to run.</param>
        /// <returns></returns>
        private IEnumerator<float> _RunCoroutineHandler(IEnumerator<float> coroutine)
        {
            activeAsyncOperations++;
            yield return Timing.WaitForOneFrame; // to prevent extreme cases where loading takes one frame 
            yield return Timing.WaitUntilDone(Timing.RunCoroutine(coroutine));
            if (--activeAsyncOperations == 0) sceneEventChannel.RaiseOnAsyncOperationsCompleted();
        }

        /// <summary>
        /// Loads a scene. 
        /// </summary>
        /// <param name="sceneData">The <see cref="SceneData"/> of the loading scene.</param>
        private IEnumerator<float> _LoadSceneAsync(SceneData sceneData)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneData.SceneReference.BuildIndex, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                sceneEventChannel.RaiseOnLoadProgress(asyncOperation.progress);
                yield return Timing.WaitForOneFrame;
            }
        }

        /// <summary>
        /// Unloads a scene. 
        /// </summary>
        /// <param name="buildIndex">The build index of the scene to unload.</param>
        private static IEnumerator<float> _UnloadSceneAsync(int buildIndex)
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(buildIndex);
            while (!asyncOperation.isDone)
            {
                yield return Timing.WaitForOneFrame;
            }
        }
    }
}