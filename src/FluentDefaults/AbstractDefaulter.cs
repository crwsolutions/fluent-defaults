using System.Linq.Expressions;
using System.Reflection;

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
        var rule = new Rule<T>(GetAction(defaultValue, expression));
        _rules.Add(rule);
        return new RuleBuilder<T, TProperty>(rule);
    }

    public RuleBuilder<T, TProperty> DefaultFor<TProperty>(
        Expression<Func<T, TProperty>> expression,
        Func<TProperty> defaultFactory
    )
    {
        var rule = new Rule<T>(GetAction(defaultFactory, expression));
        _rules.Add(rule);
        return new RuleBuilder<T, TProperty>(rule);
    }

    public RuleBuilder<T, TProperty> DefaultFor<TProperty>(
        Expression<Func<T, TProperty>> expression
    )
    {
        if (expression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException("Expression must be a member expression", nameof(expression));
        }

        var rule = new Rule<T>((MemberExpression)expression.Body);

        _rules.Add(rule);

        return new RuleBuilder<T, TProperty>(rule);
    }

    public DefaultForEachRuleBuilder<T, TProperty> DefaultForEach<TProperty>(
        Expression<Func<T, IEnumerable<TProperty>>> expression
    )
    {
        if (expression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException("Expression must be a member expression", nameof(expression));
        }

        var rule = new Rule<T>((MemberExpression)expression.Body);

        _rules.Add(rule);

        return new DefaultForEachRuleBuilder<T, TProperty>(rule);
    }

    private static Action<T> GetAction<TProperty>(
    object? defaultValueOrFactory,
    Expression<Func<T, TProperty>> expression
)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            var defaultValue = defaultValueOrFactory is Func<TProperty> factory
                ? factory()
                : defaultValueOrFactory is null
                    ? default
                    : (TProperty)defaultValueOrFactory;

            Action<T> action;
            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                var getMethod = propertyInfo.GetGetMethod();
                var setMethod = propertyInfo.GetSetMethod();

                if (getMethod != null && setMethod != null)
                {
                    action = instance =>
                    {
                        var currentValue = (TProperty?)getMethod.Invoke(instance, null);
                        if (Equals(currentValue, default(TProperty)))
                        {
                            setMethod.Invoke(instance, [defaultValue]);
                        }
                    };
                }
                else
                {
                    throw new Exception("Property has no set and get method");
                }
            }
            else if (memberExpression.Member is FieldInfo fieldInfo)
            {
                action = instance =>
                {
                    var currentValue = (TProperty?)fieldInfo.GetValue(instance);
                    if (Equals(currentValue, default(TProperty)))
                    {
                        fieldInfo.SetValue(instance, defaultValue);
                    }
                };
            }
            else
            {
                throw new Exception("Member not found");
            }

            return action;
        }

        throw new Exception("Member not found");
    }
}
