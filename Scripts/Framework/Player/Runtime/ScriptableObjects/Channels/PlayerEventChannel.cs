using System;
using UnityEngine;

namespace Framework.Player.Runtime.ScriptableObjects.Channels
{
    /// <summary>
    /// Sends and receives event relating to a player's input. 
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerEventChannel", menuName = "ScriptableObjects/Player/Channels/Player Event Channel", order = 0)]
    public class PlayerEventChannel : ScriptableObject
    {
        /// <summary>
        /// Callback on interact button pressed.  
        /// </summary>
        public event Action OnInteract;

        /// <summary>
        /// Callback on trigger enter. 
        /// </summary>
        public event Action<Collider> OnTriggerStay;

        /// <summary>
        /// Callback on trigger exit. 
        /// </summary>
        public event Action<Collider> OnTriggerExit; 

        /// <summary>
        /// Callback on pause button pressed. 
        /// </summary>
        public event Action<string> OnPause;

        /// <summary>
        /// Callback target direction updated. 
        /// </summary>
        public event Action<Vector3> OnTargetDirectionUpdated;

        /// <summary>
        /// Callback on velocity updated. 
        /// </summary>
        public event Action<Vector3> OnVelocityUpdated;

        /// <summary>
        /// Callback on animator's delta position changed. 
        /// </summary>
        public event Action<Vector3> OnAnimatorDeltaPositionUpdated; 

        /// <summary>
        /// Raises the <see cref="OnInteract"/> event. 
        /// </summary>
        public void RaiseOnInteract()
        {
            OnInteract?.Invoke();
        }

        /// <summary>
        /// Raises the <see cref="OnTriggerStay"/> event. 
        /// </summary>
        /// <param name="other">The other <see cref="Collider"/> involved in the collision.</param>
        public void RaiseOnTriggerStay(Collider other)
        {
            OnTriggerStay?.Invoke(other);
        }

        /// <summary>
        /// Raises the <see cref="OnTriggerExit"/> event. 
        /// </summary>
        /// <param name="other">The other <see cref="Collider"/> involved in the collision.</param>
        public void RaiseOnTriggerExit(Collider other)
        {
            OnTriggerExit?.Invoke(other);
        }

        /// <summary>
        /// Raises the <see cref="OnPause"/> event. 
        /// </summary>
        /// <param name="controlScheme">The control scheme that is pausing.</param>
        public void RaiseOnPause(string controlScheme)
        {
            OnPause?.Invoke(controlScheme); 
        }

        /// <summary>
        /// Raises the <see cref="OnTargetDirectionUpdated"/> event. 
        /// </summary>
        /// <param name="targetDirection">The direction.</param>
        public void RaiseOnTargetDirectionUpdated(Vector3 targetDirection)
        {
            OnTargetDirectionUpdated?.Invoke(targetDirection);
        }

        /// <summary>
        /// Raises the <see cref="OnVelocityUpdated"/> event. 
        /// </summary>
        /// <param name="velocity">The velocity.</param>
        public void RaiseOnVelocityUpdated(Vector3 velocity)
        {
            OnVelocityUpdated?.Invoke(velocity);
        }

        /// <summary>
        /// Raises the <see cref="OnAnimatorDeltaPositionUpdated"/> event. 
        /// </summary>
        /// <param name="deltaPosition">The delta position of the animator.</param>
        /// <remarks>Requires an animator with root motion.</remarks>
        public void RaiseOnAnimatorDeltaPositionUpdated(Vector3 deltaPosition)
        {
            OnAnimatorDeltaPositionUpdated?.Invoke(deltaPosition);
        }
    }
}