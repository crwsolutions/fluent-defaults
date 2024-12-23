using System.Linq.Expressions;
using System.Reflection;

namespace FluentDefaults;

public interface IDefaultRule<T>
{
    void Apply(T instance);
}

public abstract class AbstractDefaulter<T> : IDefaultRule<T>
{
    private readonly List<Action<T>> _defaultActions = new();

    public void Apply(T instance)
    {
        foreach (var action in _defaultActions)
        {
            action(instance);
        }
    }
    
    protected void DefaultFor<TProperty>(Expression<Func<T, TProperty>> expression, TProperty defaultValue)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                var getMethod = propertyInfo.GetGetMethod();
                var setMethod = propertyInfo.GetSetMethod();

                if (getMethod != null && setMethod != null)
                {
                    _defaultActions.Add(instance =>
                    {
                        var currentValue = (TProperty?)getMethod.Invoke(instance, null);
                        if (Equals(currentValue, default(TProperty)))
                        {
                            setMethod.Invoke(instance, [defaultValue]);
                        }
                    });
                }
            }
            else if (memberExpression.Member is FieldInfo fieldInfo)
            {
                _defaultActions.Add(instance =>
                {
                    var currentValue = (TProperty?)fieldInfo.GetValue(instance);
                    if (Equals(currentValue, default(TProperty)))
                    {
                        fieldInfo.SetValue(instance, defaultValue);
                    }
                });
            }
        }
    }
}
