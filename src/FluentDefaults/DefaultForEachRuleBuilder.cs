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
    public void DefaultFor<TElementProperty>(
        Expression<Func<TProperty, TElementProperty>> expression,
        TElementProperty defaultValue
    )
    {
        if (_rule.MemberExpression?.Member is PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();

            var childExpression = (MemberExpression)expression.Body;

            if (childExpression.Member is PropertyInfo childPropertyInfo)
            {
                var childGetMethod = childPropertyInfo.GetGetMethod();
                var childSetMethod = childPropertyInfo.GetSetMethod();

                if (getMethod != null && childGetMethod != null && childSetMethod != null)
                {
                    _rule.Action = instance =>
                    {
                        var currentValue = (IEnumerable<TProperty>?)
                            getMethod.Invoke(instance, null);
                        if (!Equals(currentValue, default(TProperty)))
                        {

                            {
                                var getMethod = propertyInfo.GetGetMethod();
                                foreach (var element in currentValue!)
                                {
                                    var childValue = (TElementProperty?)childGetMethod.Invoke(element, null);
                                    if (Equals(childValue, default(TElementProperty)))
                                    { 
                                        childSetMethod.Invoke(element, [defaultValue]);
                                    }
                                }
                            }
                        }
                    };
                }
            }
        }
    }
}
