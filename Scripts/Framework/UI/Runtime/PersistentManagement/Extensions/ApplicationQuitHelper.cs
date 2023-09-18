using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.UI.Runtime.PersistentManagement.Extensions
{
    public class ApplicationQuitHelper: MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.sceneLoaded += DestroyOnSceneLoad;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= DestroyOnSceneLoad;
        }

        /// <summary>
        /// Quits the application when this extension <see cref="MenuEventExtension{T}.IsInitialized"/>, bound to the keyboard escape key. 
        /// </summary>
        private void OnGUI()
        {
            if (Event.current.Equals(Event.KeyboardEvent("escape"))) ApplicationQuitButtonExtension.QuitApplication();
        }

        private void DestroyOnSceneLoad(Scene _, LoadSceneMode __) => Destroy(gameObject);
    }
}