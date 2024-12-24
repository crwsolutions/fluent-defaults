using System.Linq.Expressions;

namespace FluentDefaults;

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
