using System.Reflection;

namespace FluentDefaults;

public class RuleBuilder<T, TProperty> : BaseRuleBuilder<T, TProperty>
{
    internal RuleBuilder(Rule<T> rule) : base(rule) { }

    public void SetDefaulter(AbstractDefaulter<TProperty> defaulter)
    {
        if (_rule.MemberExpression?.Member is PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();

            if (getMethod != null)
            {
                _rule.Action = instance =>
                {
                    var currentValue = (TProperty?)getMethod.Invoke(instance, null);
                    if (!Equals(currentValue, default(TProperty)))
                    {
                        defaulter.Apply(currentValue!);
                    }
                };
            }
            else
            {
                throw new Exception("Property has no set and get method");
            }
        }
        else if (_rule.MemberExpression?.Member is FieldInfo fieldInfo)
        {
            _rule.Action = instance =>
            {
                var currentValue = (TProperty?)fieldInfo.GetValue(instance);
                if (!Equals(currentValue, default(TProperty)))
                {
                    defaulter.Apply(currentValue!);
                }
            };
        }
        else
        {
            throw new Exception("Member not found");
        }
    }
}
