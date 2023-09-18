// modified from: https://stackoverflow.com/a/72738686/20697358
using UnityEngine;

namespace Framework.RenderUtility.Runtime
{
    /// <summary>
    /// Preserves the original rotation of a GameObject
    /// </summary>
    public class PreserveChildRotation: MonoBehaviour
    {
        private Quaternion oldParentLocalRotation;

        private void Awake()
        {
            oldParentLocalRotation = GetParentLocalRotation();
        }

        private void LateUpdate()
        {
            var child = transform;
            child.localRotation = Quaternion.Inverse(GetParentLocalRotation()) * oldParentLocalRotation * child.localRotation;
            oldParentLocalRotation = GetParentLocalRotation();
        }

        /// <summary>
        /// Gets the local rotation of this <see cref="Transform"/>. 
        /// </summary>
        /// <returns><see cref="Quaternion"/> the parent's local rotation if it exists, <see cref="Transform.localRotation"/> otherwise.</returns>
        private Quaternion GetParentLocalRotation()
        {
            return transform.parent ? transform.parent.localRotation : transform.localRotation;
        }
    }
}