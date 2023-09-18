using System;
using Framework.Leaderboard.Runtime.ScriptableObjects.DataContainers;
using UnityEngine;

namespace Framework.Leaderboard.Runtime.ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "LeaderboardEventChannel", menuName = "ScriptableObjects/Leaderboard/Channels/Leaderboard Event Channel", order = 0)]
    public class LeaderboardEventChannel : ScriptableObject
    {
        /// <summary>
        /// Callback on the currently used <see cref="LeaderboardData"/> changed. 
        /// </summary>
        public event Func<LeaderboardData> OnLeaderboardDataRequested;

        /// <summary>
        /// Callback on requesting the level completion time. 
        /// </summary>
        public event Func<float> OnCompletionTimeRequested; 

        /// <summary>
        /// Raises the <see cref="OnLeaderboardDataRequested"/> event. 
        /// </summary>
        /// <returns><see cref="LeaderboardData"/> The current <see cref="LeaderboardData"/>.</returns>
        public LeaderboardData RaiseOnLeaderboardDataRequested()
        {
            return OnLeaderboardDataRequested?.Invoke();
        }

        /// <summary>
        /// Raises the <see cref="OnCompletionTimeRequested"/> event. 
        /// </summary>
        /// <returns><see cref="float"/> The amount of time it took to complete the level.</returns>
        /// <exception cref="Exception">The completion time was not returned.</exception>
        public float RaiseOnCompletionTimeRequested()
        {
            if (OnCompletionTimeRequested != null) return OnCompletionTimeRequested.Invoke();
            throw new Exception("Could not return the completion time!");
        }
    }
}