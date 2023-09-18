using UnityEngine;

namespace Framework.UI.Runtime.ScriptableObjects.DataContainers
{
    /// <summary>
    /// Stores data about the title screen. 
    /// </summary>
    [CreateAssetMenu(fileName = "TitleScreenData", menuName = "ScriptableObjects/UI/Data Containers/Title Screen Data", order = 0)]
    public class TitleScreenData : ScriptableObject
    {
        /// <summary>
        /// The <see cref="Menu"/> to be displayed.
        /// </summary>
        [field: SerializeField] public Menu StartupMenu { get; private set; }
        
        // todo - can manage what the user has unlocked on the title screen, such as levels, here
    }
}