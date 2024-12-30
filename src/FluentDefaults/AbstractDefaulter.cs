namespace FluentDefaults;

public abstract class AbstractDefaulter<T> : AbstractDefaulterBase<T>, IDefaulter<T>
{
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
}
