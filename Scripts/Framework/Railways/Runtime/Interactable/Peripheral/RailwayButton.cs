using UnityEngine;

namespace Framework.Railways.Runtime.Interactable.Peripheral
{
    /// <summary>
    /// Performs an interaction when stepped on. 
    /// </summary>
    public class RailwayButton: MonoBehaviour, IRailwayInteractable
    {
        
        public void OnInteract(){}

        public void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}