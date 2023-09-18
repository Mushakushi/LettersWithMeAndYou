using System;
using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using Framework.UI.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.DataContainers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.Extensions
{
    /// <summary>
    /// Connects multiple menus together with one extension. 
    /// </summary>
    [Serializable]
    public class MenuConnectionButtonExtension: MenuEventExtensionDictionary<Button, string, Menu>
    {
        [SerializeField] private MenuEventChannel menuEventChannel;
        
        [field: SerializeField] public override SelectionRules Selectors { get; protected set; }
        
        protected override Action Subscribe(Button visualElement)
        {
            void OnSubscribe() => PopulateMenu(visualElement.name);
            visualElement.clicked += OnSubscribe;
            return () => visualElement.clicked -= OnSubscribe;
        }

        /// <summary>
        /// <see cref="PopulateMenu"/> by name.
        /// </summary>
        /// <param name="visualElementName"></param>
        private void PopulateMenu(string visualElementName)
        {
            if (elementDictionary.TryGetValue(visualElementName, out var menu))
                menuEventChannel.RaiseOnPopulateRequested(menu);
            else Debug.LogError($"Menu requested a connection with Visual Element name key '{visualElementName}' but it does not exist!");
        }
    }
}