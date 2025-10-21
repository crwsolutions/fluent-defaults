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

    /// <summary>
    /// Sets a defaulter to apply default values to each element in the collection.
    /// </summary>
    /// <param name="defaulter">The defaulter to apply to each element in the collection.</param>
    public void SetDefaulter(AbstractDefaulter<TProperty> defaulter) =>
        SetDefaulterImpl(defaulter: defaulter);

    /// <summary>
    /// Sets a defaulter factory to apply default values to each element in the collection.
    /// </summary>
    /// <param name="defaulterFactory">The factory that creates a defaulter using the parent instance.</param>
    public void SetDefaulter(Func<T, AbstractDefaulter<TProperty>> defaulterFactory) =>
        SetDefaulterImpl(defaulterFactory);

    private void SetDefaulterImpl(Func<T, AbstractDefaulter<TProperty>>? defaulterFactory = null, AbstractDefaulter<TProperty>? defaulter = null)
    {
        _rule.Action = instance =>
        {
            var currentValue = _rule.GetCollectionValue<TProperty>(instance);
            if (!Equals(currentValue, default(TProperty)))
            {
                if (!(currentValue is System.Collections.ICollection || currentValue is System.Array))
                {
                    throw new DeferredExecutionException($"{_rule.MemberExpression} => {_rule.ChildMemberExpression}");
                }

                foreach (var element in currentValue!)
                {
                    var def = defaulterFactory != null ? defaulterFactory(instance) : defaulter;
                    def?.Apply(element);
                }
            }
        };
    }
}
