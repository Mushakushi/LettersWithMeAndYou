using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.ExtensionFramework
{
    [Serializable]
    public abstract class MenuEventExtensionDictionary<TVisualElement, TKey, TValue>: MenuEventExtension<TVisualElement>
        where TVisualElement: VisualElement
    {
        /// <summary>
        /// Maps a Button's UXML name to the scene context it wants to load.
        /// </summary>
        [SerializeField] public ElementDictionary elementDictionary;
        
        [Serializable] public sealed class ElementDictionary: SerializableDictionaryBase<TKey, TValue>{}
    }
}