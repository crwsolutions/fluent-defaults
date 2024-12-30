namespace FluentDefaults;

public interface IAsyncDefaulter<T>
{
    Task ApplyAsync(T target);
}