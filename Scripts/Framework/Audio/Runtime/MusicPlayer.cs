using System.Collections.Generic;
using Framework.Audio.Runtime.ScriptableObjects.Channels;
using MEC;
using Mushakushi.Attributes.Runtime;
using UnityEngine;

namespace Framework.Audio.Runtime
{
    public class MusicPlayer: MonoBehaviour
    {
        /// <summary>
        /// The <see cref="AudioEventChannel"/> that controls music playback. 
        /// </summary>
        [Header("Configuration"), SerializeField] private AudioEventChannel musicEventChannel;

        /// <summary>
        /// Steps over which to perform the crossfade. 
        /// </summary>
        [SerializeField, Min(1)] private int crossfadeSteps; 
        
        /// <summary>
        /// The <see cref="AudioSource"/> that is playing the current music. 
        /// </summary>
        [Header("Music"), ReadOnly, SerializeField] private AudioSource[] audioSources = new AudioSource[2];

        /// <summary>
        /// Whether or not the first audio source is actively playing the current music. 
        /// </summary>
        [ReadOnly, SerializeField] private bool initialAudioSourceIsActive;

        /// <summary>
        /// The actively playing <see cref="_CrossfadePlay"/> coroutine. 
        /// </summary>
        private CoroutineHandle activeCoroutine; 
        
        private void Awake()
        {
            audioSources[0] = gameObject.AddComponent<AudioSource>();
            audioSources[1] = gameObject.AddComponent<AudioSource>();
        }

        private void OnEnable()
        {
            musicEventChannel.OnAudioClipPlayRequested += CrossfadePlay;
            // musicEventChannel.OnPlaybackStopRequested += StopPlayback;
        }

        private void OnDisable()
        {
            musicEventChannel.OnAudioClipPlayRequested -= CrossfadePlay;
            // musicEventChannel.OnPlaybackStopRequested -= StopPlayback;
        }

        private void CrossfadePlay(AudioClip audioClip)
        {
            Debug.Log("huh");
            var fadingOutAudioSourceIndex = initialAudioSourceIsActive ? 0 : 1;
            if (activeCoroutine.IsValid) Timing.KillCoroutines(activeCoroutine);
            activeCoroutine = Timing.RunCoroutine(
                _CrossfadePlay(audioClip, audioSources[1 - fadingOutAudioSourceIndex], audioSources[fadingOutAudioSourceIndex]), 
                Segment.Update
            ); 
        }

        /// <summary>
        /// Cross-fades two <see cref="AudioClip"/> components.  
        /// </summary>
        /// <param name="audioClip">The audio clip to play on the fading in audio source.</param>
        /// <param name="fadingIn">The audio source that is fading in.</param>
        /// <param name="fadingOut">The audio source that is fading out.</param>
        /// <returns></returns>
        private IEnumerator<float> _CrossfadePlay(AudioClip audioClip, AudioSource fadingIn, AudioSource fadingOut)
        {
            fadingIn.clip = audioClip;
            fadingIn.Play();
            
            for (var i = 0; i < crossfadeSteps; i++)
            {
                var t = (float)i / crossfadeSteps;
                fadingIn.volume = Mathf.Lerp(0, 1, t);
                fadingOut.volume = Mathf.Lerp(1, 0, t);
                yield return Timing.WaitForOneFrame;
            }

            fadingOut.Stop();
            initialAudioSourceIsActive = !initialAudioSourceIsActive;
        }

        // private void StopPlayback(){}
    }
}