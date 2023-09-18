using System.Collections.Generic;
using Framework.GlobalDataContainers.Runtime;
using Framework.Leaderboard.Runtime.ScriptableObjects.Channels;
using Framework.Leaderboard.Runtime.ScriptableObjects.DataContainers;
using Framework.Railways.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.DataContainers;
using MEC;
using UnityEngine;

namespace Framework.Leaderboard.Runtime
{
    /// <summary>
    /// Manages the <see cref="LeaderboardData"/> for each level.  
    /// </summary>
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private LeaderboardData leaderboardData;
        
        [Space, SerializeField] private LeaderboardEventChannel leaderboardEventChannel;
        [SerializeField] private PuzzleEventChannel puzzleEventChannel;

        [SerializeField] private float menuDisplayDelay = 3f; 
        [Space, SerializeField] private MenuEventChannel menuEventChannel;
        [SerializeField] private Menu winScreen;
        [SerializeField] private GlobalPlayerData globalPlayerData;

        /// <summary>
        /// The amount of time this level took to clear.
        /// </summary>
        public float completionTime; 

        /// <summary>
        /// The time at which this level was started. 
        /// </summary>
        private float timeStamp; 

        private void Start()
        { 
            timeStamp = Time.time;
        }

        private void OnEnable()
        {
            puzzleEventChannel.OnPuzzleSolved += AddLeaderboardEntry;
            leaderboardEventChannel.OnLeaderboardDataRequested += ReturnLeaderboardData;
            leaderboardEventChannel.OnCompletionTimeRequested += ReturnCompletionTime;
        }

        private void OnDisable()
        {
            puzzleEventChannel.OnPuzzleSolved -= AddLeaderboardEntry;
            leaderboardEventChannel.OnLeaderboardDataRequested -= ReturnLeaderboardData;
            leaderboardEventChannel.OnCompletionTimeRequested -= ReturnCompletionTime;
        }

        /// <summary>
        /// Returns the <see cref="leaderboardData"/>. 
        /// </summary>
        private LeaderboardData ReturnLeaderboardData() => leaderboardData;

        /// <summary>
        /// Returns the <see cref="completionTime"/>. 
        /// </summary>
        private float ReturnCompletionTime() => completionTime;

        private void AddLeaderboardEntry()
        {
            Timing.RunCoroutine(_ShowWinScreen());
            leaderboardData.AddEntry(new LeaderboardEntry(completionTime = Time.time - timeStamp));
        }

        private IEnumerator<float> _ShowWinScreen()
        {
            yield return Timing.WaitForSeconds(menuDisplayDelay); 
            menuEventChannel.RaiseOnOpenRequested(winScreen, globalPlayerData.Player1ControlScheme);
        }
    }
}