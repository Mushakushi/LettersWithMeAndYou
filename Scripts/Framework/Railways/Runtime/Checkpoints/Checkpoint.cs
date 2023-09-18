using Framework.Railways.Runtime.Interactable.Carriage;
using Framework.Railways.Runtime.ScriptableObjects.Channels;
using Mushakushi.Attributes.Runtime;
using UnityEngine;

namespace Framework.Railways.Runtime.Checkpoints
{
    /// <summary>
    /// Represents a checkpoint for each carriage. 
    /// </summary>
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public abstract class Checkpoint : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="PuzzleEventChannel"/>. 
        /// </summary>
        [Header("Data"), SerializeField] protected PuzzleEventChannel puzzleEventChannel;
        
        /// <summary>
        /// The collection of <see cref="Checkpoint"/> that follows this along a path. 
        /// </summary>
        [field: SerializeField] protected virtual Checkpoint NextCheckpoint { get; private set; }

        /// <summary>
        /// Carriages transition to the center of each <see cref="CheckpointSingle"/>.
        /// Determines the fixed step interval over which this transition should occur. 
        /// </summary>
        [SerializeField, Min(0)] private int realignSteps = 2;

        [ReadOnly, SerializeField] private new Rigidbody rigidbody;
        [ReadOnly, SerializeField] private new BoxCollider collider;

        /// <summary>
        /// The carriage within this trigger. 
        /// </summary>
        /// <remarks>Assumes only one <see cref="Carriage"/> can collided with at a time.</remarks>
        [ReadOnly, SerializeField] protected Carriage currentCarriage;

        private const int MaxContacts = 8; 

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<BoxCollider>();

            collider.isTrigger = true;
            rigidbody.isKinematic = true;
        }

        private void Start()
        {
            ManualOnTriggerEnter();
        }

        /// <summary>
        /// Calls <see cref="OnTriggerEnter"/> for each <see cref="Collider"/> that overlaps with <see cref="collider"/>. 
        /// </summary>
        /// <remarks>
        /// Useful for checking intersections that occur on <see cref="Start"/>. 
        /// </remarks>
        private void ManualOnTriggerEnter()
        {
            var bounds = collider.bounds;
            var contacts = new Collider[MaxContacts];
            var halfExtents = bounds.extents * 0.5f;
            var size = Physics.OverlapBoxNonAlloc(bounds.center, halfExtents, contacts, transform.rotation, 
                Physics.AllLayers, QueryTriggerInteraction.Collide);
            for (var i = 0; i < size; i++) OnTriggerEnter(contacts[i]);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Carriage>(out var carriage)) return;
            currentCarriage = carriage;
            currentCarriage.Pause();
            currentCarriage.normalizedTime = currentCarriage.MoveToNearestPoint(transform.position, realignSteps);
            currentCarriage.OnPlayRequested += ValidatePlay;
            OnCarriageEnter();
        }
        
        /// <summary>
        /// What happens when entering the trigger of another <see cref="Carriage"/>. 
        /// </summary>
        protected abstract void OnCarriageEnter();

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Carriage>(out _)) return;
            currentCarriage.OnPlayRequested -= ValidatePlay;
            currentCarriage = null;
            OnCarriageExit();
        }

        private void OnDisable()
        {
            if (currentCarriage) currentCarriage.OnPlayRequested -= ValidatePlay;
        }

        /// <summary>
        /// What happens when exiting the trigger of another <see cref="Carriage"/>. 
        /// </summary>
        protected abstract void OnCarriageExit();

        /// <summary>
        /// Plays the <see cref="currentCarriage"/> if the <see cref="Checkpoint.currentCarriage"/> is null.
        /// Otherwise, <see cref="Carriage.OnPlayRequestFailed"/> is called on the <see cref="currentCarriage"/>. 
        /// </summary>
        private void ValidatePlay()
        {
            if (NextCheckpoint.currentCarriage == null) currentCarriage.Play();
            else currentCarriage.OnPlayRequestFailed();
        }
    }
}