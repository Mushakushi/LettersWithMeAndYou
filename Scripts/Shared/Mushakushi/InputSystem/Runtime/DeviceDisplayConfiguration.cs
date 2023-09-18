using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mushakushi.InputSystem.Runtime
{
    /// <summary>
    /// Manages all <see cref="DeviceDisplaySettings"/>. 
    /// </summary>
    [CreateAssetMenu(fileName = "DeviceDisplayConfiguration", menuName = "ScriptableObjects/Input/Device Display Configuration", order = 0)]
    public class DeviceDisplayConfiguration : ScriptableObject
    {
        /// <summary>
        /// Maps each device's raw path to their <see cref="DeviceDisplaySettings"/>. 
        /// </summary>
        [Serializable]
        public sealed class DeviceRawPathToDeviceDisplaySettings: SerializableDictionaryBase<string, DeviceDisplaySettings>{}

        /// <summary>
        /// The <see cref="DeviceRawPathToDeviceDisplaySettings"/>. 
        /// </summary>
        [field: Header("Device Sets"), Tooltip("Maps each device raw path to their DeviceDisplaySettings."), SerializeField]
        public DeviceRawPathToDeviceDisplaySettings DeviceSets { get; private set; }

        /// <summary>
        /// Gets a suitable device name of the first device on a <see cref="PlayerInput"/> component
        /// with respect to the <see cref="DeviceSets"/>. 
        /// </summary>
        /// <returns><see cref="string"/> The device name.</returns>
        public string GetDeviceName(PlayerInput playerInput)
        {
            return DeviceSets[playerInput.devices[0].ToString()]?.DisplayName;
        }

        /// <summary>
        /// Get the input icon of a raw control path with respect to the <see cref="DeviceSets"/>.
        /// </summary>
        /// <returns><see cref="Texture"/> The input icon.</returns>
        public Texture2D GetDeviceBindingIcon(string rawControlPath)
        {
            var rawControlPaths = rawControlPath.Split('/', 2);
            return DeviceSets[rawControlPaths[0]]?.Icons[rawControlPaths[1]];
        }

        private static string GetActionBindingPath(InputAction action, int bindingIndex, InputBinding.DisplayStringOptions options = 0)
        {
            if (bindingIndex < 0) throw new IndexOutOfRangeException("Binding does not exist!");
            action.GetBindingDisplayString(bindingIndex, out var deviceLayoutName, out var controlPath, options);
            return $"<{deviceLayoutName}>/{controlPath}";
        }

        /// <summary>
        /// Return the raw binding path of an <see cref="InputAction"/> by a control scheme. 
        /// </summary>
        /// <param name="action">The <see cref="InputAction"/> to get a raw binding path for.</param>
        /// <param name="controlScheme">The control scheme.</param>
        /// <param name="options">Optional set of formatting flags.</param>
        /// <returns><see cref="string"/> The raw binding path.</returns>
        public static string GetActionBindingPath(InputAction action, string controlScheme, InputBinding.DisplayStringOptions options = 0)
        {
            return GetActionBindingPath(action, action.GetBindingIndex(InputBinding.MaskByGroup(controlScheme)), options);
        }
    }
}