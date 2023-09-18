using System;
using Framework.UI.Runtime.PersistentManagement;
using Framework.UI.Runtime.ScriptableObjects.DataContainers;
using UnityEngine;

namespace Framework.UI.Runtime.ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "MenuEventChannel", menuName = "ScriptableObjects/UI/Channels/Menu Event Channel", order = 0)]
    public class MenuEventChannel : ScriptableObject
    {
        /// <summary>
        /// Callback on <see cref="Menu"/> open request. 
        /// </summary>
        public event Action<Menu, string> OnOpenRequested;

        /// <summary>
        /// Callback on <see cref="Menu"/> open. 
        /// </summary>
        public event Action OnOpenCompleted;

        /// <summary>
        /// Callback on <see cref="Menu"/> close.
        /// </summary>
        public event Action OnCloseRequested;

        /// <summary>
        /// Callback on <see cref="Menu"/> populate on the <see cref="MenuController"/>.
        /// </summary>
        public event Action<Menu> OnPopulateRequested;

        /// <summary>
        /// Raises the <see cref="OnOpenRequested"/> event. 
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> to open.</param>
        /// <param name="controlScheme">The control scheme used to navigate this menu.</param>
        public void RaiseOnOpenRequested(Menu menu, string controlScheme)
        { 
            OnOpenRequested?.Invoke(menu, controlScheme);
        }

        /// <summary>
        /// Raises the <see cref="OnOpenCompleted"/> event. 
        /// </summary>
        public void RaiseOnOpenCompleted()
        {
            OnOpenCompleted?.Invoke();
        }

        /// <summary>
        /// Raises the <see cref="OnCloseRequested"/> event.
        /// </summary>
        public void RaiseOnCloseRequested()
        {
            OnCloseRequested?.Invoke();
        }

        /// <summary>
        /// Raises the <see cref="OnPopulateRequested"/> event.
        /// </summary>
        /// <param name="menu">The menu to open.</param>
        public void RaiseOnPopulateRequested(Menu menu)
        {
            OnPopulateRequested?.Invoke(menu);
        }
    }
}