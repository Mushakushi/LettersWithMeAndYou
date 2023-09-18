using Mushakushi.Attributes.Runtime;
using UnityEngine;
using UnityEditor;

namespace Mushakushi.Attributes.Editor
{
    /// <summary>
    /// Draws the <see cref="ReadOnlyAttribute"/>
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public sealed class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        /// Includes children in GetPropertyHeight calculation.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        /// <summary>
        /// Makes a un-editable serialized field for this GUI.
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false; // Disabling the gui makes it not editable in inspector, reenable after to see it
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}