using Framework.Player.Runtime;
using Framework.SceneManagement.Runtime.ScriptableObjects.Channels;
using Framework.SceneManagement.Runtime.ScriptableObjects.DataContainers;
using Framework.SpawnSystem.Runtime.ScriptableObjects.DataContainers;
using UnityEngine;

namespace Framework.SpawnSystem.Runtime
{
    /// <summary>
    /// Manages spawning. 
    /// </summary>
    public class SpawnManager: MonoBehaviour
    {
        /// <summary>
        /// The <see cref="SpawnContext"/> for this level. 
        /// </summary>
        [Header("Configuration"), SerializeField] private SpawnContext spawnContext;

        /// <summary>
        /// The <see cref="PlayerFactory"/>. 
        /// </summary>
        [SerializeField] private PlayerFactory playerFactory; 
        
        /// <summary>
        /// The <see cref="SceneEventChannel"/>.
        /// </summary>
        [Header("Listening to Channel"), Tooltip("The SceneLoadEventChannel"), SerializeField]
        private SceneEventChannel sceneEventChannel;

        private void OnEnable()
        {
            sceneEventChannel.OnAsyncOperationsCompleted += InitializeScene;
        }

        private void OnDisable()
        {
            sceneEventChannel.OnAsyncOperationsCompleted -= InitializeScene;
        }

        /// <summary>
        /// Initializes the scene from the <see cref="SceneData"/>. 
        /// </summary>
        private void InitializeScene()
        {
            playerFactory.CreateInstance(PlayerNumber.One, spawnContext.Player1Position, spawnContext.Player1Rotation);
            playerFactory.CreateInstance(PlayerNumber.Two, spawnContext.Player2Position, spawnContext.Player2Rotation);
            sceneEventChannel.RaiseOnSceneReady();
        }
    }
}