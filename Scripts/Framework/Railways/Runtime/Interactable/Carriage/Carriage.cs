using System;
using Framework.Railways.Runtime.Paths.PathBehaviour;
using Framework.UI.Runtime.ScriptableObjects.Channels;
using Mushakushi.Attributes.Runtime;
using Mushakushi.InternalDebug.Runtime;
using Mushakushi.Splines.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

namespace Framework.Railways.Runtime.Interactable.Carriage
{
    /// <summary>
    /// Moves along a <see cref="Spline"/>. 
    /// </summary>
    /// <remarks>
    /// Containing all logic in one script reduces boilerplate significantly. 
    /// </remarks>
    [DisallowMultipleComponent]
    public class Carriage: SplineWalker, IRailwayInteractable
    {
        [Header("Data"), SerializeField] private MenuEventChannel menuEventChannel;
        
        [Header("Movement"), Rename("Initial Railway Path"), SerializeField] private RailwayPath railwayPath;

        /// <summary>
        /// How fast interpolation occurs. 
        /// </summary>
        [SerializeField] private float timeMultiplier; 

        /// <summary>
        /// The normalized interpolation on the <see cref="SplineWalker.Container"/>. 
        /// </summary>
        [Range(0,1)] public float normalizedTime;
        
        [SerializeField] private float rotationSpeed = 200;

        [SerializeField] private int splineTransitionSteps = 10;

        /// <summary>
        /// Whether or not the <see cref="Play"/> on <see cref="Awake"/>.
        /// </summary>
        [SerializeField] private bool playOnAwake; 

        /// <summary>
        /// Whether or not can't move. 
        /// </summary>
        private bool isPaused;

        /// <summary>
        /// Whether or not <see cref="isPaused"/> was true when <see cref="MenuEventChannel.OnOpenCompleted"/> was called. 
        /// </summary>
        private bool wasPausedOnMenu;
        
        /// <summary>
        /// The letter to display. 
        /// </summary>
        /// <remarks>
        /// Ideally, this should be contained in a separate script, but not doing so allows me
        /// to avoid boilerplate such as creating a channel for each carriage per level. 
        /// </remarks>
        [field: Header("Carriage Letter"),  SerializeField] public char Letter { get; private set; }

        /// <summary>
        /// The <see cref="TMP_Text"/> to display the <see cref="Letter"/> within. 
        /// </summary>
        [SerializeField] private TMP_Text letterTextComponent;

        /// <summary>
        /// The <see cref="Collider"/>. 
        /// </summary>
        [Header("Collisions"), SerializeField] private new Collider collider;

        /// <summary>
        /// The <see cref="Outline"/> around this used to describe the state visually. 
        /// </summary>
        [SerializeField] private Outline outline;

        /// <summary>
        /// The color to show when should be interacted with. 
        /// </summary>
        [SerializeField] private Color activeColor = Color.white;

        /// <summary>
        /// The color to show when shouldn't be interacted with. 
        /// </summary>
        [SerializeField] private Color inactiveColor = Color.red; 
        
        /// <summary>
        /// Callback on play requested. 
        /// </summary>
        public event Action OnPlayRequested;
        
        private void Awake()
        {
            SetColor(true);
            InitializeRailwayPath();
            InitializeCarriageLetter();
            
            if (playOnAwake) Play();
            else Pause();
        }

        /// <summary>
        /// Sets the outline of this appropriately based on <see cref="isActive"/>. 
        /// </summary>
        /// <param name="isActive">Whether or not this should be interacted with.</param>
        public void SetColor(bool isActive)
        {
            if (!outline) return;
            outline.OutlineColor = isActive ? activeColor : inactiveColor;
        }

        private void InitializeRailwayPath()
        {
            if (!railwayPath)
            {
                InternalDebug.LogError("There is no railway path assigned!", this);
                return;
            }
            Container = railwayPath.Container;
            AlignToSpline(normalizedTime, 360f);
            UpdatePosition(normalizedTime);
        }

        private void InitializeCarriageLetter()
        {
            if (letterTextComponent)letterTextComponent.text = $"{Letter}";
            else InternalDebug.LogWarning("The letter text component is not assigned!", this);
            
            if (collider) collider.isTrigger = true;
            else InternalDebug.LogWarning("There is no collider attached to this carriage!", this);
        }
        
        private void OnEnable()
        {
            if (menuEventChannel == null) return;
            menuEventChannel.OnOpenCompleted += Pause;
            menuEventChannel.OnCloseRequested += TryPlay;
        }
        
        private void OnDisable()
        {
            if (menuEventChannel == null) return; 
            menuEventChannel.OnOpenCompleted -= Pause;
            menuEventChannel.OnCloseRequested -= TryPlay;
        }

        /// <summary>
        /// Pauses.
        /// </summary>
        public void Pause()
        {
            wasPausedOnMenu = isPaused;
            isPaused = true;
        }

        /// <summary>
        /// Plays.
        /// </summary>
        public void Play()
        {
            DoRailwayPathBehaviour();
            isPaused = false; 
        }

        /// <summary>
        /// Plays if wasn't paused on menu. 
        /// </summary>
        private void TryPlay()
        {
            if (!wasPausedOnMenu) Play();
        }

        /// <summary>
        /// What happens if playing is attempted, but cannot play. 
        /// </summary>
        public void OnPlayRequestFailed()
        {
            Debug.Log("failed");
        }

        private void Update()
        {
            AlignToSpline(normalizedTime, rotationSpeed * Time.deltaTime);
            UpdatePosition(normalizedTime);
            if (isAnimating || isPaused) return;
            switch (normalizedTime)
            {
                case >= 1f when timeMultiplier > 0:
                case <= 0f when timeMultiplier < 0:
                    if (playOnAwake) 
                    {
                        railwayPath.ReachEnd(this); // todo - just make a SplineWalker component
                        normalizedTime = 0;
                    }
                    else Pause(); //  p a u s e .
                    break;
            }
            normalizedTime += Time.deltaTime * timeMultiplier;
        }

        /// <summary>
        /// Rotates from a rotation to a target rotation without attempting to rotate over 90 degrees.
        /// When the rotation is from one rotation to anther target is over 90 degrees, it will rotate from the inverse of
        /// the rotation to the target. 
        /// </summary>
        /// <param name="rotation">The rotation to rotate from.</param>
        /// <param name="targetRotation">The rotation to rotate to.</param>
        /// <param name="maxDegreesDelta">The angular step to rotate between the two rotations.</param>
        /// <returns><see cref="Quaternion"/> a rotation from two rotations by the <see cref="maxDegreesDelta"/>.</returns>
        protected override Quaternion RotateTowards(Quaternion rotation, Quaternion targetRotation, float maxDegreesDelta)
        {
            var forward = Quaternion.Angle(rotation, targetRotation) <= 90f ? rotation : Quaternion.Inverse(rotation);
            return Quaternion.RotateTowards(forward, targetRotation, maxDegreesDelta);
        }

        /// <summary>
        /// Performs the <see cref="RailwayPath.ReachStart"/> and <see cref="RailwayPath.ReachEnd"/> actions
        /// when at appropriate points in the <see cref="SplineAnimate.NormalizedTime"/>. 
        /// </summary>
        private void DoRailwayPathBehaviour()
        {
            switch (normalizedTime)
            {
                case >= 1f when timeMultiplier > 0:
                    railwayPath.ReachEnd(this);
                    normalizedTime = 0;
                    break;
                case <= 0f when timeMultiplier < 0:
                    railwayPath.ReachStart(this);
                    normalizedTime = 1;
                    break;
            }
        }

        /// <summary>
        /// Changes the <see cref="SplineAnimate.Container"/> over time while maintaining the state of <see cref="SplineAnimate.IsPlaying"/>.
        /// </summary>
        /// <param name="target">The <see cref="RailwayPath"/>.</param>
        /// <param name="t">A value between 0 and 1 representing a percentage of entire spline to start at.</param>
        public void SetRailwayPath(RailwayPath target, int t)
        {
            railwayPath = target;
            ChangeSpline(target.Container, splineTransitionSteps, t);
        }

        public void OnInteract()
        {
            OnPlayRequested?.Invoke();
        }
    }
}