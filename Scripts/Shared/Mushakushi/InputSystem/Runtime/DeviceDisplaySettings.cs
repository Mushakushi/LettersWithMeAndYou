using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Mushakushi.InputSystem.Runtime
{
    /// <summary>
    /// Class for determining which input icon represents a binding per device type. 
    /// </summary>
    [CreateAssetMenu(fileName = "DeviceDisplaySettings", menuName = "ScriptableObjects/Input/Device Display Settings", order = 0)]
    public class DeviceDisplaySettings : ScriptableObject
    {
        /// <summary>
        /// The device's name. 
        /// </summary>
        [field: Header("Display Name"), Tooltip("The device's name."), SerializeField]
        public string DisplayName { get; private set; }
        
        /// <summary>
        /// A local raw binding path (e.g. "&lt;Mouse&gt;/leftButton" becomes "leftButton") to input icon map for a device. 
        /// </summary>
        [System.Serializable] 
        public sealed class InputRawBindingPathToIcon: SerializableDictionaryBase<string, Texture2D>{}
        
        /// <summary>
        /// Optional input icons to use for the device.
        /// </summary>
        [field: Header("Input Icons"), Tooltip("Optional custom input icons to use for the device."), SerializeField]
        public InputRawBindingPathToIcon Icons { get; private set; }
    }
}