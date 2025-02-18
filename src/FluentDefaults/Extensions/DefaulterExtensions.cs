namespace FluentDefaults.Extensions;

/// <summary>
/// Extension methods for applying defaulters to a target object.
/// </summary>
public static class DefaulterExtensions
{
    /// <summary>
    /// Applies the specified defaulter to the target object.
    /// </summary>
    /// <typeparam name="T">The type of the target object.</typeparam>
    /// <param name="target">The target object to which the defaulter is applied.</param>
    /// <param name="defaulter">The defaulter to apply to the target object.</param>
    /// <returns>The original target object with the defaulter applied.</returns>
    public static T ApplyDefaulter<T>(this T target, IDefaulter<T> defaulter)
    {
        defaulter.Apply(target);
        return target;
    }

    /// <summary>
    /// Applies the specified defaulter to the target object asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the target object.</typeparam>
    /// <param name="target">The target object to which the defaulter is applied.</param>
    /// <param name="defaulter">The asynchronous defaulter to apply to the target object.</param>
    /// <returns>A task representing the asynchronous operation, with the original target object returned when completed.</returns>
    public static async Task<T> ApplyDefaulterAsync<T>(this T target, IAsyncDefaulter<T> defaulter)
    {
        await defaulter.ApplyAsync(target);
        return target;
    }
}

