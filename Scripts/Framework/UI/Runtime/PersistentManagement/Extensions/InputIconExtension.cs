using System;
using Framework.GlobalDataContainers.Runtime;
using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using Mushakushi.InputSystem.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.Extensions
{
    /// <summary>
    /// Sets the background image of a <see cref="VisualElement"/> to match with the input icon defined by its name,
    /// in format: $"{action}-{controlScheme OR context}-button*" with context denoting that <see cref="MenuController.activeControlScheme"/> should be used.
    /// * "button" can be any valid string, but it must be seperated by '-'.
    /// </summary>
    [Serializable]
    public class InputIconExtension: MenuExtension<VisualElement>
    {
        [SerializeField] private DeviceDisplayConfiguration deviceDisplayConfiguration;
        [SerializeField] private GlobalPlayerData globalPlayerData;
        [field: SerializeField] public override SelectionRules Selectors { get; protected set; }
        
        public override void Start(VisualElement container)
        {
            foreach (var button in Query(container).ToList()) SetInputIcon(button);
        }
        
        /// <summary>
        /// Sets the input icon to its appropriate icon by name. It is imperative these icons are named in format: $"{action}-{controlScheme}-icon".
        /// </summary>
        /// <param name="visualElement">The input icon.</param>
        private void SetInputIcon(VisualElement visualElement)
        {
            var actionToControlScheme = visualElement.name.Split('-', 3);
            var controlScheme = actionToControlScheme[1] == "context" ? MenuController.activeControlScheme : actionToControlScheme[1];
            var controlPath = DeviceDisplayConfiguration
                .GetActionBindingPath(globalPlayerData.InputActions.FindActionMap("Gameplay").FindAction(actionToControlScheme[0]), controlScheme);
            
            visualElement.style.backgroundImage = new StyleBackground(deviceDisplayConfiguration.GetDeviceBindingIcon(controlPath));
        }
    }
}