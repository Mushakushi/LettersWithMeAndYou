using System.Linq;
using Framework.GlobalDataContainers.Runtime;
using UnityEditor;
using UnityEngine.InputSystem;

namespace Framework.GlobalDataContainers.Editor
{
    /// <summary>
    /// Draws the <see cref="GlobalPlayerData"/>.
    /// </summary>
    [CustomEditor(typeof(GlobalPlayerData))]
    public class GlobalPlayerDataEditor : UnityEditor.Editor
    {
        private SerializedProperty player1Prefab;
        private SerializedProperty player2Prefab;
        private SerializedProperty inputActions;
        private SerializedProperty player1ControlScheme;
        private SerializedProperty player1ControlSchemeIndex;
        private SerializedProperty player2ControlScheme;
        private SerializedProperty player2ControlSchemeIndex;

        private void OnEnable()
        {
            player1Prefab = serializedObject.FindProperty("player1Prefab");
            player2Prefab = serializedObject.FindProperty("player2Prefab");
            inputActions = serializedObject.FindProperty("inputActions");
            player1ControlScheme = serializedObject.FindProperty("player1ControlScheme");
            player1ControlSchemeIndex = serializedObject.FindProperty("player1ControlSchemeIndex");
            player2ControlScheme = serializedObject.FindProperty("player2ControlScheme");
            player2ControlSchemeIndex = serializedObject.FindProperty("player2ControlSchemeIndex");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.ObjectField(player1Prefab);
            EditorGUILayout.ObjectField(player2Prefab);
            EditorGUILayout.ObjectField(inputActions);

            var actionAssetBoxedValue = inputActions.boxedValue;
            if (actionAssetBoxedValue == null)
            {
                EditorGUILayout.HelpBox("Please select a valid Input Actions asset.", MessageType.Error);
                serializedObject.ApplyModifiedProperties();
                return;
            }
            var controlSchemes = ((InputActionAsset)actionAssetBoxedValue).controlSchemes.Select(x => x.name).ToArray();
            player1ControlSchemeIndex.intValue = EditorGUILayout.Popup("Player One Control Scheme", player1ControlSchemeIndex.intValue, controlSchemes);
            player2ControlSchemeIndex.intValue = EditorGUILayout.Popup("Player Two Control Scheme", player2ControlSchemeIndex.intValue, controlSchemes);
            player1ControlScheme.stringValue = controlSchemes[player1ControlSchemeIndex.intValue]; 
            player2ControlScheme.stringValue = controlSchemes[player2ControlSchemeIndex.intValue];

            serializedObject.ApplyModifiedProperties();
        }
    }
}