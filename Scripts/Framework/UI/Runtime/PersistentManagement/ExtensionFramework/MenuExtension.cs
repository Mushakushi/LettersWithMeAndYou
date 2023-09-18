using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.ExtensionFramework {     
    
    /// <summary>
    /// An extension to <see cref="MenuController"/>. 
    /// </summary>
    /// <typeparam name="T">The <see cref="VisualElement"/>s that are queried for.</typeparam>
    public abstract class MenuExtension<T>: IMenuExtension       
        where T: VisualElement
    {
        /// <summary>
        /// The name or id used to query the <see cref="T"/> that is being operated on. 
        /// </summary>
        public abstract SelectionRules Selectors { get; protected set; }

        /// <summary>
        /// Returns the <see cref="T"/> elements.
        /// </summary>
        /// <param name="container">The <see cref="VisualElement"/> container of <see cref="T"/>.</param>
        /// <returns>The <see cref="UQueryBuilder{T}"/> of <see cref="T"/> this menu extension operates on.</returns>
        protected UQueryBuilder<T> Query(VisualElement container)
        {
            if (Selectors.name != string.Empty)
            {
                return Selectors.classes.Length > 0 
                    ? container.Query<T>(Selectors.name, Selectors.classes) 
                    : container.Query<T>(Selectors.name);
            }
            return Selectors.classes.Length > 0 
                ? container.Query<T>(classes: Selectors.classes) 
                : container.Query<T>();
        }

        /// <summary>
        /// Initializes the extension. 
        /// </summary>
        /// <param name="container">The <see cref="VisualElement"/> container of <see cref="T"/>.</param>
        public abstract void Start(VisualElement container);
    } 
}