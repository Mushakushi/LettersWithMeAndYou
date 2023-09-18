using System;
using UnityEngine;

namespace Framework.Railways.Runtime.ScriptableObjects.Channels
{
    /// <summary>
    /// Channel for puzzle related events. 
    /// </summary>
    [CreateAssetMenu(fileName = "PuzzleEventChannel", menuName = "ScriptableObjects/Railways/Channels/Puzzle Event Channel", order = 0)]
    public class PuzzleEventChannel : ScriptableObject
    {
        /// <summary>
        /// Callback on letter placed.
        /// </summary>
        public event LetterPlacedAction OnLetterPlaced;

        /// <summary>
        /// Callback on letter placed.
        /// </summary>
        /// <param name="letter">The letter that was placed.</param>
        /// <param name="index">The index of the letter that was placed in the word string.</param>
        public delegate void LetterPlacedAction(char letter, int index);

        /// <summary>
        /// Callback on letter removed. 
        /// </summary>
        public event LetterRemoveAction OnLetterRemoved;

        /// <summary>
        /// Callback on letter removed.
        /// </summary>
        /// <param name="index">The index of the letter that was removed in the word string.</param>
        public delegate void LetterRemoveAction(int index);

        /// <summary>
        /// Callback on word updated. 
        /// </summary>
        public event Action<string> OnWordUpdated; 

        /// <summary>
        /// Callback on puzzle solved. 
        /// </summary>
        public event Action OnPuzzleSolved; 

        /// <summary>
        /// Raises the <see cref="OnLetterPlaced"/> event.
        /// </summary>
        /// <param name="letter">The letter that was placed.</param>
        /// <param name="index">The index of the letter placed in the word string.</param>
        public void RaiseOnLetterPlaced(char letter, int index)
        {
            OnLetterPlaced?.Invoke(letter, index);
        }
        
        /// <summary>
        /// Raises the <see cref="OnLetterRemoved"/> event.
        /// </summary>
        /// <param name="index">The index of the letter removed in the word string.</param>
        public void RaiseOnLetterRemoved(int index)
        {
            OnLetterRemoved?.Invoke(index);
        }
        
        /// <summary>
        /// Raises the <see cref="OnWordUpdated"/> event. 
        /// </summary>
        /// <param name="word">The new word.</param>
        public void RaiseOnWordUpdated(string word)
        {
            OnWordUpdated?.Invoke(word);
        }

        /// <summary>
        /// Raises the <see cref="OnPuzzleSolved"/> event. 
        /// </summary>
        public void RaiseOnPuzzleSolved()
        {
            OnPuzzleSolved?.Invoke();
        }
    }
}