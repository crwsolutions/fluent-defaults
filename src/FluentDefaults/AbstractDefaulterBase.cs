using System.Linq.Expressions;

namespace FluentDefaults;

/// <summary>
/// Provides a base class for defining default values for properties or fields of an object.
/// </summary>
public abstract class AbstractDefaulterBase<T>
{
    internal readonly List<Rule<T>> _rules = [];

    /// <summary>
    /// Defines a default value for a specified property or field.
    /// </summary>
    /// <param name="expression">An expression that specifies the property or field.</param>
    /// <returns>A <see cref="RuleBuilder{T, TProperty}"/> for further configuration.</returns>
    public RuleBuilder<T, TProperty> DefaultFor<TProperty>(
        Expression<Func<T, TProperty>> expression
    )
    {
        var rule = new Rule<T>((MemberExpression)expression.Body);
        _rules.Add(rule);
        return new RuleBuilder<T, TProperty>(rule);
    }

    /// <summary>
    /// Defines a default value for each element in a collection property or field.
    /// </summary>
    /// <param name="expression">An expression that specifies the collection property or field.</param>
    /// <returns>A <see cref="DefaultForEachRuleBuilder{T, TProperty}"/> for further configuration.</returns>
    public DefaultForEachRuleBuilder<T, TProperty> ForEach<TProperty>(
        Expression<Func<T, IEnumerable<TProperty>>> expression
    )
    {
        var rule = new Rule<T>((MemberExpression)expression.Body);
        _rules.Add(rule);
        return new DefaultForEachRuleBuilder<T, TProperty>(rule);
    }
}
