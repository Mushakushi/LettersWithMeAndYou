using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Framework.GlobalDataContainers.Runtime
{
    /// <summary>
    /// Stores the prefab and the control configuration of the players.
    /// </summary>
    [CreateAssetMenu(fileName = "GlobalPlayerData", menuName = "ScriptableObjects/Spawn System/Data Containers/Global Player Data", order = 0)]
    public class GlobalPlayerData : ScriptableObject
    {
        /// <summary>
        /// Player one's prefab. 
        /// </summary>
        public GameObject Player1Prefab => player1Prefab;
        
        [Header("Prefabs"), Tooltip("Player one's prefab"), SerializeField]
        private GameObject player1Prefab;

        /// <summary>
        /// Player two's prefab
        /// </summary>
        public GameObject Player2Prefab => player2Prefab;

        [Tooltip("Player two's prefab"), SerializeField]
        private GameObject player2Prefab;

        /// <summary>
        /// The shared <see cref="InputActionAsset"/> between players. 
        /// </summary>
        public InputActionAsset InputActions => inputActions;

        [Header("Input"), Tooltip("The shared InputActionAsset between players."), SerializeField]
        private InputActionAsset inputActions;

        /// <summary>
        /// Player one's control scheme in the <see cref="InputActions"/>. 
        /// </summary>
        public string Player1ControlScheme => player1ControlScheme;

        [Tooltip("Player one's control scheme."), SerializeField]
        private string player1ControlScheme; 
        
        /// <summary>
        /// Player two's control scheme in the <see cref="InputActions"/>. 
        /// </summary>
        public string Player2ControlScheme => player2ControlScheme;

        [Tooltip("Player one's control scheme."), SerializeField]
        private string player2ControlScheme; 
        
#if UNITY_EDITOR
        /// <summary>
        /// The index of <see cref="Player1ControlScheme"/>. 
        /// </summary>
        [UsedImplicitly, SerializeField, HideInInspector] private int player1ControlSchemeIndex;
        
        /// <summary>
        /// The index of <see cref="Player2ControlScheme"/>. 
        /// </summary>
        [UsedImplicitly, SerializeField, HideInInspector] private int player2ControlSchemeIndex; 
#endif
    }
}