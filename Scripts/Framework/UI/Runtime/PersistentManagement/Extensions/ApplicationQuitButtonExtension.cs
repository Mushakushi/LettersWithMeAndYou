using System;
using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.Extensions
{
    [Serializable]
    public class ApplicationQuitButtonExtension: MenuEventExtension<Button>
    {
        [field: SerializeField] public override SelectionRules Selectors { get; protected set; }

        private GameObject applicationQuitHelper;

        protected override Action Subscribe(Button visualElement)
        {
            visualElement.clicked += QuitApplication;

            applicationQuitHelper = new GameObject("Keyboard Application Quit Helper", typeof(ApplicationQuitHelper));
            
            return () =>
            {
                visualElement.clicked -= QuitApplication;
                UnityEngine.Object.Destroy(applicationQuitHelper);
                applicationQuitHelper = null;
            };
        }

        /// <summary>
        /// Quits the application in build mode, and stops playing in the Unity Editor.
        /// </summary>
        public static void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}