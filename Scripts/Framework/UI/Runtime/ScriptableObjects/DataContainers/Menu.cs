using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using Framework.UI.Runtime.PersistentManagement.Extensions;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.UI.Runtime.ScriptableObjects.DataContainers
{
    /// <summary>
    /// A submenu. 
    /// </summary>
    /// <remarks>
    /// Using a <see cref="ScriptableObject"/> instead of struct to prevent shallow circular references which exceed Unity's serialization depth.
    /// </remarks>
    // todo - create custom inspector (should highlight unset fields instead of Debug.Log-ing them).
    [UsedImplicitly, CreateAssetMenu(fileName = "Menu", menuName = "ScriptableObjects/UI/Data Containers/Menu", order = 0)]
    public class Menu: ScriptableObject
    {
        /// <summary>
        /// The <see cref="VisualTreeAsset"/> of the submenu. 
        /// </summary>
        [field: SerializeField] public VisualTreeAsset Asset { get; private set; }

        /// <summary>
        /// The <see cref="IMenuExtension"/>(s). 
        /// </summary>
        [field: SerializeReference, SubclassSelector] public IMenuExtension[] Extensions { get; [UsedImplicitly] protected set; }

        /// <summary>
        /// The <see cref="Menu"/>(s) that can be navigated to via this.
        /// </summary>
        /// <remarks>
        /// Separate by design from <see cref="Extensions"/> on the basis that most <see cref="Menu"/>s include them.
        /// </remarks>
        [field: SerializeField] public MenuConnectionButtonExtension MenuConnections { get; private set; }
    }
}