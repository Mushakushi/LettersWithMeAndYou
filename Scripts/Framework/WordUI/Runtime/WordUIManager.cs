using System.Collections.Generic;
using Framework.Railways.Runtime.ScriptableObjects.Channels;
using MEC;
using TMPro;
using UnityEngine;

namespace Framework.WordUI.Runtime
{
    /// <summary>
    /// Manages the runtime display of the current word order. 
    /// </summary>
    public class WordUIManager : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="PuzzleEventChannel"/>. 
        /// </summary>
        [Header("Data")] 
        [SerializeField] private PuzzleEventChannel puzzleEventChannel;
        
        /// <summary>
        /// The <see cref="TMP_Text"/> used to render the word. 
        /// </summary>
        [SerializeField] private TMP_Text textComponent;
        
        /// <summary>
        /// The delay per iteration of <see cref="_TypeNewText"/> in seconds.
        /// </summary>
        [Header("Configuration")]
        [SerializeField] private float typeDelay;
        
        /// <summary>
        /// The delay per iteration of <see cref="_DeleteArrangedWord"/> in seconds.
        /// </summary>
        [SerializeField] private float deleteDelay;

        private void Awake()
        {
            textComponent.text = string.Empty;
        }

        private void OnEnable()
        {
            puzzleEventChannel.OnWordUpdated += Type;
        }
        
        private void OnDisable()
        {
            puzzleEventChannel.OnWordUpdated -= Type;
        }

        /// <summary>
        /// Types new word. 
        /// </summary>
        private void Type(string word)
        {
            Timing.RunCoroutine(_TypeNewText(word)); 
        }
        
        private IEnumerator<float> _TypeNewText(string text)
        {
            yield return Timing.WaitUntilDone(Timing.RunCoroutine(_DeleteArrangedWord())); 
            foreach (var character in text)
            {
                textComponent.text += character;
                yield return Timing.WaitForSeconds(typeDelay); 
            }
        }

        private IEnumerator<float> _DeleteArrangedWord()
        {
            var text = textComponent.text;
            for (var i = text.Length; i >= 0; i--)
            {
                textComponent.text = text[..i]; 
                yield return Timing.WaitForSeconds(deleteDelay); 
            }
        }
    }
}