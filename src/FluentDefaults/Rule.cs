using System.Linq.Expressions;
using System.Reflection;

namespace FluentDefaults;

internal sealed class Rule<T>
{
    internal Action<T>? Action { get; set; }

    internal Func<T, Task>? ActionAsync { get; set; }

    internal Func<T, bool>? Condition { get; private set; }

    internal MemberExpression MemberExpression { private get; set; }

    internal IEnumerable<TProperty>? GetCollectionValue<TProperty>(T instance)
    {
        return (IEnumerable<TProperty>?)(MemberExpression?.Member as PropertyInfo)?.GetGetMethod()?.Invoke(instance, null);
    }

    private TElementProperty? GetChildValue<TProperty, TElementProperty>(TProperty element)
    {
        return (TElementProperty?)(ChildMemberExpression!.Member as PropertyInfo)?.GetGetMethod()?.Invoke(element, null);
    }

    private void SetChildValue<TProperty, TElementProperty>(TProperty element, TElementProperty? defaultValue)
    {
        (ChildMemberExpression!.Member as PropertyInfo)?.GetSetMethod()?.Invoke(element, [defaultValue]);
    }

    internal TProperty? GetMemberValue<TProperty>(T instance)
    {
        return MemberExpression.Member switch
        {
            PropertyInfo propertyInfo => (TProperty?)propertyInfo.GetGetMethod()?.Invoke(instance, null),
            FieldInfo fieldInfo => (TProperty?)fieldInfo.GetValue(instance),
            _ => throw new Exception("Unsupported member type")
        };
    }

    private void SetMemberValue<TProperty>(T instance, TProperty? defaultValue)
    {
        if (MemberExpression.Member is PropertyInfo propertyInfo)
        {
            propertyInfo.GetSetMethod()?.Invoke(instance, [defaultValue]);
        }
        else if (MemberExpression.Member is FieldInfo fieldInfo)
        {
            fieldInfo.SetValue(instance, defaultValue);
        }
        else
        {
            throw new Exception("Member is not a Property or Field");
        }
    }

    internal MemberExpression? ChildMemberExpression { get; set; }

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

    internal void SetAsyncAction<TProperty>(Delegate defaultFactory)
    {
        ActionAsync = async instance =>
        {
            var currentValue = GetMemberValue<TProperty>(instance);
            if (Equals(currentValue, default(TProperty)))
            {
                var defaultValue = defaultFactory switch
                {
                    Func<Task<TProperty>> factory => await factory(),
                    Func<T, Task<TProperty>> factory => await factory(instance),
                    _ => default
                };
                SetMemberValue(instance, defaultValue);
            }
            await Task.CompletedTask;
        };
    }

    internal void SetAction<TProperty>(object? defaultValueOrFactory)
    {
        Action = instance =>
        {
            var currentValue = GetMemberValue<TProperty>(instance);
            if (Equals(currentValue, default(TProperty)))
            {
                var defaultValue = defaultValueOrFactory switch
                {
                    Func<TProperty> factory => factory(),
                    Func<T, TProperty> factory => factory(instance),
                    null => default,
                    _ => (TProperty)defaultValueOrFactory
                };

                SetMemberValue(instance, defaultValue);
            }
        };
    }

    internal void SetCollectionAction<TProperty, TElementProperty>(object? defaultValueOrFactory)
    {
        Action = instance =>
        {
            var collection = GetCollectionValue<TProperty>(instance);
            if (!Equals(collection, default(TProperty)))
            {
                foreach (var element in collection!)
                {
                    var childValue = GetChildValue<TProperty, TElementProperty>(element);
                    if (Equals(childValue, default(TElementProperty)))
                    {
                        var defaultValue = defaultValueOrFactory switch
                        {
                            Func<TElementProperty> factory => factory(),
                            Func<T, TElementProperty> factory => factory(instance),
                            Func<TProperty, TElementProperty> factory => factory(element),
                            null => default,
                            _ => (TElementProperty)defaultValueOrFactory
                        };

                        SetChildValue(element, defaultValue);
                    }
                }
            }
        };
    }
}
