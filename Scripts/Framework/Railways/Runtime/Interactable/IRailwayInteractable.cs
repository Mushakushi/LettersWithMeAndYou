using UnityEngine;

namespace Framework.Railways.Runtime.Interactable
{
    /// <summary>
    /// An interactable object. 
    /// </summary>
    public interface IRailwayInteractable
    {
        /// <summary>  
        /// This GameObject
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public GameObject gameObject { get; }
    
        /// <summary>
        /// What happens when interacted with
        /// </summary>
        public void OnInteract(); 
    }
}