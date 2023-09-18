using System;
using UnityEngine;

namespace Framework.Audio.Runtime.ScriptableObjects.Channels
{
    /// <summary>
    /// Channel on which music and SFX are played. 
    /// </summary>
    [CreateAssetMenu(fileName = "AudioEventChannel", menuName = "ScriptableObjects/Audio/Channels/Audio Event Channel", order = 0)]
    public class AudioEventChannel : ScriptableObject
    {
        /// <summary>
        /// Callback on <see cref="AudioClip"/> play request. 
        /// </summary>
        public event Action<AudioClip> OnAudioClipPlayRequested; 

        /// <summary>
        /// Callback on requesting all audio to be stopped. 
        /// </summary>
        public event Action OnPlaybackStopRequested;

        /// <summary>
        /// Raises the <see cref="OnAudioClipPlayRequested"/> event. 
        /// </summary>
        /// <param name="audioClip">The <see cref="AudioClip"/> to play.</param>
        public void RaiseOnAudioClipPlayRequested(AudioClip audioClip)
        {
            OnAudioClipPlayRequested?.Invoke(audioClip);
        }

        /// <summary>
        /// Raises the <see cref="OnPlaybackStopRequested"/> event. 
        /// </summary>
        public void RaiseOnPlaybackStopRequested()
        {
            OnPlaybackStopRequested?.Invoke();
        }
    }
}