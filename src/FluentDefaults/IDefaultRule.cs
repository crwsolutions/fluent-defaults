using System.Linq.Expressions;
using System.Reflection;

namespace FluentDefaults;

public interface IDefaulter<T>
{
    void Apply(T target);
}

public abstract class AbstractDefaulter<T>
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

internal class Rule<T>
{
    internal Action<T> Action { get; set; } = default!;
    internal Func<T, bool>? Condition { get; private set; }

    internal MemberExpression? MemberExpression { get; set; }

    internal Rule(MemberExpression memberExpression)
    {
        MemberExpression = memberExpression;
    }

    internal Rule(Action<T> propertySelector)
    {
        Action = propertySelector;
    }

    internal void SetCondition(Func<T, bool> condition)
    {
        Condition = condition;
    }

    internal void Apply(T target)
    {
        Action(target);
    }
}

public abstract class BaseRuleBuilder<T, TProperty>
{
    internal readonly Rule<T> _rule;

    internal BaseRuleBuilder(Rule<T> rule)
    {
        _rule = rule;
    }

    public BaseRuleBuilder<T, TProperty> When(Func<T, bool> condition)
    {
        _rule.SetCondition(condition);
        return this;
    }
}

public class RuleBuilder<T, TProperty> : BaseRuleBuilder<T, TProperty>
{
    internal RuleBuilder(Rule<T> rule) : base(rule) { }

    public void SetDefaulter(AbstractDefaulter<TProperty> defaulter)
    {
        if (_rule.MemberExpression?.Member is PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();

            if (getMethod != null)
            {
                _rule.Action = instance =>
                {
                    var currentValue = (TProperty?)getMethod.Invoke(instance, null);
                    if (!Equals(currentValue, default(TProperty)))
                    {
                        defaulter.Apply(currentValue!);
                    }
                };
            }
            else
            {
                throw new Exception("Property has no set and get method");
            }
        }
        else if (_rule.MemberExpression?.Member is FieldInfo fieldInfo)
        {
            _rule.Action = instance =>
            {
                var currentValue = (TProperty?)fieldInfo.GetValue(instance);
                if (!Equals(currentValue, default(TProperty)))
                {
                    defaulter.Apply(currentValue!);
                }
            };
        }
        else
        {
            throw new Exception("Member not found");
        }
    }
}
public class DefaultForEachRuleBuilder<T, TProperty> : BaseRuleBuilder<T, TProperty>
{
    internal DefaultForEachRuleBuilder(Rule<T> rule) : base(rule) { }

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
}