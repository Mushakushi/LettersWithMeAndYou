using Framework.SceneManagement.Runtime.ScriptableObjects.Channels;
using UnityEngine;
using UnityEngine.UIElements;
using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using Framework.UI.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.ScriptableObjects.DataContainers;
using JetBrains.Annotations;
using Mushakushi.InternalDebug.Runtime;

namespace Framework.UI.Runtime.PersistentManagement
{
    /// <summary>
    /// Controls a menu. 
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        /// <summary>
        /// The class name of the initial focused element. 
        /// </summary>
        [SerializeField] private string initialFocusedElementClassName = "first-focused";

        /// <summary>
        /// The name of the <see cref="VisualElement"/> that contains all menus. 
        /// </summary>
        /// <remarks>
        /// This element is cleared and populated with <see cref="VisualTreeAsset"/> menus during runtime.
        /// </remarks>
        [SerializeField] private string mainContainerName = "main-wrapper";
        
        /// <summary>
        /// The <see cref="menuEventChannel"/>. 
        /// </summary>
        [Header("Channels"), SerializeField]
        private MenuEventChannel menuEventChannel;

        /// <summary>
        /// The <see cref="SceneEventChannel"/>.
        /// </summary>
        [SerializeField] private SceneEventChannel sceneEventChannel;

        /// <summary>
        /// The control scheme that is navigating the menu. 
        /// </summary>
        // todo - change the controls of UI player input 
        public static string activeControlScheme;

        /// <summary>
        /// The <see cref="UIDocument"/> that displays the menu.  
        /// </summary>
        [Header("UI"), SerializeField] 
        private UIDocument document;
        
        /// <summary>
        /// The <see cref="IMenuExtension"/>(s) applied to the whole menu. 
        /// </summary>
        [field: SerializeReference, SubclassSelector] private IMenuExtension[] GlobalExtensions { get; [UsedImplicitly] set; }

        private void OnEnable()
        {
            menuEventChannel.OnOpenRequested += Open;
            menuEventChannel.OnCloseRequested += Close;
            sceneEventChannel.OnLoadingScreenDisplayed += Close;
            menuEventChannel.OnPopulateRequested += Populate;
        }
        
        private void OnDisable()
        {
            menuEventChannel.OnOpenRequested -= Open;
            menuEventChannel.OnCloseRequested -= Close;
            sceneEventChannel.OnLoadingScreenDisplayed -= Close;
            menuEventChannel.OnPopulateRequested -= Populate;
        }

        private void Start()
        {
            Close();
        }

        /// <summary>
        /// Shows a menu. 
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/></param>
        /// <param name="controlScheme">The control scheme used to navigate this menu.</param>
        private void Open(Menu menu, string controlScheme)
        {
            activeControlScheme = controlScheme;
            Populate(menu);
            document.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            menuEventChannel.RaiseOnOpenCompleted();
        }

        /// <summary>
        /// Hides the menu. 
        /// </summary>
        private void Close()
        {
            document.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None); 
        }

        /// <summary>
        /// Populates a <see cref="Menu"/>.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> to populate.</param>
        private void Populate(Menu menu)
        {
            var mainContainer = document.rootVisualElement.Q<VisualElement>(mainContainerName);
            if (mainContainer == null)
            {
                InternalDebug.LogError($"UIDocument '${menu.Asset} has no Visual Element with name '{mainContainerName}'! " +
                                       "The menu will not be populated.");
                return;
            }
            
            mainContainer.Clear();
            menu.Asset.CloneTree(mainContainer);
            FocusMenu(mainContainer);
            
            foreach (var extension in GlobalExtensions) extension.Start(document.rootVisualElement);
            foreach (var extension in menu.Extensions) extension.Start(mainContainer);
            menu.MenuConnections.Start(mainContainer);
        }

        /// <summary>
        /// Focuses the the Visual Element with class name <see cref="initialFocusedElementClassName"/>.
        /// </summary>
        /// <param name="initialFocusedElementContainer">
        /// The <see cref="VisualElement"/> that contains the <see cref="initialFocusedElementClassName"/>.
        /// </param>
        private void FocusMenu(VisualElement initialFocusedElementContainer)
        {
            var initialFocusedElement = initialFocusedElementContainer.Q<VisualElement>(className: initialFocusedElementClassName);
            if (initialFocusedElement == null)
            {
                InternalDebug.LogWarning($"VisualElement '{initialFocusedElementContainer}' " +
                                         $"has no Visual Element with class name '{initialFocusedElementClassName}'!");
            }
            else initialFocusedElement.Focus();
        }
    }
}