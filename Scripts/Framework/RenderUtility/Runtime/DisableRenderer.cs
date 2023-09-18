using UnityEngine;

namespace Framework.RenderUtility.Runtime
{
    /// <summary>
    /// Disables the renderer on <see cref="Start"/>. Useful for showing editor-only visualizations.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class DisableRenderer : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Renderer>().enabled = false; 
        }
    }
}