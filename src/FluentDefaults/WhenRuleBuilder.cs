namespace FluentDefaults;

/// <summary>
/// Provides a base class for building rules that specify conditions for applying default values to properties or fields.
/// </summary>
public sealed class WhenRuleBuilder<T>
{
    internal readonly Rule<T> _rule;

    internal WhenRuleBuilder(Rule<T> rule)
    {
        _rule = rule;
    }

    /// <summary>
    /// Specifies a condition that must be met for the default value to be applied.
    /// </summary>
    /// <param name="condition">A function that defines the condition.</param>
    public void When(Func<T, bool> condition)
    {
        _rule.SetCondition(condition);
    }
}
