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

public abstract class AbstractAyncDefaulter<T> : AbstractDefaulterBase<T>
{
    public async Task ApplyAsync(T target)
    {
        foreach (var rule in _rules)
        {
            if (rule.Condition == null || rule.Condition(target))
            {
                if (rule.Action != null)
                {
                    rule.Apply(target);
                }
                else 
                {
                    await rule.ApplyAsync(target);
                }
            }
        }
    }
}