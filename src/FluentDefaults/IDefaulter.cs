namespace FluentDefaults;

/// <summary>
/// Defines a method for applying default values to properties or fields of an object.
/// </summary>
/// <typeparam name="T">The type of the object to which the default values are applied.</typeparam>
public interface IDefaulter<T>
{
    /// <summary>
    /// Applies the default values to the specified target object.
    /// </summary>
    /// <param name="target">The target object to which the default values are applied.</param>
    void Apply(T target);
}
