namespace FluentDefaults;

/// <summary>
/// Exception thrown when a collection with deferred execution is detected.
/// </summary>
public class DeferredExecutionException : InvalidOperationException
{
    /// <summary>
    /// Gets the name of the member (property or field) that caused the deferred execution issue.
    /// </summary>
    public string MemberName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeferredExecutionException"/> class.
    /// </summary>
    /// <param name="memberName">The name of the member that triggered the exception, typically a property or field name.</param>
    public DeferredExecutionException(string memberName)
        : base($"Deferred execution detected on member '{memberName}'. Please ensure the collection is materialized before processing.")
    {
        MemberName = memberName;
    }
}
