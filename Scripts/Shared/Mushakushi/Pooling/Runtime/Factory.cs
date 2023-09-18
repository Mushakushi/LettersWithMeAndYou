using UnityEngine;

namespace Mushakushi.Pooling.Runtime
{
    /// <summary>
    /// Implements the IFactory interface for non-abstract types.
    /// </summary>
    /// <typeparam name="T">Specifies the non-abstract type to create.</typeparam>
    public abstract class Factory<T> : ScriptableObject, IFactory<T>
    {
        public abstract T CreateInstance();
    }
}