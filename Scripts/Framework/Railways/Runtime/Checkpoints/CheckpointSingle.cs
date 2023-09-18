using UnityEngine;

namespace Framework.Railways.Runtime.Checkpoints
{
    /// <summary>
    /// Places the carriage's letter when entered. Removes it when exited. 
    /// </summary>
    public class CheckpointSingle: Checkpoint
    {
        /// <summary>
        /// The index that the checkpoint deals with in the word. 
        /// </summary>
        [SerializeField] private int indexInWord;
        
        protected override void OnCarriageEnter()
        {
            currentCarriage.SetColor(true);
            puzzleEventChannel.RaiseOnLetterPlaced(currentCarriage.Letter, indexInWord);
        }

        protected override void OnCarriageExit()
        {
            puzzleEventChannel.RaiseOnLetterRemoved(indexInWord);
        }
    }
}