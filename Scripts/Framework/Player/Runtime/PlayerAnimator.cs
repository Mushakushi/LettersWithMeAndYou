using Framework.Player.Runtime.ScriptableObjects.Channels;
using Framework.Railways.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.Channels;
using Mushakushi.Attributes.Runtime;
using UnityEngine;

namespace Framework.Player.Runtime
{
    /// <summary>
    /// Controls an animator based on the <see cref="PlayerMovementController"/> component. 
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="PlayerEventChannel"/> 
        /// </summary>
        [Header("Data"), SerializeField] private PlayerEventChannel playerEventChannel;
        [SerializeField] private MenuEventChannel menuEventChannel;
        [SerializeField] private PuzzleEventChannel puzzleEventChannel;

        /// <summary>
        /// Distance over which to check if the animator is grounded. 
        /// </summary>
        [SerializeField] private float groundCheckDistance = 1f;
        
        /// <summary>
        /// The <see cref="Animator"/> that is being animated. 
        /// </summary>
        [Header("Animation"), ReadOnly, SerializeField] private Animator animator;
        
        private static readonly int ForwardPropertyIndex = Animator.StringToHash("Forward");
        private static readonly int TurnPropertyIndex = Animator.StringToHash("Turn");
        private static readonly int CrouchPropertyIndex = Animator.StringToHash("Crouch");
        private static readonly int GroundPropertyIndex = Animator.StringToHash("OnGround");
        private static readonly int JumpPropertyIndex = Animator.StringToHash("Jump");
        private static readonly int PushPropertyIndex = Animator.StringToHash("Push");
        private const string PushPropertyStateName = "Pushing";
        private static readonly int IsCheeringPropertyIndex = Animator.StringToHash("IsCheering"); 

        /// <summary>
        /// Damping for animations.  
        /// </summary>
        [SerializeField, Min(0.001f)] private float dampTime = 0.4f;
        
        /// <summary>
        /// The minimum player velocity magnitude required to turn and move forward.
        /// </summary>
        [SerializeField, Range(0f, 180f)] private float minMagnitude = 0.05f; 

        /// <summary>
        /// A value between 0-1 representing the extent to which the player is moving at top speed.
        /// (a value of 0 means the player is not moving, while 1 means the player is at top speed.)
        /// </summary>
        [Space, ReadOnly, SerializeField] private float normalizedForwardAmount;

        /// <summary>
        /// How much <see cref="normalizedForwardAmount"/> increases when the player is actively moving. 
        /// </summary>
        [SerializeField, Range(0.001f, 1f)] private float normalizedForwardAmountGain = 0.5f;

        /// <summary>
        /// A value between -1 and 1 representing the extent to which the player is turning 180deg to the left or right. 
        /// (a value of -1 means the player is turning -180deg; 0, 0deg; and, 1, 180deg)
        /// </summary>
        [Space, ReadOnly, SerializeField] private float normalizedTurnAmount; 

        /// <summary>
        /// Multiplier for <see cref="TurnPropertyIndex"/>. 
        /// </summary>
        [SerializeField, Min(0.001f)] private float turnPropertyMultiplier = 4f;
        
        /// <summary>
        /// The turn rate in angles while completely stationary. 
        /// </summary>
        [SerializeField, Min(0.001f)] private float stationaryTurnSpeed = 180f;
        
        /// <summary>
        /// The turn rate in angles while moving at max speed. 
        /// </summary>
        [SerializeField, Min(0.001f)] private float movingTurnSpeed = 275f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            menuEventChannel.OnOpenCompleted += Pause;
            menuEventChannel.OnCloseRequested += Play; 
            playerEventChannel.OnTargetDirectionUpdated += UpdateForwardProperty;
            playerEventChannel.OnTargetDirectionUpdated += UpdateTurnProperty;
            playerEventChannel.OnVelocityUpdated += UpdateJumpProperty;
            playerEventChannel.OnInteract += UpdatePushProperty;
            puzzleEventChannel.OnPuzzleSolved += UpdateIsCheeringProperty;
        }

        private void OnDisable()
        {
            menuEventChannel.OnOpenCompleted -= Pause;
            menuEventChannel.OnCloseRequested -= Play; 
            playerEventChannel.OnTargetDirectionUpdated -= UpdateForwardProperty;
            playerEventChannel.OnTargetDirectionUpdated -= UpdateTurnProperty;
            playerEventChannel.OnVelocityUpdated -= UpdateJumpProperty;
            playerEventChannel.OnInteract -= UpdatePushProperty;
            puzzleEventChannel.OnPuzzleSolved -= UpdateIsCheeringProperty;
        }

        private void Start()
        {
            animator.SetBool(CrouchPropertyIndex, false); // unused 
            Play();
        }

        private void Update()
        {
            UpdateOnGroundProperty();
        }

        /// <summary>
        /// Pauses the <see cref="animator"/>. 
        /// </summary>
        private void Pause()
        {
            animator.speed = 0;
        }

        /// <summary>
        /// Plays the <see cref="animator"/>. 
        /// </summary>
        private void Play()
        {
            animator.speed = 1;
        }

        /// <summary>
        /// Updates the the <see cref="GroundPropertyIndex"/> animation properties. 
        /// </summary>
        private void UpdateOnGroundProperty()
        {
            var isGrounded = CheckGroundStatus();
            animator.applyRootMotion = isGrounded;
            animator.SetBool(GroundPropertyIndex, isGrounded);
        }

        /// <summary>
        /// Updates the <see cref="ForwardPropertyIndex"/> animation property. 
        /// </summary>
        /// <param name="targetDirection">The direction of its movement.</param>
        private void UpdateForwardProperty(Vector3 targetDirection)
        {
            var isMoving = targetDirection.sqrMagnitude > 0;
            normalizedForwardAmount = Mathf.Lerp(normalizedForwardAmount, isMoving ? 1 : 0, normalizedForwardAmountGain);
            var shouldUpdate = normalizedForwardAmount >= minMagnitude;
            animator.SetFloat(ForwardPropertyIndex, shouldUpdate ? normalizedForwardAmount : 0, dampTime, Time.deltaTime);
        }

        /// <summary>
        /// Updates the <see cref="TurnPropertyIndex"/> animation property. 
        /// </summary>
        /// <param name="targetDirection">The direction of its movement.</param>
        private void UpdateTurnProperty(Vector3 targetDirection)
        {
            var cachedTransform = transform;
            var shouldUpdate = targetDirection.sqrMagnitude > 0;
            normalizedTurnAmount = shouldUpdate ? Vector3.SignedAngle(cachedTransform.forward, targetDirection, cachedTransform.up) / 180f : 0;
            normalizedTurnAmount *= turnPropertyMultiplier;
            animator.SetFloat(TurnPropertyIndex, normalizedTurnAmount, dampTime, Time.deltaTime);
            ApplyExtraRotation();
        }

        /// <summary>
        /// Updates the <see cref="JumpPropertyIndex"/> animation property. 
        /// </summary>
        /// <param name="velocity">The player's velocity.</param>
        private void UpdateJumpProperty(Vector3 velocity)
        {
            animator.SetFloat(JumpPropertyIndex, velocity.y);
        }

        /// <summary>
        /// Turn faster.
        /// </summary>
        private void ApplyExtraRotation()
        {
            var turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, normalizedForwardAmount);
            transform.Rotate(0, normalizedTurnAmount * turnSpeed * Time.deltaTime, 0);
        }

        /// <summary>
        /// Fires the <see cref="PushPropertyIndex"/> animation property. 
        /// </summary>
        private void UpdatePushProperty()
        {
            if (!CheckIsPlayingAnimation(PushPropertyStateName)) animator.SetTrigger(PushPropertyIndex);
        }

        /// <summary>
        /// Is the animator playing an animation?
        /// </summary>
        /// <param name="stateName">The name of the state containing the animation.</param>
        /// <returns><see cref="bool"/> Whether or not the animation is playing.</returns>
        private bool CheckIsPlayingAnimation(string stateName)
        {
            var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return currentStateInfo.IsName(stateName) && currentStateInfo.normalizedTime < 1.0f;
        }

        private void OnAnimatorMove()
        {
            playerEventChannel.RaiseOnAnimatorDeltaPositionUpdated(animator.deltaPosition);
        }

        /// <summary>
        /// Is the animator grounded?
        /// </summary>
        private bool CheckGroundStatus()
        {
            return Physics.Raycast(transform.position, Vector3.down * groundCheckDistance);
        }

        /// <summary>
        /// Sets the <see cref="IsCheeringPropertyIndex"/> animation property to true. 
        /// </summary>
        private void UpdateIsCheeringProperty()
        {
            animator.SetBool(IsCheeringPropertyIndex, true);
        }
    }
}