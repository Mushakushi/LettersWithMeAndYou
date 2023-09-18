using System;
using Framework.SceneManagement.Runtime.ScriptableObjects.Channels;
using Framework.SceneManagement.Runtime.ScriptableObjects.DataContainers;
using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.Extensions
{
    [Serializable]
    public class SceneLoadButtonExtension: MenuEventExtensionDictionary<Button, string, SceneData>
    {
        /// <summary>
        /// The <see cref="SceneEventChannel"/>.
        /// </summary>
        [FormerlySerializedAs("sceneLoadEventChannel")] [SerializeField] private SceneEventChannel sceneEventChannel;
        
        [field: SerializeField] public override SelectionRules Selectors { get; protected set; }

        protected override Action Subscribe(Button visualElement)
        {
            void OnSubscribe() => RequestSceneLoad(visualElement.name);
            visualElement.clicked += OnSubscribe;
            return () => visualElement.clicked -= OnSubscribe;
        }
        
        /// <summary>
        /// Requests scene load. 
        /// </summary>
        /// <param name="visualElementName"></param>
        private void RequestSceneLoad(string visualElementName)
        {
            if(elementDictionary.TryGetValue(visualElementName, out var sceneContext))
                sceneEventChannel.RaiseOnLoadRequested(sceneContext);
            else Debug.LogError($"Button with name '${visualElementName}' was requested to load a scene but was not added to the LoadSceneButtons!");
        }
    }
}