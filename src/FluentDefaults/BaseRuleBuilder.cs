namespace FluentDefaults;

/// <summary>
/// Provides a base class for building rules that specify default values for properties or fields.
/// </summary>
public abstract class BaseRuleBuilder<T, TProperty>
{
    internal readonly Rule<T> _rule;

    internal BaseRuleBuilder(Rule<T> rule)
    {
        _rule = rule;
    }

    /// <summary>
    /// Specifies a condition that must be met for the default value to be applied.
    /// </summary>
    /// <param name="condition">A function that defines the condition.</param>
    /// <returns>The current <see cref="BaseRuleBuilder{T, TProperty}"/> instance.</returns>
    public BaseRuleBuilder<T, TProperty> When(Func<T, bool> condition)
    {
        _rule.SetCondition(condition);
        return this;
    }

    /// <summary>
    /// Specifies a default value for the property or field.
    /// </summary>
    /// <param name="defaultValue">The default value to be set.</param>
    /// <returns>The current <see cref="BaseRuleBuilder{T, TProperty}"/> instance.</returns>
    public BaseRuleBuilder<T, TProperty> Is(TProperty defaultValue)
    {
        _rule.SetAction<TProperty>(defaultValue);
        return this;
    }

    /// <summary>
    /// Specifies a factory function that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultFunction">A function that produces the default value.</param>
    /// <returns>The current <see cref="BaseRuleBuilder{T, TProperty}"/> instance.</returns>
    public BaseRuleBuilder<T, TProperty> Is(Func<TProperty> defaultFunction)
    {
        _rule.SetAction<TProperty>(defaultFunction);
        return this;
    }

    /// <summary>
    /// Specifies a factory function that receives the instance and that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultFunction">A function that receives the instance and that produces the default value.</param>
    /// <returns>The current <see cref="BaseRuleBuilder{T, TProperty}"/> instance.</returns>
    public BaseRuleBuilder<T, TProperty> Is(Func<T, TProperty> defaultFunction) 
    { 
        _rule.SetAction<TProperty>(defaultFunction);
        return this;
    }

    /// <summary>
    /// Specifies an asynchronous factory function that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultAsyncFunction">A function that produces the default value asynchronously.</param>
    /// <returns>The current <see cref="BaseRuleBuilder{T, TProperty}"/> instance.</returns>
    public BaseRuleBuilder<T, TProperty> IsAsync(Func<Task<TProperty>> defaultAsyncFunction)
    {
        _rule.SetAsyncAction<TProperty>(defaultAsyncFunction);
        return this;
    }

    /// <summary>
    /// Specifies an asynchronous factory function that receives a reference to the instance that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultAsyncFunction">A function that produces the default value asynchronously.</param>
    /// <returns>The current <see cref="BaseRuleBuilder{T, TProperty}"/> instance.</returns>
    public BaseRuleBuilder<T, TProperty> IsAsync(Func<T, Task<TProperty>> defaultAsyncFunction)
    {
        _rule.SetAsyncAction<TProperty>(defaultAsyncFunction);
        return this;
    }
}
