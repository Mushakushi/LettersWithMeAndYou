using System;
using Framework.Railways.Runtime.Interactable.Carriage;
using Mushakushi.Attributes.Runtime;
using UnityEngine;

namespace Framework.Railways.Runtime.Paths.PathBehaviour
{
    /// <summary>
    /// Allows the next path of a path to be variable. 
    /// </summary>
    [Serializable]
    public class RailwayPathSwitchContinue: RailwayPath
    {
        [field: ReadOnly, SerializeField] public override RailwayPath PreviousPath { get; protected set; }

        public override RailwayPath NextPath
        {
            get => nextConnections[nextConnectionIndex];
            protected set { }
        }

        /// <summary>
        /// Collection of <see cref="RailwayPath"/> connected to this.
        /// </summary>
        [SerializeField] private RailwayPath[] nextConnections;

        /// <summary>
        /// The index of the next <see cref="nextConnections"/>. 
        /// </summary>
        [SerializeField] private int nextConnectionIndex;
        
        public override Action<Carriage> ReachStart => carriage => carriage.SetRailwayPath(PreviousPath, 1);
        public override Action<Carriage> ReachEnd => carriage => carriage.SetRailwayPath(NextPath, 0);

        /// <summary>
        /// Callback on path switch.
        /// </summary>
        public event SwitchAction OnSwitch;

        /// <summary>
        /// Callback on path switch. 
        /// </summary>
        /// <param name="nextConnectionIndex">The index of the next connection.</param>
        public delegate void SwitchAction(int nextConnectionIndex);

        /// <summary>
        /// Changes the next connection. 
        /// </summary>
        public void ChangeNextConnection()
        {
            nextConnectionIndex = ++nextConnectionIndex % nextConnections.Length;
            OnSwitch?.Invoke(nextConnectionIndex);
        }
    }
}