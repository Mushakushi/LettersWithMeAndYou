namespace Mushakushi.Pooling.Runtime
{
    /// <summary>
    /// Represents a factory.
    /// </summary>
    /// <typeparam name="T">Specifies the type to create.</typeparam>
    public interface IFactory<out T>
    {
        /// <summary>
        /// Creates an instance. 
        /// </summary>
        /// <returns><see cref="T"/> The instance.</returns>
        T CreateInstance();
    }
}