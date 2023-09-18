using Framework.GlobalDataContainers.Runtime;
using Framework.SceneManagement.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.DataContainers;
using UnityEngine;

namespace Framework.UI.Runtime.Menus.TitleScreen
{
    /// <summary>
    /// Requests the title screen when the scene is loaded. 
    /// </summary>
    public class TitleScreenRequester: MonoBehaviour
    {
        /// <summary>
        /// The <see cref="GlobalPlayerData"/>. 
        /// </summary>
        [SerializeField] private GlobalPlayerData globalPlayerData;
        
        /// <summary>
        /// The <see cref="menuEventChannel"/>. 
        /// </summary>
        [Header("Channels"), SerializeField]
        private MenuEventChannel menuEventChannel;

        [SerializeField] private SceneEventChannel sceneEventChannel;

        /// <summary>
        /// The <see cref="TitleScreenData"/>.
        /// </summary>
        [Tooltip("The title screen data."), SerializeField]
        private TitleScreenData titleScreen;
        
        private void Start()
        {
            // todo - handle if player 2 navigates to the title screen 
            menuEventChannel.RaiseOnOpenRequested(titleScreen.StartupMenu, globalPlayerData.Player1ControlScheme);
        }

        private void OnEnable()
        {
            menuEventChannel.OnOpenCompleted += sceneEventChannel.RaiseOnSceneReady;
        }
        
        private void OnDisable()
        {
            menuEventChannel.OnOpenCompleted -= sceneEventChannel.RaiseOnSceneReady;
        }
    }
}