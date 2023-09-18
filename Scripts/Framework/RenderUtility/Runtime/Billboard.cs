using Mushakushi.Attributes.Runtime;
using UnityEngine;

namespace Framework.RenderUtility.Runtime
{
    /// <summary>
    /// Billboards a GameObject using two methods. 
    /// </summary>
    public class Billboard: MonoBehaviour
    {
        /// <summary>
        /// The camera to face. 
        /// </summary>
        [Header("Billboard")]
        [SerializeField] private new Transform camera;

        /// <summary>
        /// Whether or not to just inherit the camera rotation. 
        /// </summary>
        [SerializeField] private bool inheritCameraRotation = true; 

        /// <summary>
        /// Whether or not to flip the rotation. 
        /// </summary>
        [SerializeField] private bool flipped;

        /// <summary>
        /// The speed at which to rotate. 
        /// </summary>
        [SerializeField, Min(0)] private float rotationSpeed = 25f; 

        /// <summary>
        /// Whether or not to constrain x axis rotation. 
        /// </summary>
        [Header("Constraints")] 
        [SerializeField, Rename("X")] private bool constrainX;

        /// <summary>
        /// Whether or not to constrain y axis rotation. 
        /// </summary>
        [SerializeField, Rename("Y")] private bool constrainY;
        
        /// <summary>
        /// Whether or not to constrain z axis rotation. 
        /// </summary>
        [SerializeField, Rename("Z")] private bool constrainZ;

        private void LateUpdate()
        {
            if (!camera) return;
            Quaternion lookRotation;
            if (inheritCameraRotation)
            {
                var cameraForward = new Vector3(
                    constrainY ? 0 : camera.forward.x, 
                    constrainX ? 0 : camera.forward.y, 
                    constrainZ ? 0 : camera.forward.z
                );
                lookRotation = Quaternion.LookRotation(cameraForward * (flipped ? -1 : 1));
            }
            else
            {
                var direction = (camera.position - transform.position).normalized * (flipped ? 1 : -1);
                lookRotation = Quaternion.LookRotation(new Vector3(
                    constrainY ? 0 : direction.x, 
                    constrainX ? 0 : direction.y,
                    constrainZ ? 0 : direction.z
                ));
            } 
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}