using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentDefaults;

internal sealed class Rule<T>
{
    internal Action<T>? Action { get; set; }

    internal Func<T, Task>? ActionAsync { get; set; }

    internal Func<T, bool>? Condition { get; private set; }

    internal MemberExpression MemberExpression { get; set; }

    internal IEnumerable<TProperty>? GetCollectionValue<TProperty>(T instance)
    {
        var (target, member) = GetTargetAndMember(MemberExpression, instance);
        return (IEnumerable<TProperty>?)(member as PropertyInfo)?.GetGetMethod()?.Invoke(target, null);
    }

    private TElementProperty? GetChildValue<TProperty, TElementProperty>(TProperty element)
    {
        var (target, member) = GetTargetAndMember(ChildMemberExpression!, element);

        return member switch
        {
            PropertyInfo prop => (TElementProperty?)prop.GetValue(target),
            FieldInfo field => (TElementProperty?)field.GetValue(target),
            _ => throw new Exception("Unsupported member type")
        };
    }

    private void SetChildValue<TProperty, TElementProperty>(TProperty element, TElementProperty? value)
    {
        var (target, member) = GetTargetAndMember(ChildMemberExpression!, element);

        switch (member)
        {
            case PropertyInfo prop:
                prop.GetSetMethod()?.Invoke(target, [value]);
                break;
            case FieldInfo field:
                field.SetValue(target, value);
                break;
            default:
                throw new Exception("Unsupported member type");
        }
    }

    internal TProperty? GetMemberValue<TProperty>(T instance)
    {
        var (target, member) = GetTargetAndMember(MemberExpression, instance);
        return member switch
        {
            PropertyInfo prop => (TProperty?)prop.GetValue(target),
            FieldInfo field => (TProperty?)field.GetValue(target),
            _ => throw new Exception("Unsupported member type")
        };
    }

    private void SetMemberValue<TProperty>(T instance, TProperty? value)
    {
        var (target, member) = GetTargetAndMember(MemberExpression, instance);
        switch (member)
        {
            case PropertyInfo prop:
                prop.GetSetMethod()?.Invoke(target, [value]);
                break;
            case FieldInfo field:
                field.SetValue(target, value);
                break;
            default:
                throw new Exception("Member is not a Property or Field");
        }
    }


    private static (object? target, MemberInfo member) GetTargetAndMember(Expression memberExpression, object? instance)
    {
        object? current = instance;
        var stack = new Stack<MemberExpression>();

        Expression? expr = memberExpression;
        while (expr is MemberExpression m)
        {
            stack.Push(m);
            expr = m.Expression;
        }

        while (stack.Count > 1)
        {
            var m = stack.Pop();
            current = m.Member switch
            {
                PropertyInfo prop => prop.GetValue(current),
                FieldInfo field => field.GetValue(current),
                _ => throw new Exception("Unsupported member type")
            };
        }

        var lastMember = stack.Pop();
        return (current, lastMember.Member);
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
                if (!(collection is ICollection || collection is Array))
                {
                    // Throw a custom exception with member information
                    throw new DeferredExecutionException($"{MemberExpression} => {ChildMemberExpression}");
                }

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
