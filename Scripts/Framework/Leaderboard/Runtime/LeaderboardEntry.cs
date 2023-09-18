using System;

namespace Framework.Leaderboard.Runtime
{
    /// <summary>
    /// Represents a single entry in the leaderboard for a level. 
    /// </summary>
    [Serializable]
    public struct LeaderboardEntry: IComparable<LeaderboardEntry>
    {
        /// <summary>
        /// The amount of time it took to complete the level. 
        /// </summary>
        public float completionTime;

        public LeaderboardEntry(float completionTime)
        {
            this.completionTime = completionTime;
        }

        public int CompareTo(LeaderboardEntry other)
        {
            return completionTime.CompareTo(other.completionTime);
        }
    }
}