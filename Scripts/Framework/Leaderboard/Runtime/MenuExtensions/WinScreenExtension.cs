using System;
using Framework.Leaderboard.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using Mushakushi.UIToolkit.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.Leaderboard.Runtime.MenuExtensions
{
    /// <summary>
    /// Just updates the text to display your current time in the win screen. 
    /// </summary>
    [Serializable]
    public class WinScreenExtension: MenuExtension<LabelAutoFit>
    {
        [SerializeField] private LeaderboardEventChannel leaderboardEventChannel;
        [field: SerializeField] public override SelectionRules Selectors { get; protected set; }
        public override void Start(VisualElement container)
        {
            var time = leaderboardEventChannel.RaiseOnCompletionTimeRequested();
            var message = $"Congrats! We did it in {PopulateLeaderboardExtension.ConvertToMinutes((int)time)}!";
            Query(container).First().text = message; 
        }
    }
}