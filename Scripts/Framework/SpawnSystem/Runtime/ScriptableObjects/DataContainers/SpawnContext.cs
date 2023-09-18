using UnityEngine;

namespace Framework.SpawnSystem.Runtime.ScriptableObjects.DataContainers
{
    [CreateAssetMenu(fileName = "SpawnContext", menuName = "ScriptableObjects/Spawn System/Data Containers/Spawn Context", order = 0)]
    public class SpawnContext : ScriptableObject
    {
        /// <summary>
        /// Player one's spawn position. 
        /// </summary>
        [field: Header("Spawning"), Tooltip("Player one's spawn position."), SerializeField]
        public Vector3 Player1Position { get; private set; }
        
        /// <summary>
        /// Player one's rotation. 
        /// </summary>
        [field: SerializeField] public Quaternion Player1Rotation { get; private set; }
        
        /// <summary>
        /// Player two's spawn position. 
        /// </summary>
        [field: Space, Tooltip("Player two's spawn position."), SerializeField]
        public Vector3 Player2Position { get; private set; }
        
        /// <summary>
        /// Player two's rotation. 
        /// </summary>
        [field: SerializeField] public Quaternion Player2Rotation { get; private set; }
    }
}