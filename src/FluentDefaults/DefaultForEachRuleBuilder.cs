using System.Linq.Expressions;
using System.Reflection;

namespace FluentDefaults;

/// <summary>
/// Provides functionality to set default values for each element in a collection property of a rule.
/// </summary>
public class DefaultForEachRuleBuilder<T, TProperty> : BaseRuleBuilder<T, TProperty>
{
    internal DefaultForEachRuleBuilder(Rule<T> rule) : base(rule) { }

    /// <summary>
    /// Sets a defaulter to apply default values to each element in the collection.
    /// </summary>
    /// <param name="defaulter">The defaulter to apply to each element in the collection.</param>
    public void SetDefaulter(AbstractDefaulter<TProperty> defaulter)
    {
        if (_rule.MemberExpression?.Member is PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();

            if (getMethod != null)
            {
                _rule.Action = instance =>
                {
                    var currentValue = (IEnumerable<TProperty>?)
                        getMethod.Invoke(instance, null);
                    if (!Equals(currentValue, default(TProperty)))
                    {
                        foreach (var element in currentValue!)
                        {
                            defaulter.Apply(element);
                        }
                    }
                };
            }
        }
    }

    /// <summary>
    /// Defines a default value for a specified property or field of each element in the collection.
    /// </summary>
    /// <param name="expression">An expression that specifies the property or field.</param>
    /// <param name="defaultValue">The default value to be set.</param>
    /// <returns>A <see cref="RuleBuilder{T, TProperty}"/> for further configuration.</returns>
    public BaseRuleBuilder<T, TProperty> DefaultFor<TElementProperty>(
        Expression<Func<TProperty, TElementProperty>> expression,
        TElementProperty defaultValue
    )
    {
        _rule.ChildMemberExpression = (MemberExpression)expression.Body;
        _rule.SetCollectionAction<TProperty, TElementProperty>(defaultValue);

        return this;
    }

    /// <summary>
    /// Defines a default value for a specified property or field of each element in the collection.
    /// </summary>
    /// <param name="expression">An expression that specifies the property or field.</param>
    /// <returns>A <see cref="RuleBuilder{T, TProperty}"/> for further configuration.</returns>
    public DefaultForEachRuleBuilder<T, TProperty> DefaultFor<TElementProperty>(
        Expression<Func<TProperty, TElementProperty>> expression
    )
    {
        _rule.ChildMemberExpression = (MemberExpression)expression.Body;
        return this;
    }

    /// <summary>
    /// Specifies a default value for the property or field.
    /// </summary>
    /// <param name="defaultValue">The default value to be set.</param>
    /// <returns>The current <see cref="BaseRuleBuilder{T, TProperty}"/> instance.</returns>
    public DefaultForEachRuleBuilder<T, TProperty> Is<TElementProperty>(TElementProperty defaultValue)
    {
        _rule.SetCollectionAction<TProperty, TElementProperty>(defaultValue);
        return this;
    }

    /// <summary>
    /// Specifies a factory function that produces the default value for the property or field.
    /// </summary>
    /// <param name="defaultFunction">A function that produces the default value.</param>
    /// <returns>The current <see cref="BaseRuleBuilder{T, TProperty}"/> instance.</returns>
    public DefaultForEachRuleBuilder<T, TProperty> Is<TElementProperty>(Func<TElementProperty> defaultFunction)
    {
        _rule.SetCollectionAction<TProperty, TElementProperty>(defaultFunction);
        return this;
    }

    ///// <summary>
    ///// Specifies a condition that must be met for the default value to be applied.
    ///// </summary>
    ///// <param name="condition">A function that defines the condition.</param>
    ///// <returns>The current <see cref="BaseRuleBuilder{T, TProperty}"/> instance.</returns>
    //public DefaultForEachRuleBuilder<T, TProperty> When(Func<TElementProperty, bool> condition)
    //{
    //    _rule.SetCondition(condition);
    //    return this;
    //}
}
