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
}
