namespace Mushakushi.Pooling.Runtime
{
    /// <summary>
    /// Represents a collection that pools objects of T.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the pool.</typeparam>
    public interface IPool<T>
    {
        /// <summary>
        /// Initializes the pool. 
        /// </summary>
        /// <param name="poolSize">The amount of <see cref="T"/> members to initialize.</param>
        void Prewarm(int poolSize);

        /// <summary>
        /// Requests an member of <see cref="T"/> in the pool. 
        /// </summary>
        /// <returns><see cref="T"/> The member.</returns>
        T Allocate();

        /// <summary>
        /// Returns an element to the pool. 
        /// </summary>
        /// <param name="member">The member to return.</param>
        void Free(T member);
    }
}