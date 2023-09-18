using Framework.Player.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.DataContainers;
using UnityEngine;

namespace Framework.Player.Runtime
{
    /// <summary>
    /// Controls the menus for each player. 
    /// </summary>
    public class PlayerMenuController: MonoBehaviour
    {
        [Header("Data"), SerializeField] private PlayerEventChannel playerEventChannel;
        [SerializeField] private MenuEventChannel menuEventChannel;
        
        /// <summary>
        /// The pause menu to display. 
        /// </summary>
        [SerializeField] private Menu pauseMenu;

        private void OnEnable()
        {
            playerEventChannel.OnPause += RequestOpenMenu;
        }

        private void OnDisable()
        {
            playerEventChannel.OnPause -= RequestOpenMenu;
        }

        /// <summary>
        /// Opens the menu <see cref="pauseMenu"/>. 
        /// </summary>
        /// <param name="controlScheme">The control scheme being used to open the menu.</param>
        private void RequestOpenMenu(string controlScheme)
        {
            menuEventChannel.RaiseOnOpenRequested(pauseMenu, controlScheme);
        }
    }
}