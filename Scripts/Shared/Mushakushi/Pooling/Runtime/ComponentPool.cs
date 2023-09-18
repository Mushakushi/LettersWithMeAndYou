using UnityEngine;

namespace Mushakushi.Pooling.Runtime
{
    /// <summary>
    /// Implements a Pool for <see cref="Component"/> types.
    /// </summary>
    /// <typeparam name="T">Specifies the component to pool.</typeparam>
    public abstract class ComponentPool<T>: Pool<T> where T: Component
    {
        /// <summary>
        /// The pool root. 
        /// </summary>
        private Transform parent;

        public override T Allocate()
        {
            var member = base.Allocate();
            member.gameObject.SetActive(true);
            return member;
        }

        public override void Free(T member)
        {
            member.transform.SetParent(parent);
            member.gameObject.SetActive(false);
            base.Free(member);
        }

        protected override T CreateInstance()
        {
            var member = base.CreateInstance();
            member.transform.SetParent(parent);
            member.gameObject.SetActive(false);
            return member;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
#if UNITY_EDITOR
            DestroyImmediate(parent.gameObject);
#else 
            Destroy(parent.gameObject);
#endif
        }
    }
}