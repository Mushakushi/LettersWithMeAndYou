using Mushakushi.Attributes.Runtime;
using UnityEngine;

namespace Framework.Railways.Runtime.ScriptableObjects.DataContainers
{
    /// <summary>
    /// Describes the puzzle for any level. 
    /// </summary>
    [CreateAssetMenu(fileName = "PuzzleConfiguration", menuName = "ScriptableObjects/Railways/Data Containers/Puzzle Configuration", order = 0)]
    public class PuzzleConfiguration : ScriptableObject
    {
        /// <summary>
        /// The currently placed letters. 
        /// </summary>
        [ReadOnly, SerializeField] public char[] arrangedWord;
        
        /// <summary>
        /// The word. 
        /// </summary>
        [field: SerializeField] public char[] Word { get; private set; }
        
        /// <summary>
        /// This character describes a letter that hasn't been placed. 
        /// </summary>
        public const char NoBreakSpace = '\u00A0';
    }
}