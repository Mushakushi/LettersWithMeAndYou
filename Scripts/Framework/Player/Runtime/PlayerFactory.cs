using Cinemachine;
using Framework.GlobalDataContainers.Runtime;
using Mushakushi.InternalDebug.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Framework.Player.Runtime
{
    /// <summary>
    /// Responsible for instantiating players. 
    /// </summary>
    public class PlayerFactory: MonoBehaviour
    {
        /// <summary>
        /// The <see cref="globalPlayerData"/>. 
        /// </summary>
        [SerializeField] private GlobalPlayerData globalPlayerData;

        [SerializeField] private CinemachineTargetGroup playerTargetGroup;

        /// <summary>
        /// Spawn a Player. 
        /// </summary>
        /// <param name="playerNumber">The number of the player to spawn. 1 or 2.</param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void CreateInstance(PlayerNumber playerNumber, Vector3 position, Quaternion rotation)
        {
            PlayerMovementController player;
            switch (playerNumber)
            {
                case PlayerNumber.One:
                    player = InstantiateInstance(globalPlayerData.Player1Prefab, globalPlayerData.Player1ControlScheme, Keyboard.current); 
                    break;
                case PlayerNumber.Two:
                    player = InstantiateInstance(globalPlayerData.Player2Prefab, globalPlayerData.Player2ControlScheme, Keyboard.current); 
                    break;
                default: 
                    InternalDebug.LogWarning("The player number of this player is not valid! It will not be instantiated.", gameObject);
                    return;
            }
            
            SpawnPlayer(player, position, rotation);
        }

        /// <summary>
        /// Instantiates the player. 
        /// </summary>
        /// <param name="prefab">The player prefab.</param>
        /// <param name="controlScheme">The control scheme to use for this player.</param>
        /// <param name="pairWithDevice">The device to pair the player with.</param>
        /// <returns><see cref="PlayerMovementController"/> Attached to the <see cref="prefab"/></returns>
        private static PlayerMovementController InstantiateInstance(GameObject prefab, string controlScheme, InputDevice pairWithDevice)
        {
            if (pairWithDevice == null)
            {
                InternalDebug.LogError("Could not pair with device! Player will not be instantiated.");
                return null;
            }
            var playerInput = PlayerInput.Instantiate(prefab, controlScheme: controlScheme, pairWithDevice: pairWithDevice);
            var player = GetPlayerComponentFromAttachedComponent(playerInput);
            player.controlScheme = controlScheme;
            return player; 
        }

        /// <summary>
        /// Spawns a player. 
        /// </summary>
        /// <param name="playerMovementController">To spawn.</param>
        /// <param name="position">At position.</param>
        /// <param name="rotation">At rotation.</param>
        private void SpawnPlayer(PlayerMovementController playerMovementController, Vector3 position, Quaternion rotation)
        {
            var playerTransform = playerMovementController.transform;
            playerMovementController.Controller.enabled = false;
            playerTransform.position = position;
            playerTransform.rotation = rotation; 
            if (Physics.Raycast(playerMovementController.transform.position, Vector3.down, out var raycastHit))
            {
                playerMovementController.transform.position = raycastHit.point; // character controller will move itself out of the collider 
            }
            playerMovementController.Controller.enabled = true;

            playerTransform.SetParent(transform);
            playerTargetGroup.AddMember(playerTransform, 1, playerMovementController.Controller.radius);
        }

        /// <summary>
        /// Retrieves the <see cref="PlayerMovementController"/> component. 
        /// </summary>
        /// <param name="component">The sibling component.</param>
        /// <returns><see cref="PlayerMovementController"/> The component.</returns>
        private static PlayerMovementController GetPlayerComponentFromAttachedComponent(Component component)
        {
            if (component.TryGetComponent<PlayerMovementController>(out var playerComponent)) return playerComponent;
            InternalDebug.LogError($"{component.name} does not have a {nameof(PlayerMovementController)} component attached!");
            return null;
        }
    }
}