using System.Linq.Expressions;

namespace FluentDefaults;

public abstract class AbstractDefaulter<T> : IDefaulter<T>
{
    private readonly List<Rule<T>> _rules = [];

    public void Apply(T target)
    {
        foreach (var rule in _rules)
        {
            if (rule.Condition == null || rule.Condition(target))
            {
                rule.Apply(target);
            }
        }
    }

    public RuleBuilder<T, TProperty> DefaultFor<TProperty>(
        Expression<Func<T, TProperty>> expression,
        TProperty defaultValue
    )
    {
        var rule = new Rule<T>((MemberExpression)expression.Body);
        rule.SetAction<TProperty>(defaultValue);
        _rules.Add(rule);
        return new RuleBuilder<T, TProperty>(rule);
    }

    public RuleBuilder<T, TProperty> DefaultFor<TProperty>(
        Expression<Func<T, TProperty>> expression,
        Func<TProperty> defaultFactory
    )
    {
        var rule = new Rule<T>((MemberExpression)expression.Body);
        rule.SetAction<TProperty>(defaultFactory);
        _rules.Add(rule);
        return new RuleBuilder<T, TProperty>(rule);
    }

    public RuleBuilder<T, TProperty> DefaultFor<TProperty>(
        Expression<Func<T, TProperty>> expression
    )
    {
        var rule = new Rule<T>((MemberExpression)expression.Body);

        _rules.Add(rule);

        return new RuleBuilder<T, TProperty>(rule);
    }

    public DefaultForEachRuleBuilder<T, TProperty> DefaultForEach<TProperty>(
        Expression<Func<T, IEnumerable<TProperty>>> expression
    )
    {
        var rule = new Rule<T>((MemberExpression)expression.Body);

        _rules.Add(rule);

        return new DefaultForEachRuleBuilder<T, TProperty>(rule);
    }
}
