namespace FluentDefaults;

public abstract class AbstractAsyncDefaulter<T> : AbstractDefaulterBase<T>, IAsyncDefaulter<T>
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