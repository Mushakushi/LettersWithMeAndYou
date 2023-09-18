using System.Collections.Generic;
using Framework.SceneManagement.Runtime.ScriptableObjects.Channels;
using MEC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.SceneManagement.Runtime
{
    /// <summary>
    /// Shows loading screen on scene load.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreenController: MonoBehaviour
    {
        /// <summary>
        /// The amount of steps it should take to fully fade in. 
        /// </summary>
        [SerializeField, Min(1)] private int fadeInSteps;
        
        /// <summary>
        /// The amount of steps it should take to fully fade out. 
        /// </summary>
        [SerializeField, Min(1)] private int fadeOutSteps;

        /// <summary>
        /// Whether or not the loading screen is fading in. 
        /// </summary>
        private bool isFadingIn;

        /// <summary>
        /// The camera rendering this scene. 
        /// </summary>
        [SerializeField] private Camera sceneCamera; 
        
        [SerializeField] private SceneEventChannel sceneEventChannel;
        
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            sceneCamera.enabled = false; 
        }

        private void Start()
        {
            Timing.RunCoroutine(_Fade(true));
        }

        private void OnEnable()
        {
            sceneEventChannel.OnSceneReady += FadeOut;
        }

        private void OnDisable()
        {
            sceneEventChannel.OnSceneReady -= FadeOut;
        }

        /// <summary>
        /// Fades in the scene if fadeIn is true, fades out and unloads otherwise.
        /// </summary>
        private IEnumerator<float> _Fade(bool fadeIn)
        {
            while (isFadingIn) yield return Timing.WaitForOneFrame;
            if (!fadeIn)
            {
                Destroy(sceneCamera.gameObject);
                OnDisable();   
            }
            isFadingIn = fadeIn;
            
            var targetAlpha = fadeIn ? 1 : 0;
            var steps = fadeIn ? fadeInSteps : fadeOutSteps;
            for (var i = 0; i < steps; i++)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, (float)(i + 1) / steps);
                yield return Timing.WaitForOneFrame;
            }

            if (isFadingIn)
            {
                sceneCamera.enabled = true;
                isFadingIn = false;
                sceneEventChannel.RaiseOnLoadingScreenShown();
            }
            else SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
        }

        private void FadeOut()
        {
            Timing.RunCoroutine(_Fade(false));
        }
    }
}