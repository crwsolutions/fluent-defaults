using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentDefaults;

internal class Rule<T>
{
    internal Action<T>? Action { get; set; }

    internal Func<T, Task>? ActionAsync { get; set; }

    internal Func<T, bool>? Condition { get; private set; }

    internal MemberExpression MemberExpression { get; set; }

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

    internal void SetAsyncAction<TProperty>(
        Delegate defaultFactory
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
                        var defaultValue = defaultFactory switch
                        {
                            Func<Task<TProperty>> factory => await factory(),
                            Func<T, Task<TProperty>> factory => await factory(instance),
                            _ => default
                        };
                        setMethod.Invoke(instance, [defaultValue]);
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
                    var defaultValue = defaultFactory switch
                    {
                        Func<Task<TProperty>> factory => await factory(),
                        Func<T, Task<TProperty>> factory => await factory(instance),
                        _ => default
                    };
                    fieldInfo.SetValue(instance, defaultValue);
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
                        var defaultValue = defaultValueOrFactory switch
                        {
                            Func<TProperty> factory => factory(),
                            Func<T, TProperty> factory => factory(instance),
                            null => default,
                            _ => (TProperty)defaultValueOrFactory
                        };

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
                    var defaultValue = defaultValueOrFactory switch
                    {
                        Func<TProperty> factory => factory(),
                        Func<T, TProperty> factory => factory(instance),
                        null => default,
                        _ => (TProperty)defaultValueOrFactory
                    };

                    fieldInfo.SetValue(instance, defaultValue);
                }
            };
        }
        else
        {
            throw new Exception("Member not found");
        }
    }

    internal void SetCollectionAction<TProperty, TElementProperty>(object? defaultValueOrFactory)
    {
        if (MemberExpression?.Member is PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();

            if (ChildMemberExpression!.Member is PropertyInfo childPropertyInfo)
            {
                var childGetMethod = childPropertyInfo.GetGetMethod();
                var childSetMethod = childPropertyInfo.GetSetMethod();

                if (getMethod != null && childGetMethod != null && childSetMethod != null)
                {
                    Action = instance =>
                    {
                        var list = (IEnumerable<TProperty>?)getMethod.Invoke(instance, null);
                        if (!Equals(list, default(TProperty)))
                        {
                            var getMethod = propertyInfo.GetGetMethod();
                            foreach (var element in list!)
                            {
                                var childValue = (TElementProperty?)childGetMethod.Invoke(element, null);
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

                                    childSetMethod.Invoke(element, [defaultValue]);
                                }
                            }
                        }
                    };
                }
            }
        }
    }
}
