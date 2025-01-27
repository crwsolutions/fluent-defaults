using System.Linq.Expressions;

namespace FluentDefaults;

/// <summary>
/// Provides functionality to set default values for each element in a collection property of a rule.
/// </summary>
public sealed class DefaultForEachRuleBuilder<T, TProperty>
{
    private readonly Rule<T> _rule;

    internal DefaultForEachRuleBuilder(Rule<T> rule)
    {
        _rule = rule;
    }

    /// <summary>
    /// Sets a defaulter to apply default values to each element in the collection.
    /// </summary>
    /// <param name="defaulter">The defaulter to apply to each element in the collection.</param>
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
            var currentValue = _rule.GetCollectionValue<TProperty>(instance);
            if (!Equals(currentValue, default(TProperty)))
            {
                foreach (var element in currentValue!)
                {
                    var defaulterInstance = defaulterFactory != null ? defaulterFactory(instance) : defaulter;
                    defaulterInstance?.Apply(element);
                }
            }
        };
    }

    /// <summary>
    /// Defines a default value for a specified property or field of each element in the collection.
    /// </summary>
    /// <param name="expression">An expression that specifies the property or field.</param>
    /// <returns>A <see cref="RuleBuilder{T, TProperty}"/> for further configuration.</returns>
    public DefaultForEachIsRuleBuilder<T, TProperty> DefaultFor<TElementProperty>(
        Expression<Func<TProperty, TElementProperty>> expression
    )
    {
        _rule.ChildMemberExpression = (MemberExpression)expression.Body;
        return new DefaultForEachIsRuleBuilder<T, TProperty>(_rule);
    }
}
