using System.Linq;
using Framework.Railways.Runtime.ScriptableObjects.Channels;
using Framework.Railways.Runtime.ScriptableObjects.DataContainers;
using UnityEngine;

namespace Framework.Railways.Runtime
{
    /// <summary>
    /// Manages the puzzle for any level. 
    /// </summary>
    public class PuzzleManager: MonoBehaviour
    {
        /// <summary>
        /// The <see cref="PuzzleConfiguration"/>. 
        /// </summary>
        [SerializeField] private PuzzleConfiguration puzzleConfiguration;

        /// <summary>
        /// The <see cref="puzzleEventChannel"/>. 
        /// </summary>
        [SerializeField] private PuzzleEventChannel puzzleEventChannel;

        /// <summary>
        /// Whether or not all characters in <see cref="PuzzleConfiguration.arrangedWord"/>
        /// (which are <see cref="PuzzleConfiguration.NoBreakSpace"/> on <see cref="Awake"/>),
        /// have been set <see cref="PlaceLetter"/> for the first time.
        /// </summary>
        private bool isReady; 

        private void Awake()
        {
            Prewarm();
        }

        private void OnEnable()
        {
            puzzleEventChannel.OnLetterPlaced += PlaceLetter;
            puzzleEventChannel.OnLetterRemoved += RemoveLetter; 
        }
        
        private void OnDisable()
        {
            puzzleEventChannel.OnLetterPlaced -= PlaceLetter;
            puzzleEventChannel.OnLetterRemoved -= RemoveLetter; 
        }

        /// <summary>
        /// Initializes the <see cref="PuzzleConfiguration.arrangedWord"/>. 
        /// </summary>
        private void Prewarm()
        {
            var length = puzzleConfiguration.Word.Length;
            puzzleConfiguration.arrangedWord = new char[length];
            for (var i = 0; i < length; ++i) puzzleConfiguration.arrangedWord[i] = PuzzleConfiguration.NoBreakSpace;
        }

        /// <summary>
        /// Places a letter and determines if it solved the level. 
        /// </summary>
        /// <param name="letter">The letter that was placed.</param>
        /// <param name="index">The index of the letter placed in the word string.</param>
        private void PlaceLetter(char letter, int index)
        {
            puzzleConfiguration.arrangedWord[index] = letter;
            if (isReady || (isReady = CheckIfReady())) RaiseWordUpdate();
            if (GetPuzzleStatus()) puzzleEventChannel.RaiseOnPuzzleSolved();
        }

        /// <summary>
        /// Determines <see cref="isReady"/>. 
        /// </summary>
        /// <returns>Whether or not <see cref="isReady"/> should be true.</returns>
        private bool CheckIfReady()
        {
            return puzzleConfiguration.arrangedWord.All(character => character != PuzzleConfiguration.NoBreakSpace);
        }
        
        /// <summary>
        /// Removes a letter.  
        /// </summary>
        /// <param name="index">The index of the letter removed in the word string.</param>
        private void RemoveLetter(int index)
        {
            puzzleConfiguration.arrangedWord[index] = PuzzleConfiguration.NoBreakSpace;
            RaiseWordUpdate();
        }
        
        /// <summary>
        /// Raises the <see cref="PuzzleEventChannel.OnWordUpdated"/> event with the <see cref="PuzzleConfiguration.arrangedWord"/>. 
        /// </summary>
        private void RaiseWordUpdate() => puzzleEventChannel.RaiseOnWordUpdated(new string(puzzleConfiguration.arrangedWord));
        
        /// <summary>
        /// Returns whether or not <see cref="PuzzleConfiguration.arrangedWord"/> and <see cref="PuzzleConfiguration.Word"/> spell the same word. 
        /// </summary>
        /// <returns></returns>
        private bool GetPuzzleStatus()
        {
            return puzzleConfiguration.arrangedWord.SequenceEqual(puzzleConfiguration.Word);
        }
    }
}