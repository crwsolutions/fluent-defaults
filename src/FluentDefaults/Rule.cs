using System.Linq.Expressions;
using System.Reflection;

namespace FluentDefaults;

internal class Rule<T>
{
    internal Action<T> Action { get; set; } = default!;
    internal Func<T, bool>? Condition { get; private set; }

    internal MemberExpression MemberExpression { get; }

    internal Rule(MemberExpression memberExpression)
    {
        MemberExpression = memberExpression;
    }

    internal void SetCondition(Func<T, bool> condition)
    {
        Condition = condition;
    }

    internal void Apply(T target)
    {
        Action(target);
    }

    internal void SetAction<TProperty>(
        object? defaultValueOrFactory
    )
    {
        var defaultValue = defaultValueOrFactory is Func<TProperty> factory
            ? factory()
            : defaultValueOrFactory is null
                ? default
                : (TProperty)defaultValueOrFactory;

        if (MemberExpression.Member is PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();
            var setMethod = propertyInfo.GetSetMethod();

            if (getMethod != null && setMethod != null)
            {
                Action = instance =>
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
        else if (MemberExpression.Member is FieldInfo fieldInfo)
        {
            Action = instance =>
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
    }
}
