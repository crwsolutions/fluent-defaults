using System.Reflection;

namespace FluentDefaults;

public class DefaultForEachRuleBuilder<T, TProperty> : BaseRuleBuilder<T, TProperty>
{
    internal DefaultForEachRuleBuilder(Rule<T> rule) : base(rule) { }

    public void SetDefaulter(AbstractDefaulter<TProperty> defaulter)
    {
        if (_rule.MemberExpression?.Member is PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();

            if (getMethod != null)
            {
                _rule.Action = instance =>
                {
                    var currentValue = (IEnumerable<TProperty>?)
                        getMethod.Invoke(instance, null);
                    if (!Equals(currentValue, default(TProperty)))
                    {
                        foreach (var element in currentValue!)
                        {
                            defaulter.Apply(element);
                        }
                    }
                };
            }
        }
    }
}