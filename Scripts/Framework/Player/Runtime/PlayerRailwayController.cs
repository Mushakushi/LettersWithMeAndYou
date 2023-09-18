using System.Collections.Generic;
using Framework.Player.Runtime.ScriptableObjects.Channels;
using Framework.Railways.Runtime.Interactable;
using JetBrains.Annotations;
using MEC;
using Mushakushi.Attributes.Runtime;
using UnityEngine;

namespace Framework.Player.Runtime
{
    /// <summary>
    /// Controls the player's interactions with the Railway system.
    /// </summary>
    public class PlayerRailwayController : MonoBehaviour
    {
        [Header("Data"), SerializeField] private PlayerEventChannel playerEventChannel;

        /// <summary>
        /// The <see cref="IRailwayInteractable"/> being touched. 
        /// </summary>
        private IRailwayInteractable currentInteractable;
        
#if UNITY_EDITOR
        /// <summary>
        /// Debug show the <see cref="IRailwayInteractable"/> implementing GameObject. 
        /// </summary>
        [field: ReadOnly, UsedImplicitly, SerializeField] private GameObject currentInteractableGameObject; 
#endif

        /// <summary>
        /// The delay in seconds between interacting with an object and the interaction actually happening. 
        /// </summary>
        [SerializeField, Min(0)] private float interactDelay; 

        private void OnEnable()
        {
            playerEventChannel.OnTriggerStay += GetInteractableFromCollider;
            playerEventChannel.OnTriggerExit += RemoveInteractable;
            playerEventChannel.OnInteract += MoveCarriage;
        }
        
        private void OnDisable()
        {
            playerEventChannel.OnTriggerStay -= GetInteractableFromCollider;
            playerEventChannel.OnTriggerExit -= RemoveInteractable;
            playerEventChannel.OnInteract -= MoveCarriage;
        }

        private void GetInteractableFromCollider(Collider other)
        {
            if (!other.TryGetComponent<IRailwayInteractable>(out var interactable)) return;
            SetCurrentInteractable(interactable);
        }

        private void RemoveInteractable(Collider other)
        {
            if (!other.TryGetComponent<IRailwayInteractable>(out _)) return;
            SetCurrentInteractable(null);
        }

        /// <summary>
        /// Sets the <see cref="currentInteractable"/>, invoking it's relevant events. 
        /// </summary>
        /// <param name="interactable">The <see cref="IRailwayInteractable"/>.</param>
        private void SetCurrentInteractable(IRailwayInteractable interactable)
        {
#if UNITY_EDITOR
            currentInteractableGameObject = interactable?.gameObject;
#endif
            currentInteractable = interactable;
        }

        private void MoveCarriage()
        {
            Timing.RunCoroutine(_MoveCarriage());
        }

        private IEnumerator<float> _MoveCarriage()
        {
            yield return Timing.WaitForSeconds(interactDelay);
            currentInteractable?.OnInteract();
        }
    }
}