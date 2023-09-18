using System;
using Framework.Railways.Runtime.Interactable.Carriage;
using UnityEngine;
using UnityEngine.Splines;

namespace Framework.Railways.Runtime.Paths.PathBehaviour
{
    public abstract class RailwayPath: MonoBehaviour
    {
        /// <summary>
        /// The <see cref="SplineContainer"/> to follow. 
        /// </summary>
        [field: SerializeField] public SplineContainer Container { get; protected set; }

        /// <summary>
        /// Gets the <see cref="RailwayPath"/> directly before this one, if any. 
        /// </summary>
        public abstract RailwayPath PreviousPath { get; protected set; }
        
        /// <summary>
        /// Gets the <see cref="RailwayPath"/> directly after this one, if any. 
        /// </summary>
        public abstract RailwayPath NextPath { get; protected set; }

        /// <summary>
        /// Callback on reaching the start of the <see cref="Container"/>. 
        /// </summary>
        public abstract Action<Carriage> ReachStart { get; }
        
        /// <summary>
        /// Callback on reaching the end of the <see cref="Container"/>. 
        /// </summary>
        public abstract Action<Carriage> ReachEnd { get; }
    }
}