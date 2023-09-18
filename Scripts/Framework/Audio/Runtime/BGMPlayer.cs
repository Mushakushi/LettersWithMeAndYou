using Framework.Audio.Runtime.ScriptableObjects.Channels;
using UnityEngine;

namespace Framework.Audio.Runtime
{
    /// <summary>
    /// Plays a background music. 
    /// </summary>
    public class BGMPlayer: MonoBehaviour
    {
        /// <summary>
        /// The initial music to play, if any. 
        /// </summary>
        [SerializeField] private AudioClip initialAudioClip;

        [SerializeField] private AudioEventChannel musicEventChannel;
        
        private void Start()
        {
            if (initialAudioClip) musicEventChannel.RaiseOnAudioClipPlayRequested(initialAudioClip);
        }
    }
}