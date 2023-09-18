using System;
using Framework.SceneManagement.Runtime.ScriptableObjects.Channels;
using Framework.UI.Runtime.PersistentManagement.ExtensionFramework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Framework.UI.Runtime.PersistentManagement.Extensions
{
    /// <summary>
    /// Restarts scene on button click;
    /// </summary>
    [Serializable]
    public class RestartSceneButtonExtension: MenuEventExtension<Button>
    {
        [FormerlySerializedAs("sceneLoadEventChannel")] [SerializeField] private SceneEventChannel sceneEventChannel;
        [field: SerializeField] public override SelectionRules Selectors { get; protected set; }
        protected override Action Subscribe(Button visualElement)
        {
            visualElement.clicked += sceneEventChannel.RaiseOnRestartRequested;
            return () => visualElement.clicked -= sceneEventChannel.RaiseOnRestartRequested;
        }
    }
}