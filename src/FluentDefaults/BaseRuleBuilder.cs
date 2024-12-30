namespace FluentDefaults;

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

    public BaseRuleBuilder<T, TProperty> Is(TProperty defaultValue)
    {
        _rule.SetAction<TProperty>(defaultValue);
        return this;
    }

    public BaseRuleBuilder<T, TProperty> Is(Func<TProperty> defaultFactory)
    {
        _rule.SetAction<TProperty>(defaultFactory);
        return this;
    }

    public BaseRuleBuilder<T, TProperty> IsAsync(Func<Task<TProperty>> defaultFactory)
    {
        _rule.SetAsyncAction(defaultFactory);
        return this;
    }
}
