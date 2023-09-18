using System.Collections.Generic;
using UnityEngine;

namespace Mushakushi.Pooling.Runtime
{
    public abstract class Pool<T> : ScriptableObject, IPool<T>
    {
        /// <summary>
        /// The available members. 
        /// </summary>
        private readonly Stack<T> availableMembers = new();
        
        /// <summary>
        /// The <see cref="IFactory{T}"/>. 
        /// </summary>
        protected abstract IFactory<T> Factory { get; }
        
        /// <summary>
        /// Whether or not <see cref="Prewarm"/> has run. 
        /// </summary>
        private bool IsPrewarmed { get; set; }

        /// <summary>
        /// Creates an instance. 
        /// </summary>
        /// <returns><see cref="T"/> The instance.</returns>
        protected virtual T CreateInstance() => Factory.CreateInstance();

        public virtual void Prewarm(int poolSize)
        {
            if (IsPrewarmed)
            {
#if DEVELOPMENT_BUILD || UNITY_EDITOR || UNITY_ASSERTIONS
                Debug.LogWarning($"Pool {name} has already been prewarmed. Pre-warming can only happen once for the lifetime of the object.", this);
#endif
                return;
            }

            for (var i = 0; i < poolSize; i++)
            {
                availableMembers.Push(Factory.CreateInstance());
            }

            IsPrewarmed = true;
        }
        
        /// <summary>
        /// Requests a collection of <see cref="T"/> members in the pool. 
        /// </summary>
        /// <param name="count">The amount of <see cref="T"/> members to request.</param>
        /// <returns><see cref="IEnumerable{T}"/> The members.</returns>
        public IEnumerable<T> Allocate(int count)
        {
            var members = new T[count];
            for (var i = 0; i < count; i++) members[count] = Allocate();
            return members; 
        }
        
        public virtual T Allocate()
        {
            return availableMembers.Count > 0 ? availableMembers.Pop() : Factory.CreateInstance();
        }

        /// <summary>
        /// Returns a <see cref="T"/> collection to the pool. 
        /// </summary>
        /// <param name="members">The members to return.</param>
        public void Free(IEnumerable<T> members)
        {
            foreach (var member in members) Free(member);
        }

        public virtual void Free(T member)
        {
            availableMembers.Push(member);
        }

        protected virtual void OnDisable()
        {
            availableMembers.Clear();
            IsPrewarmed = false;
        }
    }
}