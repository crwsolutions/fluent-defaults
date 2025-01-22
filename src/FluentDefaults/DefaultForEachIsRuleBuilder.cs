namespace FluentDefaults;

/// <summary>
/// Provides functionality to define default values for a specified property or field of each element in a collection.
/// </summary>
public sealed class DefaultForEachIsRuleBuilder<T, TProperty>
{
    private readonly Rule<T> _rule;

    internal DefaultForEachIsRuleBuilder(Rule<T> rule)
    {
        _rule = rule;
    }

    /// <summary>
    /// Specifies a default value for the property or field.
    /// </summary>
    /// <param name="defaultValue">The default value to be set.</param>
    public void Is<TElementProperty>(TElementProperty defaultValue) =>
        _rule.SetCollectionAction<TProperty, TElementProperty>(defaultValue);

    /// <summary>
    /// Specifies a factory function that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultFunction">A function that produces the default value.</param>
    public void Is<TElementProperty>(Func<TElementProperty> defaultFunction) =>
        _rule.SetCollectionAction<TProperty, TElementProperty>(defaultFunction);

    /// <summary>
    /// Specifies a factory function that receives the element that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultFunction">A function that produces the default value.</param>
    public void Is<TElementProperty>(Func<T, TElementProperty> defaultFunction) =>
        _rule.SetCollectionAction<TProperty, TElementProperty>(defaultFunction);

    /// <summary>
    /// Specifies a factory function that receives the parent instance that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultFunction">A function that produces the default value.</param>
    public void Is<TElementProperty>(Func<TProperty, TElementProperty> defaultFunction) =>
        _rule.SetCollectionAction<TProperty, TElementProperty>(defaultFunction);
}
