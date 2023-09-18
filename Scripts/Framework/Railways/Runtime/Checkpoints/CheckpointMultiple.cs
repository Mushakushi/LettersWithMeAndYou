using Framework.Railways.Runtime.Paths.PathBehaviour;
using Mushakushi.Attributes.Runtime;
using UnityEngine;

namespace Framework.Railways.Runtime.Checkpoints
{
    public class CheckpointMultiple: CheckpointSingle
    {
        protected override Checkpoint NextCheckpoint => nextCheckpoints[nextCheckpointIndex];

        /// <summary>
        /// The collection of <see cref="Checkpoint"/> that may occur next. 
        /// </summary>
        [SerializeField] private Checkpoint[] nextCheckpoints;

        /// <summary>
        /// The index of an element in <see cref="nextCheckpoints"/> that shows the <see cref="NextCheckpoint"/>. 
        /// </summary>
        [ReadOnly, SerializeField] private int nextCheckpointIndex;

        /// <summary>
        /// The <see cref="GameObject"/> that acts as an indicator for the <see cref="nextCheckpointIndex"/>. 
        /// </summary>
        [SerializeField] private GameObject indicator; 

        /// <summary>
        /// The <see cref="RailwayPathSwitchContinue"/> used to determine whether to change the
        /// <see cref="nextCheckpointIndex"/>. 
        /// </summary>
        [SerializeField] private RailwayPathSwitchContinue railwayPathSwitchContinue;

        private void OnEnable()
        {
            railwayPathSwitchContinue.OnSwitch += UpdateNextCheckpointIndex;
        }

        private void OnDisable()
        {
            railwayPathSwitchContinue.OnSwitch -= UpdateNextCheckpointIndex;
        }

        private void Start()
        {
            UpdateIndicator();
        }

        /// <summary>
        /// Sets the value of <see cref="nextCheckpointIndex"/>. 
        /// </summary>
        /// <param name="newIndex">The new value to set.</param>
        private void UpdateNextCheckpointIndex(int newIndex)
        {
            nextCheckpointIndex = newIndex; 
            UpdateIndicator();
        }

        /// <summary>
        /// Updates the orientation of the <see cref="indicator"/>. 
        /// </summary>
        private void UpdateIndicator()
        {
            var direction = (NextCheckpoint.transform.position - indicator.transform.position).normalized;
            indicator.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); 
        }
    }
}