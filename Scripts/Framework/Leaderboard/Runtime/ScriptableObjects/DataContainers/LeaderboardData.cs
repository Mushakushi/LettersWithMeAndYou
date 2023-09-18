using System.Collections.Generic;
using UnityEngine;

namespace Framework.Leaderboard.Runtime.ScriptableObjects.DataContainers
{
    [CreateAssetMenu(fileName = "Leaderboard", menuName = "ScriptableObjects/Leaderboard/Data Containers/Leaderboard Data", order = 0)]
    public class LeaderboardData : ScriptableObject
    {
        /// <summary>
        /// The <see cref="LeaderboardEntry"/>s. 
        /// </summary>
        [field: SerializeField] public List<LeaderboardEntry> Entries { get; private set; }

        /// <summary>
        /// Adds an entry to <see cref="Entries"/> and automatically sorts. 
        /// </summary>
        /// <param name="entry">The <see cref="LeaderboardEntry"/> to add.</param>
        public void AddEntry(LeaderboardEntry entry)
        {
            var index = Entries.BinarySearch(entry);
            if (index < 0) index = ~index; 
            Entries.Insert(index, entry);
        }
    }
}