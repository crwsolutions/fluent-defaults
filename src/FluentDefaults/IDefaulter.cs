namespace FluentDefaults;

public interface IDefaulter<T>
{
    void Apply(T target);
}
