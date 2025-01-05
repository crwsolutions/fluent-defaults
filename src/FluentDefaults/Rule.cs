using System.Linq.Expressions;
using System.Reflection;

namespace FluentDefaults;

internal class Rule<T>
{
    internal Action<T>? Action { get; set; }

    internal Func<T, Task>? ActionAsync { get; set; }

    internal Func<T, bool>? Condition { get; private set; }

    internal MemberExpression MemberExpression { get; set; }

    internal Rule(MemberExpression memberExpression)
    {
        MemberExpression = memberExpression;
    }

    internal void SetCondition(Func<T, bool> condition)
    {
        Condition = condition;
    }

    internal async Task ApplyAsync(T target)
    {
        if (ActionAsync is not null)
        {
            await ActionAsync(target);
        }
    }

    internal void Apply(T target)
    {
        if (Action is not null)
        { 
            Action(target);
        }
    }

    internal void SetAsyncAction<TProperty>(
        Func<Task<TProperty>> defaultValue
    )
    {
        if (MemberExpression.Member is PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();
            var setMethod = propertyInfo.GetSetMethod();

            if (getMethod != null && setMethod != null)
            {
                ActionAsync = async instance =>
                {
                    var currentValue = (TProperty?)getMethod.Invoke(instance, null);
                    if (Equals(currentValue, default(TProperty)))
                    {
                        var v = await defaultValue();
                        setMethod.Invoke(instance, [v]);
                    }
                    await Task.CompletedTask;
                };
            }
            else
            {
                throw new Exception("Property has no set and get method");
            }
        }
        else if (MemberExpression.Member is FieldInfo fieldInfo)
        {
            ActionAsync = async instance =>
            {
                var currentValue = (TProperty?)fieldInfo.GetValue(instance);
                if (Equals(currentValue, default(TProperty)))
                {
                    var v = await defaultValue();
                    fieldInfo.SetValue(instance, v);
                }
                await Task.CompletedTask;
            };
        }
        else
        {
            throw new Exception("Member not found");
        }
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
