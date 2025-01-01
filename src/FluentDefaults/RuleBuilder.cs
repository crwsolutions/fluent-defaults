using System.Reflection;

namespace FluentDefaults;

/// <summary>
/// Provides a builder for defining rules that apply default values to properties or fields.
/// </summary>
public class RuleBuilder<T, TProperty> : BaseRuleBuilder<T, TProperty>
{
    internal RuleBuilder(Rule<T> rule) : base(rule) { }

    /// <summary>
    /// Sets a defaulter to apply default values to the specified property or field.
    /// </summary>
    /// <param name="defaulter">The defaulter to apply to the property or field.</param>
    /// <exception cref="Exception">Thrown if the property or field does not have a get method or if the member is not found.</exception>
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
