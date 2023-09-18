// modified from https://answers.unity.com/questions/1487864/change-a-variable-name-only-on-the-inspector.html
using JetBrains.Annotations;
using UnityEngine;

namespace Mushakushi.Attributes.Runtime
{
    /// <summary>
    /// Renames a property. 
    /// </summary>
    public sealed class RenameAttribute : PropertyAttribute
    {
        /// <summary>
        /// The renamed name. 
        /// </summary>
        [UsedImplicitly] public readonly string name; 
    
        public RenameAttribute(string name)
        {
            this.name = name;
        }
    }
}