using UnityEngine;

namespace Framework.Railways.Runtime.Checkpoints
{
    /// <summary>
    /// Empty checkpoint. Keeps the cart there until its target index is cleared. 
    /// </summary>
    public class CheckpointFreeSpace: Checkpoint
    {
        /// <summary>
        /// The index that must be open before this checkpoint plays. 
        /// </summary>
        [SerializeField] private int nextIndex; 
        
        private void OnEnable()
        {
            puzzleEventChannel.OnLetterRemoved += CheckIndexIsEmpty;
        }
        
        private void OnDisable()
        {
            puzzleEventChannel.OnLetterRemoved += CheckIndexIsEmpty;
        }

        private void CheckIndexIsEmpty(int removedIndex)
        {
            if (removedIndex != nextIndex) return;
            if (currentCarriage) currentCarriage.Play();
        }

        protected override void OnCarriageEnter()
        {
            currentCarriage.SetColor(false);
        }

        protected override void OnCarriageExit(){}
    }
}