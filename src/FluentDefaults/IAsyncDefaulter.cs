namespace FluentDefaults;

/// <summary>
/// Defines a method for asynchronously applying default values to properties or fields of an object.
/// </summary>
/// <typeparam name="T">The type of the object to which the default values are applied.</typeparam>
public interface IAsyncDefaulter<T>
{
    /// <summary>
    /// Asynchronously applies the default values to the specified target object.
    /// </summary>
    /// <param name="target">The target object to which the default values are applied.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ApplyAsync(T target);
}

