using Framework.Player.Runtime.ScriptableObjects.Channels;
using Framework.SceneManagement.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.Channels;
using Mushakushi.Attributes.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Framework.Player.Runtime
{
    /// <summary>
    /// Controls the player movement.
    /// </summary>
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Data"), SerializeField] private PlayerEventChannel playerEventChannel;
        [SerializeField] private MenuEventChannel menuEventChannel;
        [SerializeField] private SceneEventChannel sceneEventChannel;

        /// <summary>
        /// The <see cref="CharacterController"/>.
        /// </summary>
        [field: Header("Player"), ReadOnly, SerializeField] public CharacterController Controller { get; private set; }

        /// <summary>
        /// The <see cref="PlayerInput"/>.
        /// </summary>
        [ReadOnly, SerializeField] private PlayerInput playerInput;

        /// <summary>
        /// The assigned control scheme this player uses. 
        /// </summary>
        /// <seealso cref="PlayerFactory.globalPlayerData"/>
        [ReadOnly] public string controlScheme; 

        /// <summary>
        /// The target direction this frame. 
        /// </summary>
        [ReadOnly, SerializeField] private Vector2 inputDirection;

        /// <summary>
        /// The current velocity.
        /// </summary>
        [ReadOnly, SerializeField] public Vector3 velocity;

        /// <summary>
        /// Speed. 
        /// </summary>
        [SerializeField, Min(0.001f)] public float rootMotionSpeedMultiplier = 1f;

        /// <summary>
        /// Deceleration. 
        /// </summary>
        [SerializeField, Range(0.001f, 1f)] public float deceleration = 0.05f; 

        /// <summary>
        /// Gravity.
        /// </summary>
        public Vector3 gravity;
        
        /// <summary>
        /// How much force is applied to other rigidbodies <see cref="OnControllerColliderHit"/>.
        /// </summary>
        [SerializeField] private float pushPower = 1;
        
        private void Awake()
        {
            Controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            ActivateInput();
        }

        private void OnEnable()
        {
            menuEventChannel.OnOpenCompleted += DeactivateInput;
            menuEventChannel.OnCloseRequested += ActivateInput;
            sceneEventChannel.OnLoadingScreenDisplayed += Destroy;
            playerEventChannel.OnAnimatorDeltaPositionUpdated += OnAnimatorDeltaPositionUpdated;
        }
        
        private void OnDisable()
        {
            menuEventChannel.OnOpenCompleted -= DeactivateInput;
            menuEventChannel.OnCloseRequested -= ActivateInput;
            sceneEventChannel.OnLoadingScreenDisplayed -= Destroy;
            playerEventChannel.OnAnimatorDeltaPositionUpdated -= OnAnimatorDeltaPositionUpdated;
        }
        
        public void OnMove(InputAction.CallbackContext value)
        {
            inputDirection = value.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext value)
        {
            if (!value.started || !playerInput.inputIsActive) return;
            playerEventChannel.RaiseOnInteract();
        }

        public void OnPause(InputAction.CallbackContext value)
        {
            if (!value.started) return; 
            playerEventChannel.RaiseOnPause(controlScheme);
        }

        private void Update()
        {
            if (!playerInput.inputIsActive) return;
            AddVelocity((new Vector3(inputDirection.x, 0, inputDirection.y) + gravity) * Time.deltaTime);
            playerEventChannel.RaiseOnTargetDirectionUpdated(new Vector3(inputDirection.x, 0, inputDirection.y)); 
        }

        /// <summary>
        /// Updates the <see cref="velocity"/>. 
        /// </summary>
        private void AddVelocity(Vector3 additionalVelocity)
        {
            if (Controller.isGrounded) velocity.y = 0;
            velocity += additionalVelocity;
            velocity.x *= deceleration;
            velocity.z *= deceleration;
            playerEventChannel.RaiseOnVelocityUpdated(velocity);
        }

        /// <summary>
        /// Moves. 
        /// </summary>
        /// <param name="deltaPosition">The animator delta position.</param>
        /// <seealso cref="PlayerAnimator"/>
        private void OnAnimatorDeltaPositionUpdated(Vector3 deltaPosition)
        {
            Controller.Move(new Vector3(deltaPosition.x, velocity.y, deltaPosition.z) * rootMotionSpeedMultiplier); 
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            PushRigidbody(hit);
        }

        /// <summary>
        /// Pushes the <see cref="hit"/> if it has a non-kinematic <see cref="Rigidbody"/>. 
        /// </summary>
        /// <param name="hit">The <see cref="OnControllerColliderHit"/>.</param>
        private void PushRigidbody(ControllerColliderHit hit)
        {
            if (hit.controller != null) return; 
            var attachedRigidbody = hit.collider.attachedRigidbody;
            if (attachedRigidbody == null || attachedRigidbody.isKinematic) return;
            attachedRigidbody.velocity = hit.moveDirection * pushPower; 
        }

        /// <summary>
        /// Destroys this player on restart. 
        /// </summary>
        private void Destroy()
        {
            Destroy(gameObject);
            OnDisable();
        }

        /// <summary>
        /// Activates the attached <see cref="PlayerInput"/>.
        /// </summary>
        private void ActivateInput()
        {
            playerInput.ActivateInput();
        }

        /// <summary>
        /// Deactivates the attached <see cref="playerInput"/>.
        /// </summary>
        private void DeactivateInput()
        {
            playerInput.DeactivateInput();
        }

        private void OnTriggerStay(Collider other)
        {
            playerEventChannel.RaiseOnTriggerStay(other);
        }

        private void OnTriggerEnter(Collider other) => OnTriggerStay(other);

        private void OnTriggerExit(Collider other)
        {
            playerEventChannel.RaiseOnTriggerExit(other);
        }
    }
}