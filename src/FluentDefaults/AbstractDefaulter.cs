namespace FluentDefaults;

/// <summary>
/// Provides an abstract base class for applying default values to properties or fields of an object.
/// </summary>
/// <typeparam name="T">The type of the object to which the default values are applied.</typeparam>
public abstract class AbstractDefaulter<T> : AbstractDefaulterBase<T>, IDefaulter<T>
{
    /// <summary>
    /// Synchronously applies the default values to the specified target object.
    /// </summary>
    /// <param name="target">The target object to which the default values are applied.</param>
    public void Apply(T target)
    {
        foreach (var rule in _rules)
        {
            if (rule.Condition == null || rule.Condition(target))
            {
                rule.Apply(target);
            }
        }
    }
}
