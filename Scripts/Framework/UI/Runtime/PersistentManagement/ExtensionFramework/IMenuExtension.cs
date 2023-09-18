using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.ExtensionFramework
{
    public interface IMenuExtension
    {
        /// <summary>
        /// The name or id used to query the <see cref="VisualElement"/>(s) being operated on. 
        /// </summary>
        public SelectionRules Selectors { get; }

        /// <summary>
        /// Initializes the extension. 
        /// </summary>
        /// <param name="container">The <see cref="VisualElement"/> container of the <see cref="VisualElement"/>(s) being operated on.</param>
        public void Start(VisualElement container);
    }
}