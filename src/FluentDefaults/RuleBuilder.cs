namespace FluentDefaults;

/// <summary>
/// Provides a builder for defining rules that apply default values to properties or fields.
/// </summary>
public sealed class RuleBuilder<T, TProperty>
{
    private readonly Rule<T> _rule;

    internal RuleBuilder(Rule<T> rule)
    {
        _rule = rule;
    }

    /// <summary>
    /// Specifies a default value for the property or field.
    /// </summary>
    /// <param name="defaultValue">The default value to be set.</param>
    /// <returns>The current <see cref="WhenRuleBuilder{T}"/> instance.</returns>
    public WhenRuleBuilder<T> Is(TProperty defaultValue)
    {
        _rule.SetAction<TProperty>(defaultValue);
        return new WhenRuleBuilder<T>(_rule);
    }

    /// <summary>
    /// Specifies a factory function that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultFunction">A function that produces the default value.</param>
    /// <returns>The current <see cref="WhenRuleBuilder{T}"/> instance.</returns>
    public WhenRuleBuilder<T> Is(Func<TProperty> defaultFunction)
    {
        _rule.SetAction<TProperty>(defaultFunction);
        return new WhenRuleBuilder<T>(_rule);
    }

    /// <summary>
    /// Specifies a factory function that receives the instance and that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultFunction">A function that receives the instance and that produces the default value.</param>
    /// <returns>The current <see cref="WhenRuleBuilder{T}"/> instance.</returns>
    public WhenRuleBuilder<T> Is(Func<T, TProperty> defaultFunction)
    {
        _rule.SetAction<TProperty>(defaultFunction);
        return new WhenRuleBuilder<T>(_rule);
    }

    /// <summary>
    /// Specifies an asynchronous factory function that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultAsyncFunction">A function that produces the default value asynchronously.</param>
    /// <returns>The current <see cref="WhenRuleBuilder{T}"/> instance.</returns>
    public WhenRuleBuilder<T> IsAsync(Func<Task<TProperty>> defaultAsyncFunction)
    {
        _rule.SetAsyncAction<TProperty>(defaultAsyncFunction);
        return new WhenRuleBuilder<T>(_rule);
    }

    /// <summary>
    /// Specifies an asynchronous factory function that receives a reference to the instance that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultAsyncFunction">A function that produces the default value asynchronously.</param>
    /// <returns>The current <see cref="WhenRuleBuilder{T}"/> instance.</returns>
    public WhenRuleBuilder<T> IsAsync(Func<T, Task<TProperty>> defaultAsyncFunction)
    {
        _rule.SetAsyncAction<TProperty>(defaultAsyncFunction);
        return new WhenRuleBuilder<T>(_rule);
    }

    /// <summary>
    /// Sets a defaulter to apply default values to the specified property or field.
    /// </summary>
    /// <param name="defaulter">The defaulter to apply to the property or field.</param>
    /// <exception cref="Exception">Thrown if the property or field does not have a get method or if the member is not found.</exception>
    public void SetDefaulter(AbstractDefaulter<TProperty> defaulter) =>
        SetDefaulterImpl(defaulter: defaulter);

    /// <summary>
    /// Sets a defaulter factory to apply default values to the specified property or field.
    /// </summary>
    /// <param name="defaulterFactory">The factory function to create the defaulter to apply to the property or field.</param>
    /// <exception cref="Exception">Thrown if the property or field does not have a get method or if the member is not found.</exception>
    public void SetDefaulter(Func<T, AbstractDefaulter<TProperty>> defaulterFactory) =>
        SetDefaulterImpl(defaulterFactory);

    private void SetDefaulterImpl(Func<T, AbstractDefaulter<TProperty>>? defaulterFactory = null, AbstractDefaulter<TProperty>? defaulter = null)
    {
        _rule.Action = instance =>
        {
            var currentValue = _rule.GetMemberValue<TProperty>(instance);
            if (!Equals(currentValue, default(TProperty)))
            {
                var defaulterInstance = defaulterFactory != null ? defaulterFactory(instance) : defaulter; ;
                defaulterInstance?.Apply(currentValue!);
            }
        };
    }
}
