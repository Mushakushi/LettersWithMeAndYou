using System;
using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.ExtensionFramework
{
    /// <summary>
    /// Adds custom functionality to <see cref="MenuController"/> via event subscription. 
    /// </summary>
    /// <typeparam name="T">The <see cref="VisualElement"/> this menu extension operates on.</typeparam>
    public abstract class MenuEventExtension<T>: MenuExtension<T>
        where T: VisualElement
    {
        /// <summary>
        /// True if <see cref="Start"/> has ran, false otherwise or if the event has been unsubscribed.
        /// </summary>
        protected bool IsInitialized { get; private set; }
        
        /// <summary>
        /// How to subscribe to the event(s). 
        /// </summary>
        /// <param name="visualElement">The <see cref="VisualElement"/> to subscribe.</param>
        /// <returns><see cref="Action"/> How to unsubscribe to the event(s).</returns>
        protected abstract Action Subscribe(T visualElement);

        /// <summary>
        /// Subscribes to each <see cref="T"/> in the <see cref="MenuExtension{T}.Query"/>. 
        /// Unsubscription is handled internally. 
        /// </summary>
        /// <param name="container"></param>
        /// <seealso cref="Subscribe"/>
        public sealed override void Start(VisualElement container)
        {
            var query = Query(container);
            foreach (var element in query.ToList())
            {
                var unsubscribe = Subscribe(element);
                RegisterUnsubscription(new OnDetachFromPanelArgs(element, unsubscribe));
            }

            IsInitialized = true;
        }

        /// <summary>
        /// Registers the <see cref="DetachFromPanelEvent"/> event on the <see cref="VisualElement"/> to call <see cref="OnMenuDetachFromPanel"/>.
        /// </summary>
        /// <param name="args">The <see cref="OnDetachFromPanelArgs"/> containing the <see cref="VisualElement"/>.</param>
        private void RegisterUnsubscription(OnDetachFromPanelArgs args)
        {
            args.visualElement.RegisterCallback<DetachFromPanelEvent, OnDetachFromPanelArgs>(OnMenuDetachFromPanel, args);
        }

        /// <summary>
        /// Unsubscribes callbacks as defined by the <see cref="OnDetachFromPanelArgs"/> on <see cref="DetachFromPanelEvent"/>. 
        /// </summary>
        private void OnMenuDetachFromPanel(DetachFromPanelEvent _, OnDetachFromPanelArgs args)
        {
            args.visualElement.UnregisterCallback<DetachFromPanelEvent, OnDetachFromPanelArgs>(OnMenuDetachFromPanel);
            args.unsubscribe.Invoke();

            IsInitialized = false;
        }
        
        /// <summary>
        /// Callback args for <see cref="MenuEventExtension{T}.OnMenuDetachFromPanel"/>. 
        /// </summary>
        private struct OnDetachFromPanelArgs
        {
            /// <summary>
            /// The <see cref="VisualElement"/> that is having its event registered. 
            /// </summary>
            public readonly T visualElement;
            
            /// <summary>
            /// The callback that unsubscribes all of <see cref="visualElement"/>'s subscriptions. 
            /// </summary>
            public readonly Action unsubscribe;
            
            public OnDetachFromPanelArgs(T visualElement, Action unsubscribe)
            {
                this.visualElement = visualElement;
                this.unsubscribe = unsubscribe;
            }
        }
    }
}