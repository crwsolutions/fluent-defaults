namespace FluentDefaults;

/// <summary>
/// Provides an abstract base class for asynchronously applying default values to properties or fields of an object.
/// Use this if any asynchronous factory functions are used for defaults.
/// </summary>
/// <typeparam name="T">The type of the object to which the default values are applied.</typeparam>
public abstract class AbstractAsyncDefaulter<T> : AbstractDefaulterBase<T>, IAsyncDefaulter<T>
{
    /// <summary>
    /// Asynchronously applies the default values to the specified target object.
    /// Both synchronous and asynchronous defaulters are applied.
    /// </summary>
    /// <param name="target">The target object to which the default values are applied.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ApplyAsync(T target)
    {
        foreach (var rule in _rules)
        {
            if (rule.Condition == null || rule.Condition(target))
            {
                if (rule.Action != null)
                {
                    rule.Apply(target);
                }
                else
                {
                    await rule.ApplyAsync(target);
                }
            }
        }
    }
}