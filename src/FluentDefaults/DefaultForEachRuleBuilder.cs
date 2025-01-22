using System.Linq.Expressions;
using System.Reflection;

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
    /// <returns>A <see cref="RuleBuilder{T, TProperty}"/> for further configuration.</returns>
    public DefaultForEachIsRuleBuilder<T, TProperty> DefaultFor<TElementProperty>(
        Expression<Func<TProperty, TElementProperty>> expression
    )
    {
        _rule.ChildMemberExpression = (MemberExpression)expression.Body;
        return new DefaultForEachIsRuleBuilder<T, TProperty>(_rule);
    }
}
