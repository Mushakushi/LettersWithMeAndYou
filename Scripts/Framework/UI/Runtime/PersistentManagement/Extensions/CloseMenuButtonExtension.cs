using System;
using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using Framework.UI.Runtime.ScriptableObjects.Channels;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.Extensions
{
    /// <summary>
    /// Requests to close the menu on click.
    /// </summary>
    [Serializable]
    public class CloseMenuButtonExtension: MenuEventExtension<Button>
    {
        [SerializeField] private MenuEventChannel menuEventChannel;
        [field: SerializeField] public override SelectionRules Selectors { get; protected set; }
        protected override Action Subscribe(Button visualElement)
        {
            visualElement.clicked += menuEventChannel.RaiseOnCloseRequested;
            return () => visualElement.clicked -= menuEventChannel.RaiseOnCloseRequested;
        }
    }
}