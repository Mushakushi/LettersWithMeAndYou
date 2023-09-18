using System;
using Framework.Leaderboard.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using Mushakushi.UIToolkit.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.Leaderboard.Runtime.MenuExtensions
{
    /// <summary>
    /// Populates a leaderboard with data from a <see cref="Leaderboard"/>. 
    /// </summary>
    [Serializable]
    public class PopulateLeaderboardExtension: MenuExtension<LabelAutoFit>
    {
        [SerializeField] private LeaderboardEventChannel leaderboardEventChannel;
        [field: SerializeField] public override SelectionRules Selectors { get; protected set; }
        
        /// <summary>
        /// Converts seconds to a nicely formatted string. 
        /// </summary>
        public static string ConvertToMinutes(int seconds)
        {
            var minutes = seconds / 60;
            return $"{(minutes > 0 ? $"{minutes} minutes " : "")}{seconds % 60} seconds";
        }
        
        public override void Start(VisualElement container)
        {
            var leaderboardData = leaderboardEventChannel.RaiseOnLeaderboardDataRequested();
            if (leaderboardData == null) return; 
            var entries = Query(container).ToList();
            var minCount = Mathf.Min(entries.Count, leaderboardData.Entries.Count);
            for (var i = 0; i < minCount; i++)
            {
                entries[i].text = ConvertToMinutes((int)leaderboardData.Entries[i].completionTime);
            }
        }
    }
}